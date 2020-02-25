using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.ViewModel
{
    public class LoginVM
    {
        private Users user;

        public Users User
        {
            get { return user; }
            set { user = value; }
        }

        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }

        public event EventHandler HasLoggedIn;

        public LoginVM()
        {
            User = new Users();

            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);
        }

        public async void Login()
        {
            //using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            //{
            //    conn.CreateTable<Users>();

            //    var user = conn.Table<Users>().Where(u => u.Username == User.Username).FirstOrDefault();

            //    if (user.Password == User.Password)
            //    {
            //        App.UserId = user.Id.ToString();
            //        HasLoggedIn(this, new EventArgs());
            //    }
            //}

            try
            {
                var loginUser = (await App.MobileServiceClient.GetTable<Users>().Where(u => u.Username == User.Username).ToListAsync()).FirstOrDefault();

                if (loginUser.Password == User.Password)
                {
                    App.UserId = loginUser.Id;
                    HasLoggedIn(this, new EventArgs());
                }
            }
            catch(Exception ex)
            {

            }
        }


        public async void Register()
        {
            //using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            //{
            //    conn.CreateTable<User>();

            //    var result = DatabaseHelper.Insert(User);

            //    if (result)
            //    {
            //        App.UserId = user.Id.ToString();
            //        HasLoggedIn(this, new EventArgs());
            //    }

            //}

            try
            {
                await App.MobileServiceClient.GetTable<Users>().InsertAsync(User);
                App.UserId = User.Id.ToString();
                HasLoggedIn(this, new EventArgs());
            }
            catch (Exception ex)
            {

            }

        }
    }
}
