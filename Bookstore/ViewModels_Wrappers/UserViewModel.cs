using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class UserViewModel : NotifyPropertyChangeHandler
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
                    OnPropertyChanged(nameof(Login));
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
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public UserViewModel(User user)
        {
            Model = user;
        }
    }
}
