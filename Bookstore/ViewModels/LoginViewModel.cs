using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookstore
{
    public class LoginViewModel : NotifyPropertyChangeHandler
    {
        private readonly Context context;

        // Commands
        public ICommand LoginCommand;

        public LoginViewModel(Context context)
        {
            this.context = context;

            LoginCommand = new RelayCommand();
        }

        private void Login()
        {

        }


    }
}
