using NotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesApp.ViewModel.Commands
{
    public class NewNoteCommand : ICommand
    {
        public NotesVM VM { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //Notebook selectedNotebook = parameter as Notebook;
            //if (selectedNotebook != null) {
            //    return true;
            //}

            return true;
        }

        public void Execute(object parameter)
        {
            Notebook selectedNotebook = parameter as Notebook;
            if (selectedNotebook != null)
            {
                VM.CreateNote(selectedNotebook.Id);
            }
        }

        public NewNoteCommand(NotesVM vm)
        {
            VM = vm;
        }

    }
}
