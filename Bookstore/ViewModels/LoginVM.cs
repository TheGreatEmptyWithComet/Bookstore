using Bookstore.View;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Bookstore
{
    public class LoginVM : NotifyPropertyChangeHandler
    {
        #region Delegates and events
        /****************************************************************************************/
        public delegate void SetCurrentUser(User user);
        public event SetCurrentUser OnSetCurrentUser;

        public delegate void NotifyLogout();
        public event NotifyLogout OnNotifyLogout;
        #endregion


        #region Properties
        /****************************************************************************************/
        private readonly Context context;
        private LoginWindow loginWindow;

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


        #region Commands
        /****************************************************************************************/
        public ICommand LoginCommand { get; private set; }
        public ICommand CheckUserDataCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        #endregion


        #region Constructor
        /****************************************************************************************/
        public LoginVM(Context context)
        {
            this.context = context;

            // !!! temporary assignment
            isAdminMode = true;

            //LoadDataFromDB();

            LoginCommand = new RelayCommand(Login);
            CheckUserDataCommand = new RelayCommand(CheckUserData);
            LogoutCommand = new RelayCommand(Logout);

        }
        #endregion


        #region Methods
        /****************************************************************************************/
        private void Login()
        {
            UserLogin = string.Empty;
            UserPassword = string.Empty;

            loginWindow = new LoginWindow();
            loginWindow.Owner = Application.Current.MainWindow;

            if (loginWindow.ShowDialog() == true)
            {
                // login is unique value and user existance is checked in CheckUserData() method
                User currentUser = context.Users.Where(user => user.Login == UserLogin).FirstOrDefault()!;
                OnSetCurrentUser?.Invoke(currentUser);
                IsAdminMode = true;
            }
        }
        private void CheckUserData()
        {
            // check login
            if (!context.Users.Any(user => user.Login == UserLogin))
            {
                ErrorMessage = "No such a login exists";
                return;
            }
            // password
            else if (!context.Users.Any(user => user.Login == UserLogin && user.Password == UserPassword))
            {
                ErrorMessage = "Wrong passsword";
                return;
            }
            loginWindow.DialogResult = true;
            loginWindow.Close();
        }
        private void Logout()
        {
            IsAdminMode = false;
            OnNotifyLogout?.Invoke();
        }

        #endregion
    }
}
