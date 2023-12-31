﻿using Bookstore.View;
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
    public class ReservePageVM : NotifyPropertyChangeHandler
    {
        #region Properties
        /****************************************************************************************/
        private readonly Context context;
        // variable is used to prevent some data checking while they are edited
        private bool editDataMode = false;
        private ReserveDataWindow reserveDataWindow;
        // View model for window binding
        public ReserveVM CurrentReserve { get; private set; }

        // DB row data
        private List<Reserve> allReserves;
        // Data for WPF
        public ObservableCollection<ReserveVM> Reserves { get { return new ObservableCollection<ReserveVM>(allReserves.Select(i => new ReserveVM(i))); } }

        private ReserveVM selectedReserve;
        public ReserveVM SelectedReserve
        {
            get
            {
                return selectedReserve;
            }
            set
            {
                if (selectedReserve != value)
                {
                    selectedReserve = value;
                    NotifyPropertyChanged(nameof(SelectedReserve));
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
        public ReservePageVM(Context context)
        {
            this.context = context;
            LoadDataFromDB();
            InitCommands();

            // subscribe to BookPageVM event to make it able to invoke this class AddNewreserve method from there
            BookPageVM.OnNewReserveNeeded += AddNewReserve;
        }
        #endregion


        #region Methods
        /****************************************************************************************/
        private void InitCommands()
        {
            AddCommand = new RelayCommand<Book>(AddNewReserve);
            SaveCommand = new RelayCommand(CheckData);
            EditCommand = new RelayCommand(EditReserve);
            DeleteCommand = new RelayCommand(DeleteReserve);
        }
        private void AddNewReserve(Book? alreadySelectedBook = null)
        {
            // Create new reserve
            Reserve newReserve = new Reserve() { DateTime = DateTime.Now, User = LoginVM.CurrentUser, Book = new Book() };
            if (alreadySelectedBook != null)
            {
                newReserve.Book = alreadySelectedBook;
            }
            CurrentReserve = new ReserveVM(newReserve);
            ErrorMessage = string.Empty;

            // Create and show window
            reserveDataWindow = new ReserveDataWindow();
            reserveDataWindow.Owner = Application.Current.MainWindow;

            if (reserveDataWindow.ShowDialog() == true)
            {
                context.Add(newReserve);
                allReserves.Add(newReserve);
                SaveChanges();
            }
        }
        private void EditReserve()
        {
            editDataMode = true;

            // Create edited reserve
            Reserve editedReserve = new Reserve()
            {
                Book = SelectedReserve.Model.Book,
                DateTime = SelectedReserve.Date,
                Amount = SelectedReserve.Amount,
            };
            CurrentReserve = new ReserveVM(editedReserve);
            ErrorMessage = string.Empty;

            // Create and show window
            reserveDataWindow = new ReserveDataWindow();
            reserveDataWindow.Owner = Application.Current.MainWindow;

            if (reserveDataWindow.ShowDialog() == true)
            {
                // change data
                SelectedReserve.Model.Book = editedReserve.Book;
                SelectedReserve.Date = editedReserve.DateTime;
                SelectedReserve.Amount = editedReserve.Amount;

                editDataMode = false;

                // update db
                SaveChanges();
            }
        }
        private void DeleteReserve()
        {
            // remove record
            context.Remove(SelectedReserve.Model);
            allReserves.Remove(SelectedReserve.Model);

            // update db
            SaveChanges();
        }
        private void CheckData()
        {
            // check book
            if (CurrentReserve.Book.Model == null)
            {
                ErrorMessage = "Select a book";
                return;
            }
            // check amount
            else if (CurrentReserve.Amount <= 0)
            {
                ErrorMessage = "Amount must be bigger than 0";
                return;
            }
            else if (editDataMode == false && CurrentReserve.Book != null && CurrentReserve.Amount > CurrentReserve.Book.AvailableToSaleAmount)
            {
                ErrorMessage = "Amount to reserve is bigger than available amount";
                return;
            }
            // if reserve is edited, it takes into consideration the old reserve amount when check the available stock exceeding
            else if (editDataMode == true && CurrentReserve.Book != null && CurrentReserve.Amount > CurrentReserve.Book.AvailableToSaleAmount + SelectedReserve.Amount)
            {
                ErrorMessage = "Amount to reserve is bigger than available amount";
                return;
            }

            reserveDataWindow.DialogResult = true;
            reserveDataWindow.Close();
        }

        public void LoadDataFromDB()
        {
            allReserves = context.Reserves.ToList();
            NotifyPropertyChanged(nameof(Reserves));
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
