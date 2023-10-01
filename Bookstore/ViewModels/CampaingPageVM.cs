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
    public class CampaingPageVM : NotifyPropertyChangeHandler
    {
        #region Properties
        /****************************************************************************************/
        private readonly Context context;
        // variable is used to prevent some data checking while they are edited
        private bool editDataMode = false;
        private CampaingDataWindow campaingDataWindow;
        // View model for window binding
        public CampaingVM CurrentCampaing { get; private set; }

        // DB row data
        private List<Campaing> allCampaings;
        // Data for WPF
        public ObservableCollection<CampaingVM> Campaings { get { return new ObservableCollection<CampaingVM>(allCampaings.Select(i => new CampaingVM(i))); } }

        private CampaingVM selectedCampaing;
        public CampaingVM SelectedCampaing
        {
            get
            {
                return selectedCampaing;
            }
            set
            {
                if (selectedCampaing != value)
                {
                    selectedCampaing = value;
                    NotifyPropertyChanged(nameof(SelectedCampaing));
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
        public CampaingPageVM(Context context)
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
            AddCommand = new RelayCommand(AddNewCampaing);
            SaveCommand = new RelayCommand(CheckData);
            EditCommand = new RelayCommand(EditCampaing);
            DeleteCommand = new RelayCommand(DeleteCampaing);
        }
        private void AddNewCampaing()
        {
            // Create new user
            Campaing newCampaing = new Campaing();
            CurrentCampaing = new CampaingVM(newCampaing);
            ErrorMessage = string.Empty;

            // Create and show window
            campaingDataWindow = new CampaingDataWindow();
            campaingDataWindow.Owner = Application.Current.MainWindow;

            if (campaingDataWindow.ShowDialog() == true)
            {
                context.Add(newCampaing);
                allCampaings.Add(newCampaing);
                SaveChanges();
            }
        }
        private void EditCampaing()
        {
            editDataMode = true;

            // Create edited user
            Campaing editedCampaing = new Campaing()
            {
                Name = SelectedCampaing.Name,
                DiscountPercent = SelectedCampaing.DiscountPercent
            };
            CurrentCampaing = new CampaingVM(editedCampaing);
            ErrorMessage = string.Empty;

            // Create and show window
            campaingDataWindow = new CampaingDataWindow();
            campaingDataWindow.Owner = Application.Current.MainWindow;

            if (campaingDataWindow.ShowDialog() == true)
            {
                // change data
                SelectedCampaing.Name = editedCampaing.Name;
                SelectedCampaing.DiscountPercent = editedCampaing.DiscountPercent;

                editDataMode = false;

                // update db
                SaveChanges();
            }
        }
        private void DeleteCampaing()
        {
            // remove record
            context.Remove(SelectedCampaing.Model);
            allCampaings.Remove(SelectedCampaing.Model);

            // update db
            SaveChanges();
        }
        private void CheckData()
        {
            // check name
            if (String.IsNullOrEmpty(CurrentCampaing.Name))
            {
                ErrorMessage = "Name must not be empty";
                return;
            }
            // check discount
            else if (CurrentCampaing.DiscountPercent<=0)
            {
                ErrorMessage = "Discount percent must be bigger than 0";
                return;
            }
            campaingDataWindow.DialogResult = true;
            campaingDataWindow.Close();
        }

        public void LoadDataFromDB()
        {
            allCampaings = context.Campaings.ToList();
            NotifyPropertyChanged(nameof(Campaings));
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
