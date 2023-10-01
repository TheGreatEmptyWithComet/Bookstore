using Bookstore.View;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Bookstore
{
    public class AuthorPageVM : NotifyPropertyChangeHandler
    {
        #region Properties
        /****************************************************************************************/
        private readonly Context context;
        // variable is used to prevent some data checking while they are edited
        private bool editDataMode = false;
        private AuthorDataWindow authorDataWindow;
        // View model for window binding
        public AuthorVM CurrentAuthor { get; private set; }

        // DB row data
        private List<Author> allAuthors;
        // Data for WPF
        public ObservableCollection<AuthorVM> Authors { get { return new ObservableCollection<AuthorVM>(allAuthors.Select(i => new AuthorVM(i))); } }

        private AuthorVM selectedAuthor;
        public AuthorVM SelectedAuthor
        {
            get
            {
                return selectedAuthor;
            }
            set
            {
                if (selectedAuthor != value)
                {
                    selectedAuthor = value;
                    NotifyPropertyChanged(nameof(SelectedAuthor));
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
        public AuthorPageVM(Context context)
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
            AddCommand = new RelayCommand(AddNewAuthor);
            SaveCommand = new RelayCommand(CheckData);
            EditCommand = new RelayCommand(EditAuthor);
            DeleteCommand = new RelayCommand(DeleteAuthor);
        }
        private void AddNewAuthor()
        {
            // Create new user
            Author newAuthor = new Author();
            CurrentAuthor = new AuthorVM(newAuthor);
            ErrorMessage = string.Empty;

            // Create and show window
            authorDataWindow = new AuthorDataWindow();
            authorDataWindow.Owner = Application.Current.MainWindow;

            if (authorDataWindow.ShowDialog() == true)
            {
                context.Add(newAuthor);
                allAuthors.Add(newAuthor);
                SaveChanges();
            }
        }
        private void EditAuthor()
        {
            editDataMode = true;

            // Create edited user
            Author editedAuthor = new Author()
            {
                FirstName = SelectedAuthor.FirstName,
                LastName = SelectedAuthor.LastName,
                MiddleName = SelectedAuthor.MiddleName,
            };
            CurrentAuthor = new AuthorVM(editedAuthor);
            ErrorMessage = string.Empty;

            // Create and show window
            authorDataWindow = new AuthorDataWindow();
            authorDataWindow.Owner = Application.Current.MainWindow;

            if (authorDataWindow.ShowDialog() == true)
            {
                // change data
                SelectedAuthor.FirstName = editedAuthor.FirstName;
                SelectedAuthor.LastName = editedAuthor.LastName;
                SelectedAuthor.MiddleName = editedAuthor.MiddleName;

                editDataMode = false;

                // update db
                SaveChanges();
            }
        }
        private void DeleteAuthor()
        {
            // remove record
            context.Remove(SelectedAuthor.Model);
            allAuthors.Remove(SelectedAuthor.Model);

            // update db
            SaveChanges();
        }
        private void CheckData()
        {
            // check first name
            if (String.IsNullOrEmpty(CurrentAuthor.FirstName))
            {
                ErrorMessage = "First name must not be empty";
                return;
            }
            // check last name
            else if (String.IsNullOrEmpty(CurrentAuthor.LastName))
            {
                ErrorMessage = "Password must not be empty";
                return;
            }
            authorDataWindow.DialogResult = true;
            authorDataWindow.Close();
        }

        public void LoadDataFromDB()
        {
            allAuthors = context.Authors.ToList();
            NotifyPropertyChanged(nameof(Authors));
        }
        private void SaveChanges()
        {
            try
            {
                context.SaveChanges();
                LoadDataFromDB();
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
