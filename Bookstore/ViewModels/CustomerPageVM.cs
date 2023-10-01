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
    public class CustomerPageVM : NotifyPropertyChangeHandler
    {
        #region Properties
        /****************************************************************************************/
        private readonly Context context;
        // variable is used to prevent some data checking while they are edited
        private bool editDataMode = false;
        private CustomerDataWindow customerDataWindow;
        // View model for window binding
        public CustomerVM CurrentCustomer { get; private set; }

        // DB row data
        private List<Customer> allCustomers;
        // Data for WPF
        public ObservableCollection<CustomerVM> Customers { get { return new ObservableCollection<CustomerVM>(allCustomers.Select(i => new CustomerVM(i))); } }

        private CustomerVM selectedCustomer;
        public CustomerVM SelectedCustomer
        {
            get
            {
                return selectedCustomer;
            }
            set
            {
                if (selectedCustomer != value)
                {
                    selectedCustomer = value;
                    NotifyPropertyChanged(nameof(SelectedCustomer));
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
        public CustomerPageVM(Context context)
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
            AddCommand = new RelayCommand(AddNewCustomer);
            SaveCommand = new RelayCommand(CheckData);
            EditCommand = new RelayCommand(EditCustomer);
            DeleteCommand = new RelayCommand(DeleteCustomer);
        }
        private void AddNewCustomer()
        {
            // Create new user
            Customer newCustomer = new Customer();
            CurrentCustomer = new CustomerVM(newCustomer);
            ErrorMessage = string.Empty;

            // Create and show window
            customerDataWindow = new CustomerDataWindow();
            customerDataWindow.Owner = Application.Current.MainWindow;

            if (customerDataWindow.ShowDialog() == true)
            {
                context.Add(newCustomer);
                allCustomers.Add(newCustomer);
                SaveChanges();
            }
        }
        private void EditCustomer()
        {
            editDataMode = true;

            // Create edited user
            Customer editedCustomer = new Customer()
            {
                Name = SelectedCustomer.Name,
                PhoneNumber = SelectedCustomer.PhoneNumber
            };
            CurrentCustomer = new CustomerVM(editedCustomer);
            ErrorMessage = string.Empty;

            // Create and show window
            customerDataWindow = new CustomerDataWindow();
            customerDataWindow.Owner = Application.Current.MainWindow;

            if (customerDataWindow.ShowDialog() == true)
            {
                // change data
                SelectedCustomer.Name = editedCustomer.Name;
                SelectedCustomer.PhoneNumber = editedCustomer.PhoneNumber;

                editDataMode = false;

                // update db
                SaveChanges();
            }
        }
        private void DeleteCustomer()
        {
            // remove record
            context.Remove(SelectedCustomer.Model);
            allCustomers.Remove(SelectedCustomer.Model);

            // update db
            SaveChanges();
        }
        private void CheckData()
        {
            // check name
            if (String.IsNullOrEmpty(CurrentCustomer.Name))
            {
                ErrorMessage = "Name must not be empty";
                return;
            }
            // check number if it is unique
            else if (allCustomers.Any(customer => customer.PhoneNumber == CurrentCustomer.PhoneNumber))
            {
                // Allow save name in a case it wasn't changed
                if (!(editDataMode && CurrentCustomer.PhoneNumber == SelectedCustomer.PhoneNumber))
                {
                    ErrorMessage = "Such a number is already exists";
                    return;
                }
            }
            // check number if it is not empty
            else if (String.IsNullOrEmpty(CurrentCustomer.PhoneNumber))
            {
                ErrorMessage = "Phone number must not be empty";
                return;
            }
            customerDataWindow.DialogResult = true;
            customerDataWindow.Close();
        }

        public void LoadDataFromDB()
        {
            allCustomers = context.Customers.ToList();
            NotifyPropertyChanged(nameof(Customers));
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
