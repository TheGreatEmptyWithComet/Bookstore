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
    public class ArrivalPageVM : NotifyPropertyChangeHandler
    {
        #region Properties
        /****************************************************************************************/
        private readonly Context context;
        // variable is used to prevent some data checking while they are edited
        private bool editDataMode = false;
        private ArrivalDataWindow arrivalDataWindow;
        // View model for window binding
        public ArrivalVM CurrentArrival { get; private set; }

        // DB row data
        private List<Arrival> allArrivals;
        // Data for WPF
        public ObservableCollection<ArrivalVM> Arrivals { get { return new ObservableCollection<ArrivalVM>(allArrivals.Select(i => new ArrivalVM(i))); } }

        private ArrivalVM selectedArrival;
        public ArrivalVM SelectedArrival
        {
            get
            {
                return selectedArrival;
            }
            set
            {
                if (selectedArrival != value)
                {
                    selectedArrival = value;
                    NotifyPropertyChanged(nameof(SelectedArrival));
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
        public ArrivalPageVM(Context context)
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
            AddCommand = new RelayCommand(AddNewArrival);
            SaveCommand = new RelayCommand(CheckData);
            EditCommand = new RelayCommand(EditArrival);
            DeleteCommand = new RelayCommand(DeleteArrival);
        }
        private void AddNewArrival()
        {
            // Create new arrival
            Arrival newArrival = new Arrival() { DateTime = DateTime.Now, User = LoginVM.CurrentUser };
            CurrentArrival = new ArrivalVM(newArrival);
            ErrorMessage = string.Empty;

            // Create and show window
            arrivalDataWindow = new ArrivalDataWindow();
            arrivalDataWindow.Owner = Application.Current.MainWindow;

            if (arrivalDataWindow.ShowDialog() == true)
            {
                context.Add(newArrival);
                allArrivals.Add(newArrival);
                SaveChanges();
            }
        }
        private void EditArrival()
        {
            editDataMode = true;

            // Create edited arrival
            Arrival editedArrival = new Arrival()
            {
                Book = SelectedArrival.Model.Book,
                DateTime = SelectedArrival.Date,
                Amount = SelectedArrival.Amount,
                Cost = SelectedArrival.Cost
            };
            CurrentArrival = new ArrivalVM(editedArrival);
            ErrorMessage = string.Empty;

            // Create and show window
            arrivalDataWindow = new ArrivalDataWindow();
            arrivalDataWindow.Owner = Application.Current.MainWindow;

            if (arrivalDataWindow.ShowDialog() == true)
            {
                // change data
                SelectedArrival.Model.Book = editedArrival.Book;
                SelectedArrival.Date = editedArrival.DateTime;
                SelectedArrival.Amount = editedArrival.Amount;
                SelectedArrival.Cost = editedArrival.Cost;

                editDataMode = false;

                // update db
                SaveChanges();
            }
        }
        private void DeleteArrival()
        {
            // remove record
            context.Remove(SelectedArrival.Model);
            allArrivals.Remove(SelectedArrival.Model);

            // update db
            SaveChanges();
        }
        private void CheckData()
        {
            // check amount
            if (CurrentArrival.Amount <= 0)
            {
                ErrorMessage = "Amount must be bigger than 0";
                return;
            }
            // check cost
            else if (CurrentArrival.Cost <= 0)
            {
                ErrorMessage = "Cost must be bigger than 0";
                return;
            }
            arrivalDataWindow.DialogResult = true;
            arrivalDataWindow.Close();
        }

        public void LoadDataFromDB()
        {
            allArrivals = context.Arrivals.ToList();
            NotifyPropertyChanged(nameof(Arrivals));
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
