using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ObservableCollection<Note> Notes { get; set; }


        private Notebook selectedNotebook;
        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set {
                selectedNotebook = value;
                OnPropertyChanged("SelectedNotebook");
                ReadNotes();
            }
        }



        private Note note;

        public Note SelectedNote
        {
            get { return note; }
            set {
                note = value;
                SelectedNoteChanged(this, new EventArgs());
                OnPropertyChanged("SelectedNote");
            }
        }




        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public BeginEditCommand BeginEditCommand { get; set; }
        public HasEditedCommand HasEditedCommand { get; set; }
        public CancelNotebookEditCommand CancelNotebookEditCommand { get; set; }
        public HasEditedNoteCommand HasEditedNoteCommand { get; set; }
        public CancelNoteEditCommand CancelNoteEditCommand { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SelectedNoteChanged;


        public NotesVM()
        {
            // create tables in db as first task
            DatabaseHelper.CreateTables<Notebook>();
            DatabaseHelper.CreateTables<Note>();

            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            BeginEditCommand = new BeginEditCommand(this);
            HasEditedCommand = new HasEditedCommand(this);
            CancelNotebookEditCommand = new CancelNotebookEditCommand();

            HasEditedNoteCommand = new HasEditedNoteCommand(this);
            CancelNoteEditCommand = new CancelNoteEditCommand();

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();
        }


        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public async void CreateNotebook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "New notebook",
                UserId = Int32.Parse(App.UserId)
            };

#if USEAZURE
            try
            {
                await App.MobileServiceClient.GetTable<Notebook>().InsertAsync(newNotebook);
            }
            catch (Exception ex)
            {

            }
#else
            DatabaseHelper.Insert(newNotebook);
#endif

            ReadNotebooks();
        }


        // Note for Azure Port: parameter notebookId must be a string
        public async void CreateNote(int notebookId)
        {
            Note newNote = new Note()
            {
                NotebookId = notebookId,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                Title = "New note"
            };

#if USEAZURE
            try
            {
                await App.MobileServiceClient.GetTable<Note>().InsertAsync(newNote);
            }
            catch (Exception ex)
            {

            }
#else
            DatabaseHelper.Insert(newNote);
#endif

            ReadNotes();
        }


        public async void ReadNotebooks()
        {
#if USEAZURE
            try
            {
                Notebooks.Clear();

                var notebooks = await App.MobileServiceClient.GetTable<Notebook>().Where(n => n.UserId == App.UserId).ToListAsync();

                foreach (var notebook in notebooks)
                {
                    Notebooks.Add(notebook);
                }
            }
            catch (Exception ex)
            {
            }
#else
            using (SQLiteConnection conn = new SQLiteConnection(DatabaseHelper.dbFile)) {
                var notebooks = conn.Table<Notebook>().ToList();

                Notebooks.Clear();

                foreach (var notebook in notebooks) {
                    Notebooks.Add(notebook);
                }
            }
#endif

        }


        public async void ReadNotes()
        {
#if USEAZURE
            try
            {
                Notes.Clear();

                var notes = await App.MobileServiceClient.GetTable<Note>().Where(n => n.NotebookId == SelectedNotebook.Id).ToListAsync();

                foreach (var note in notes)
                {
                    Notes.Add(note);
                }
            }
            catch (Exception ex)
            {
            }
#else
            using (SQLiteConnection conn = new SQLiteConnection(DatabaseHelper.dbFile)) {
                if (SelectedNotebook != null) {
                    var notes = conn.Table<Note>().Where(n => n.NotebookId == SelectedNotebook.Id);

                    Notes.Clear();
                    foreach (var note in notes) {
                        Notes.Add(note);
                    }
                }
            }
#endif
        }


        public async void HasRenamed(Notebook notebook)
        {
            if (notebook != null)
            {
#if USEAZURE
                try
                {
                    await App.MobileServiceClient.GetTable<Notebook>().UpdateAsync(notebook);
                }
                catch (Exception ex)
                {
                }
#else
                 DatabaseHelper.Update(notebook);
#endif

                ReadNotebooks();
            }
        }


        public async void HasRenamedNote(Note note)
        {
            if (note != null)
            {
#if USEAZURE
                try
                {
                    await App.MobileServiceClient.GetTable<Note>().UpdateAsync(note);
                }
                catch (Exception ex)
                {
                }
#else
                DatabaseHelper.Update(note);
#endif

                ReadNotes();
            }
        }


        public async void UpdateSelectedNote()
        {
            SelectedNote.UpdatedTime = DateTime.Now;

#if USEAZURE
            try
            {
                await App.MobileServiceClient.GetTable<Note>().UpdateAsync(SelectedNote);
            }
            catch (Exception ex)
            {
            }
#else
             DatabaseHelper.Update(SelectedNote);
#endif

        }
    }
}
