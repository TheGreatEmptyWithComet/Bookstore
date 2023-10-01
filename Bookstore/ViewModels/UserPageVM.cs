using Bookstore.View;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Bookstore.LoginVM;

namespace Bookstore
{
    public class UserPageVM : NotifyPropertyChangeHandler
    {
        #region Delegates and events
        /****************************************************************************************/
        //public delegate void NotifyUserChanged(UserPageVM userPageViewModel);
        //public event NotifyUserChanged OnNotifyUserChanged;
        #endregion


        #region Properties
        /****************************************************************************************/
        private readonly Context context;
        // variable is used to prevent some data checking while they are edited
        private bool editDataMode = false;
        private UserDataWindow userDataWindow;
        // View model for window binding
        public UserVM CurrentUser { get; private set; }

        // DB row data
        private List<User> allUsers;
        // Data for WPF
        public ObservableCollection<UserVM> Users { get { return new ObservableCollection<UserVM>(allUsers.Select(i => new UserVM(i))); } }

        private UserVM selectedUser;
        public UserVM SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                if (selectedUser != value)
                {
                    selectedUser = value;
                    NotifyPropertyChanged(nameof(SelectedUser));
                }
            }
        }

        // Error message for data window
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
        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        #endregion


        #region Constructor
        /****************************************************************************************/
        public UserPageVM(Context context)
        {
            this.context = context;
            LoadDataFromDB();
            InitCommands();
        }
        #endregion


        #region Methods
        /****************************************************************************************/
        private void InitCommands()
        {
            AddCommand = new RelayCommand(AddNewUser);
            SaveCommand = new RelayCommand(CheckData);
            EditCommand = new RelayCommand(EditUser);
            DeleteCommand = new RelayCommand(DeleteUser);
        }
        private void AddNewUser()
        {
            // Create new user
            User newUser = new User();
            CurrentUser = new UserVM(newUser);
            ErrorMessage = string.Empty;

            // Create and show window
            userDataWindow = new UserDataWindow();
            userDataWindow.Owner = Application.Current.MainWindow;

            if (userDataWindow.ShowDialog() == true)
            {
                context.Add(newUser);
                allUsers.Add(newUser);
                SaveChanges();
            }
        }
        private void EditUser()
        {
            editDataMode = true;

            // Create edited user
            User editedUser = new User()
            {
                Login = SelectedUser.Login,
                Password = SelectedUser.Password
            };
            CurrentUser = new UserVM(editedUser);
            ErrorMessage = string.Empty;

            // Create and show window
            userDataWindow = new UserDataWindow();
            userDataWindow.Owner = Application.Current.MainWindow;

            if (userDataWindow.ShowDialog() == true)
            {
                // change data
                SelectedUser.Login = editedUser.Login;
                SelectedUser.Password = editedUser.Password;

                editDataMode = false;

                // update db
                SaveChanges();
            }
        }
        private void DeleteUser()
        {
            // remove user except last
            if (Users.Count > 1)
            {
                context.Remove(SelectedUser.Model);
                allUsers.Remove(SelectedUser.Model);
                
                // update db
                SaveChanges();
            }
            else
            {
                MessageBox.Show("The last user can't be deleted");
            }
        }
        private void CheckData()
        {
            // check login
            if (allUsers.Any(user => user.Login == CurrentUser.Login))
            {
                // Allow save login in a case it wasn't changed
                if (!(editDataMode && CurrentUser.Login==SelectedUser.Login))
                {
                    ErrorMessage = "Such a login is already exists";
                    return;
                }
            }
            else if (String.IsNullOrEmpty(CurrentUser.Login))
            {
                ErrorMessage = "Login must not be empty";
                return;
            }
            // password
            else if (String.IsNullOrEmpty(CurrentUser.Password))
            {
                ErrorMessage = "Password must not be empty";
                return;
            }
            userDataWindow.DialogResult = true;
            userDataWindow.Close();
        }

        public void LoadDataFromDB()
        {
            allUsers = context.Users.ToList();
            NotifyPropertyChanged(nameof(Users));
        }
        private void SaveChanges()
        {
            try
            {
                context.SaveChanges();
                LoadDataFromDB();
                //OnNotifyUserChanged?.Invoke(this);
            }
            catch (Exception ex)
            {
                string innerMessage = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
                MessageBox.Show(ex.Message + "\n" + innerMessage, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
