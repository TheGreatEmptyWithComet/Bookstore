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
    public class PublisherPageVM : NotifyPropertyChangeHandler
    {
        #region Properties
        /****************************************************************************************/
        private readonly Context context;
        // variable is used to prevent some data checking while they are edited
        private bool editDataMode = false;
        private PublisherDataWindow publisherDataWindow;
        // View model for window binding
        public PublisherVM CurrentPublisher { get; private set; }

        // DB row data
        private List<Publisher> allPublishers;
        // Data for WPF
        public ObservableCollection<PublisherVM> Publishers { get { return new ObservableCollection<PublisherVM>(allPublishers.Select(i => new PublisherVM(i))); } }

        private PublisherVM selectedPublisher;
        public PublisherVM SelectedPublisher
        {
            get
            {
                return selectedPublisher;
            }
            set
            {
                if (selectedPublisher != value)
                {
                    selectedPublisher = value;
                    NotifyPropertyChanged(nameof(SelectedPublisher));
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
        public PublisherPageVM(Context context)
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
            AddCommand = new RelayCommand(AddNewPublisher);
            SaveCommand = new RelayCommand(CheckData);
            EditCommand = new RelayCommand(EditPublisher);
            DeleteCommand = new RelayCommand(DeletePublisher);
        }
        private void AddNewPublisher()
        {
            // Create new user
            Publisher newPublisher = new Publisher();
            CurrentPublisher = new PublisherVM(newPublisher);
            ErrorMessage = string.Empty;

            // Create and show window
            publisherDataWindow = new PublisherDataWindow();
            publisherDataWindow.Owner = Application.Current.MainWindow;

            if (publisherDataWindow.ShowDialog() == true)
            {
                context.Add(newPublisher);
                allPublishers.Add(newPublisher);
                SaveChanges();
            }
        }
        private void EditPublisher()
        {
            editDataMode = true;

            // Create edited user
            Publisher editedPublisher = new Publisher()
            {
                Name = SelectedPublisher.Name
            };
            CurrentPublisher = new PublisherVM(editedPublisher);
            ErrorMessage = string.Empty;

            // Create and show window
            publisherDataWindow = new PublisherDataWindow();
            publisherDataWindow.Owner = Application.Current.MainWindow;

            if (publisherDataWindow.ShowDialog() == true)
            {
                // change data
                SelectedPublisher.Name = editedPublisher.Name;

                editDataMode = false;

                // update db
                SaveChanges();
            }
        }
        private void DeletePublisher()
        {
            // remove record
            context.Remove(SelectedPublisher.Model);
            allPublishers.Remove(SelectedPublisher.Model);

            // update db
            SaveChanges();
        }
        private void CheckData()
        {
            // check first name
            if (String.IsNullOrEmpty(CurrentPublisher.Name))
            {
                ErrorMessage = "Name must not be empty";
                return;
            }
            publisherDataWindow.DialogResult = true;
            publisherDataWindow.Close();
        }

        public void LoadDataFromDB()
        {
            allPublishers = context.Publishers.ToList();
            NotifyPropertyChanged(nameof(Publishers));
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
