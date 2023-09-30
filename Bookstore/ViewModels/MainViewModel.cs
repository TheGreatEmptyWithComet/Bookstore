using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class MainViewModel : NotifyPropertyChangeHandler
    {
        private readonly Context context;

        // Inner view models
        public LoginViewModel LoginViewModel { get; private set; }
        

       

        public MainViewModel()
        {
            context = new Context();

            LoginViewModel = new LoginViewModel(context);
        }

        

    }
}
