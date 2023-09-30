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
    public class LoginViewModel : NotifyPropertyChangeHandler
    {
        #region Properties
        //////////////////////////////////////////////////////////////////////////////////////////
        private readonly Context context;
        private LoginWindow loginWindow;

        // DB row data
        private List<User> allUsers;
        // Data for WPF
        public ObservableCollection<UserViewModel> Users { get { return new ObservableCollection<UserViewModel>(allUsers.Select(i => new UserViewModel(i))); } }

        public UserViewModel CurrentUser { get; private set; }

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
                    OnPropertyChanged(nameof(IsAdminMode));
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
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        #endregion


        #region Commands
        //////////////////////////////////////////////////////////////////////////////////////////
        public ICommand LoginCommand { get; private set; }
        public ICommand CheckUserDataCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        #endregion


        #region Constructor
        //////////////////////////////////////////////////////////////////////////////////////////
        public LoginViewModel(Context context)
        {
            this.context = context;

            // !!! temporary assignment
            isAdminMode = true;

            LoadDataFromDB();

            LoginCommand = new RelayCommand(Login);
            CheckUserDataCommand = new RelayCommand(CheckUserData);
            LogoutCommand = new RelayCommand(Logout);
        }
        #endregion


        #region Methods
        //////////////////////////////////////////////////////////////////////////////////////////
        private void Login()
        {
            UserLogin = string.Empty;
            UserPassword = string.Empty;

            loginWindow = new LoginWindow();
            loginWindow.Owner = Application.Current.MainWindow;

            if (loginWindow.ShowDialog() == true)
            {
                // login is unique value and user existance is checked in CheckUserData() method
                CurrentUser = Users.Where(user => user.Login == UserLogin).FirstOrDefault()!;
                IsAdminMode = true;
            }
        }
        private void CheckUserData()
        {
            // check login
            if (!allUsers.Any(user => user.Login == UserLogin))
            {
                ErrorMessage = "No such a login exists";
                return;
            }
            // password
            else if (!allUsers.Any(user => user.Login == UserLogin && user.Password == UserPassword))
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
        }

        public void LoadDataFromDB()
        {
            allUsers = context.Users.ToList();
            OnPropertyChanged(nameof(Users));
        }

        #endregion
    }
}
