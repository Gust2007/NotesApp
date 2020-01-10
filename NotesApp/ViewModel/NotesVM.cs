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

            ReadNotebooks();
            ReadNotes();
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public void CreateNotebook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "New notebook",
                UserId = Int32.Parse(App.UserId)
            };

            DatabaseHelper.Insert(newNotebook);

            ReadNotebooks();
        }

        public void CreateNote(int notebookId)
        {
            Note newNote = new Note()
            {
                NotebookId = notebookId,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                Title = "New note"
            };

            DatabaseHelper.Insert(newNote);

            ReadNotes();
        }


        public void ReadNotebooks()
        {
            using (SQLiteConnection conn = new SQLiteConnection(DatabaseHelper.dbFile)) {
                var notebooks = conn.Table<Notebook>().ToList();

                Notebooks.Clear();

                foreach (var notebook in notebooks) {
                    Notebooks.Add(notebook);
                }
            }
        }

        public void ReadNotes()
        {
            using (SQLiteConnection conn = new SQLiteConnection(DatabaseHelper.dbFile)) {
                if (SelectedNotebook != null) {
                    var notes = conn.Table<Note>().Where(n => n.NotebookId == SelectedNotebook.Id);

                    Notes.Clear();
                    foreach (var note in notes) {
                        Notes.Add(note);
                    }
                }
            }
        }


        public void HasRenamed(Notebook notebook)
        {
            if (notebook != null)
            {
                DatabaseHelper.Update(notebook);
                ReadNotebooks();
            }
        }

        public void HasRenamedNote(Note note)
        {
            if (note != null)
            {
                DatabaseHelper.Update(note);
                ReadNotes();
            }
        }

        public void UpdateSelectedNote()
        {
            SelectedNote.UpdatedTime = DateTime.Now;
            DatabaseHelper.Update(SelectedNote);
        }
    }
}
