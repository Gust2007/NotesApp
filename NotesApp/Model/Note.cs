﻿using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Model
{
    public class Note : INotifyPropertyChanged
    {
        // Note for Azure Port: Property Id must be a string
        private int id;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { id = value;
                OnPropertyChanged("Id");
            }
        }

        // Note for Azure Port: Property notebookId must be a string
        private int notebookId;

        [Indexed]
        public int NotebookId
        {
            get { return notebookId; }
            set { notebookId = value;
                OnPropertyChanged("NotebookId");
            }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value;
                OnPropertyChanged("Title");
            }
        }

        private DateTime createdTime;

        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value;
                OnPropertyChanged("CreatedTime");
            }
        }

        private DateTime updatedTime;

        public DateTime UpdatedTime
        {
            get { return updatedTime; }
            set { updatedTime = value;
                OnPropertyChanged("UpdatedTime");
            }
        }

        private string fileLocation;

        public string FileLocation
        {
            get { return fileLocation; }
            set { fileLocation = value;
                OnPropertyChanged("FileLocation");
            }
        }

        private bool isEditing;

        [Ignore]
        public bool IsEditing
        {
            get { return isEditing; }
            set { 
                isEditing = value;
                OnPropertyChanged("IsEditing");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyChanged)
        {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyChanged));
            }
        }
    }
}
