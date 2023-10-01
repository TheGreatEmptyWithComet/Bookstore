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
    public class GenrePageVM : NotifyPropertyChangeHandler
    {
        #region Properties
        /****************************************************************************************/
        private readonly Context context;
        // variable is used to prevent some data checking while they are edited
        private bool editDataMode = false;
        private GenreDataWindow genreDataWindow;
        // View model for window binding
        public GenreVM CurrentGenre { get; private set; }

        // DB row data
        private List<Genre> allGenres;
        // Data for WPF
        public ObservableCollection<GenreVM> Genres { get { return new ObservableCollection<GenreVM>(allGenres.Select(i => new GenreVM(i))); } }

        private GenreVM selectedGenre;
        public GenreVM SelectedGenre
        {
            get
            {
                return selectedGenre;
            }
            set
            {
                if (selectedGenre != value)
                {
                    selectedGenre = value;
                    NotifyPropertyChanged(nameof(SelectedGenre));
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
        public GenrePageVM(Context context)
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
            AddCommand = new RelayCommand(AddNewGenre);
            SaveCommand = new RelayCommand(CheckData);
            EditCommand = new RelayCommand(EditGenre);
            DeleteCommand = new RelayCommand(DeleteGenre);
        }
        private void AddNewGenre()
        {
            // Create new user
            Genre newGenre = new Genre();
            CurrentGenre = new GenreVM(newGenre);
            ErrorMessage = string.Empty;

            // Create and show window
            genreDataWindow = new GenreDataWindow();
            genreDataWindow.Owner = Application.Current.MainWindow;

            if (genreDataWindow.ShowDialog() == true)
            {
                context.Add(newGenre);
                allGenres.Add(newGenre);
                SaveChanges();
            }
        }
        private void EditGenre()
        {
            editDataMode = true;

            // Create edited user
            Genre editedGenre = new Genre()
            {
                Name = SelectedGenre.Name
            };
            CurrentGenre = new GenreVM(editedGenre);
            ErrorMessage = string.Empty;

            // Create and show window
            genreDataWindow = new GenreDataWindow();
            genreDataWindow.Owner = Application.Current.MainWindow;

            if (genreDataWindow.ShowDialog() == true)
            {
                // change data
                SelectedGenre.Name = editedGenre.Name;

                editDataMode = false;

                // update db
                SaveChanges();
            }
        }
        private void DeleteGenre()
        {
            // remove record
            context.Remove(SelectedGenre.Model);
            allGenres.Remove(SelectedGenre.Model);

            // update db
            SaveChanges();
        }
        private void CheckData()
        {
            // check name if it is unique
            if (allGenres.Any(genre => genre.Name == CurrentGenre.Name))
            {
                // Allow save name in a case it wasn't changed
                if (!(editDataMode && CurrentGenre.Name == SelectedGenre.Name))
                {
                    ErrorMessage = "Such a name is already exists";
                    return;
                }
            }
            // check name if it is not empty
            else if (String.IsNullOrEmpty(CurrentGenre.Name))
            {
                ErrorMessage = "Name must not be empty";
                return;
            }
            genreDataWindow.DialogResult = true;
            genreDataWindow.Close();
        }

        public void LoadDataFromDB()
        {
            allGenres = context.Genres.ToList();
            NotifyPropertyChanged(nameof(Genres));
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
