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
    public class BooksVM : NotifyPropertyChangeHandler
    {
        #region Properties
        /****************************************************************************************/
        private readonly Context context;
        // variable is used to prevent some data checking while they are edited
        private bool editDataMode = false;
        private BookDataWindow bookDataWindow;
        // View model for window binding
        public BookVM CurrentBook { get; private set; }

        // DB row data
        private List<Book> allBooks;
        // Data for WPF
        public ObservableCollection<BookVM> Books { get { return new ObservableCollection<BookVM>(allBooks.Select(i => new BookVM(i))); } }

        private BookVM selectedBook;
        public BookVM SelectedBook
        {
            get
            {
                return selectedBook;
            }
            set
            {
                if (selectedBook != value)
                {
                    selectedBook = value;
                    NotifyPropertyChanged(nameof(SelectedBook));
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
        public BooksVM(Context context)
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
            AddCommand = new RelayCommand(AddNewBook);
            SaveCommand = new RelayCommand(CheckData);
            EditCommand = new RelayCommand(EditBook);
            DeleteCommand = new RelayCommand(DeleteBook);
        }
        private void AddNewBook()
        {
            // Create new user
            Book newBook = new Book();
            CurrentBook = new BookVM(newBook);
            ErrorMessage = string.Empty;

            // Create and show window
            bookDataWindow = new BookDataWindow();
            bookDataWindow.Owner = Application.Current.MainWindow;

            if (bookDataWindow.ShowDialog() == true)
            {
                context.Add(newBook);
                allBooks.Add(newBook);
                SaveChanges();
            }
        }
        private void EditBook()
        {
            editDataMode = true;

            // Create edited user
            Book editedBook = new Book()
            {
                Name = SelectedBook.Name,
                Author = SelectedBook.Model.Author,
                Publisher = SelectedBook.Model.Publisher,
                PagesNumber = SelectedBook.PagesNumber,
                Genre = SelectedBook.Model.Genre,
                PublicationYear = SelectedBook.PublicationYear,
                SalesPrice = SelectedBook.SalesPrice,
                Campaing = SelectedBook.Model.Campaing,
                IsSequel = SelectedBook.IsSequel,
                PrequelBook = SelectedBook.Model.PrequelBook,
                IsNewArrival = SelectedBook.IsNewArrival,
                ReservedForCustomer = SelectedBook.Model.ReservedForCustomer,
                OriginBook = SelectedBook.Model.OriginBook
            };
            CurrentBook = new BookVM(editedBook);
            ErrorMessage = string.Empty;

            // Create and show window
            bookDataWindow = new BookDataWindow();
            bookDataWindow.Owner = Application.Current.MainWindow;

            if (bookDataWindow.ShowDialog() == true)
            {
                // change data
                SelectedBook.Name = editedBook.Name;
                SelectedBook.Model.Author = editedBook.Author;
                SelectedBook.Model.Publisher = editedBook.Publisher;
                SelectedBook.PagesNumber = editedBook.PagesNumber;
                SelectedBook.Model.Genre = editedBook.Genre;
                SelectedBook.PublicationYear = editedBook.PublicationYear;
                SelectedBook.SalesPrice = editedBook.SalesPrice;
                SelectedBook.Model.Campaing = editedBook.Campaing;
                SelectedBook.IsSequel = editedBook.IsSequel ?? false;
                SelectedBook.Model.PrequelBook = editedBook.PrequelBook;
                SelectedBook.IsNewArrival = editedBook.IsNewArrival ?? false;
                SelectedBook.Model.ReservedForCustomer = editedBook.ReservedForCustomer;
                SelectedBook.Model.OriginBook = editedBook.OriginBook;

                editDataMode = false;

                // update db
                SaveChanges();
            }
        }
        private void DeleteBook()
        {
            // remove record
            context.Remove(SelectedBook.Model);
            allBooks.Remove(SelectedBook.Model);

            // update db
            SaveChanges();
        }
        private void CheckData()
        {
            // check name
            if (string.IsNullOrEmpty(CurrentBook.Name))
            {
                ErrorMessage = "Name must not be empty";
                return;
            }
            // check price
            else if (CurrentBook.SalesPrice < 0)
            {
                ErrorMessage = "Price must be bigger or equal than 0";
                return;
            }
            bookDataWindow.DialogResult = true;
            bookDataWindow.Close();
        }

        public void LoadDataFromDB()
        {
            allBooks = context.Books.ToList();
            NotifyPropertyChanged(nameof(Books));
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
