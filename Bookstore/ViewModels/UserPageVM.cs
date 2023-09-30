using Bookstore.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.View
{
    public class UserPageVM : NotifyPropertyChangeHandler
    {
        #region Delegates and events
        //////////////////////////////////////////////////////////////////////////////////////////
        public delegate void NotifyUserChanged(UserPageVM userPageViewModel);
        public event NotifyUserChanged OnNotifyUserChanged;
        #endregion

        #region Properties
        //////////////////////////////////////////////////////////////////////////////////////////
        private readonly Context context;
        //private LoginWindow loginWindow;

        // DB row data
        private List<User> allUsers;
        // Data for WPF
        public ObservableCollection<UserVM> Users { get { return new ObservableCollection<UserVM>(allUsers.Select(i => new UserVM(i))); } }

        public UserVM CurrentUser { get; private set; }

        // Mode to define app window content to be shown
        private bool isAdminMode = false;
        public bool IsAdminMode
        {
            get => isAdminMode;
            set
            {
                if (isAdminMode != value)
                {
                    isAdminMode = value;
                    NotifyPropertyChanged(nameof(IsAdminMode));
                }
            }
        }
        // Login and pass properties
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        // Error message
        private string errorMessage;
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
                NotifyPropertyChanged(nameof(ErrorMessage));
            }
        }
        #endregion

        public UserPageVM()
        {

        }
    }
}
