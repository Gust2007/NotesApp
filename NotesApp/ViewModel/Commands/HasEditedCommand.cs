using NotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesApp.ViewModel.Commands
{
    public class HasEditedCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public NotesVM VM { get; set; }

        public HasEditedCommand(NotesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is Notebook noteBook && noteBook.IsEditing)
            {
                noteBook.IsEditing = false;
                VM.HasRenamed(noteBook);
            }
        }
    }
}
