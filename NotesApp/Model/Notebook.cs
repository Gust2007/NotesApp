using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using SQLite;

namespace NotesApp.Model
{
    public class Notebook : INotifyPropertyChanged
    {
        private int id;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private int userId;

        [Indexed]
        public int UserId
        {
            get { return userId; }
            set {
                userId = value;
                OnPropertyChanged("UserId");
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set {
                name = value;
                OnPropertyChanged("Name");
            }
        }


        private bool isEditing;
        public bool IsEditing
        {
            get {
                return isEditing;
            }

            set {
                isEditing = value;
                OnPropertyChanged("IsEditing");
            }
        }



        public Notebook()
        {
            IsEditing = false;
        }

        public void StartEditing()
        {
            IsEditing = true;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
