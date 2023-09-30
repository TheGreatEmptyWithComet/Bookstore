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
        private readonly Context context = new Context();

        // mode to define app window content to be shown
        private bool isAdminMode = false;
        public bool IsAdminMode
        {
            get => isAdminMode;
            set
            {
                if (isAdminMode != value)
                {
                    isAdminMode = value;
                    OnPropertyChanged(nameof(IsAdminMode));
                }
            }
        }

    }
}
