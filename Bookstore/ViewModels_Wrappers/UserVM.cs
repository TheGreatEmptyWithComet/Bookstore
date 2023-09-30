using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class UserVM : NotifyPropertyChangeHandler
    {
        public User Model { get; set; }

        public int Id { get => Model.Id; }

        public string Login
        {
            get => Model.Login;
            set
            {
                if (Model.Login != value)
                {
                    Model.Login = value;
                    NotifyPropertyChanged(nameof(Login));
                }
            }
        }
        public string Password
        {
            get => Model.Password;
            set
            {
                if (Model.Password != value)
                {
                    Model.Password = value;
                    NotifyPropertyChanged(nameof(Password));
                }
            }
        }

        public UserVM(User user)
        {
            Model = user;
        }
    }
}
