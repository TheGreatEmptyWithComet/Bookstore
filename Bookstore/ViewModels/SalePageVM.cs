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
    public class SalePageVM : NotifyPropertyChangeHandler
    {
        #region Properties
        /****************************************************************************************/
        private readonly Context context;
        // variable is used to prevent some data checking while they are edited
        private bool editDataMode = false;
        private SaleDataWindow saleDataWindow;
        // View model for window binding
        public SaleVM CurrentSale { get; private set; }

        // DB row data
        private List<Sale> allSales;
        // Data for WPF
        public ObservableCollection<SaleVM> Sales { get { return new ObservableCollection<SaleVM>(allSales.Select(i => new SaleVM(i))); } }

        private SaleVM selectedSale;
        public SaleVM SelectedSale
        {
            get
            {
                return selectedSale;
            }
            set
            {
                if (selectedSale != value)
                {
                    selectedSale = value;
                    NotifyPropertyChanged(nameof(SelectedSale));
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
        public SalePageVM(Context context)
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
            AddCommand = new RelayCommand(AddNewSale);
            SaveCommand = new RelayCommand(CheckData);
            EditCommand = new RelayCommand(EditSale);
            DeleteCommand = new RelayCommand(DeleteSale);
        }
        private void AddNewSale()
        {
            // Create new sale
            Sale newSale = new Sale() { DateTime = DateTime.Now };
            CurrentSale = new SaleVM(newSale);
            ErrorMessage = string.Empty;

            // Create and show window
            saleDataWindow = new SaleDataWindow();
            saleDataWindow.Owner = Application.Current.MainWindow;

            if (saleDataWindow.ShowDialog() == true)
            {
                context.Add(newSale);
                allSales.Add(newSale);
                SaveChanges();
            }
        }
        private void EditSale()
        {
            editDataMode = true;

            // Create edited sale
            Sale editedSale = new Sale()
            {
                Book = SelectedSale.Model.Book,
                DateTime = SelectedSale.Date,
                Amount = SelectedSale.Amount,
                Price = SelectedSale.Price
            };
            CurrentSale = new SaleVM(editedSale);
            ErrorMessage = string.Empty;

            // Create and show window
            saleDataWindow = new SaleDataWindow();
            saleDataWindow.Owner = Application.Current.MainWindow;

            if (saleDataWindow.ShowDialog() == true)
            {
                // change data
                SelectedSale.Model.Book = editedSale.Book;
                SelectedSale.Date = editedSale.DateTime;
                SelectedSale.Amount = editedSale.Amount;
                SelectedSale.Price = editedSale.Price;

                editDataMode = false;

                // update db
                SaveChanges();
            }
        }
        private void DeleteSale()
        {
            // remove record
            context.Remove(SelectedSale.Model);
            allSales.Remove(SelectedSale.Model);

            // update db
            SaveChanges();
        }
        private void CheckData()
        {
            // check amount
            if (CurrentSale.Amount <= 0)
            {
                ErrorMessage = "Amount must be bigger than 0";
                return;
            }
            // check cost
            else if (CurrentSale.Price <= 0)
            {
                ErrorMessage = "Price must be bigger than 0";
                return;
            }
            saleDataWindow.DialogResult = true;
            saleDataWindow.Close();
        }

        public void LoadDataFromDB()
        {
            allSales = context.Sales.ToList();
            NotifyPropertyChanged(nameof(Sales));
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
