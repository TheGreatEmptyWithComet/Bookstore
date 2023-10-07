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
using System.ComponentModel;
using System.Windows.Data;

namespace Bookstore
{
    public class BookPageVM : NotifyPropertyChangeHandler
    {
        #region Delegates & Events
        /****************************************************************************************/
        public delegate void BookActionNeeded(Book book);
        public static event BookActionNeeded? OnNewArrivalNeeded;
        public static event BookActionNeeded? OnNewSaleNeeded;
        public static event BookActionNeeded? OnNewReserveNeeded;
        #endregion


        #region Properties
        /****************************************************************************************/
        private readonly Context context;
        // variable is used to prevent some data checking while they are edited
        private bool editDataMode = false;
        private BookDataWindow bookDataWindow;
        // View model for window binding
        public BookVM CurrentBook { get; private set; }

        //public ObservableCollection<BookVM> Books { get { return new ObservableCollection<BookVM>(allBooks.Select(i => new BookVM(i))); } }
        // DB row data
        private List<Book> allBooks;
        // Data for WPF
        public ICollectionView Books { get; private set; }

        private BookVM selectedBook;
        public BookVM SelectedBook
        {
            get => selectedBook;
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
            get => errorMessage;
            set
            {
                errorMessage = value;
                NotifyPropertyChanged(nameof(ErrorMessage));
            }
        }
        // Properties for serch
        private bool searchIsVisible = false;
        public bool SearchIsVisible
        {
            get => searchIsVisible;
            set
            {
                searchIsVisible = value;
                NotifyPropertyChanged(nameof(SearchIsVisible));
            }
        }
        private string bookNameForSerach = string.Empty;
        public string BookNameForSerach
        {
            get => bookNameForSerach;
            set
            {
                bookNameForSerach = value;
                NotifyPropertyChanged(nameof(BookNameForSerach));
            }
        }
        private AuthorVM? authorForSearch;
        public AuthorVM? AuthorForSearch
        {
            get => authorForSearch;
            set
            {
                if (authorForSearch != value)
                {
                    authorForSearch = value;
                    NotifyPropertyChanged(nameof(AuthorForSearch));
                }
            }
        }
        private GenreVM? gengeForSearch;
        public GenreVM? GengeForSearch
        {
            get => gengeForSearch;
            set
            {
                if (gengeForSearch != value)
                {
                    gengeForSearch = value;
                    NotifyPropertyChanged(nameof(GengeForSearch));
                }
            }
        }
        public bool IsNewArrivalForSearch { get; set; } = false;
        #endregion


        #region Commands
        /****************************************************************************************/
        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand ClearCampaingCommand { get; private set; }
        public ICommand ClearPrequelBookCommand { get; private set; }
        public ICommand AddNewArrivalCommand { get; private set; }
        public ICommand AddNewSaleCommand { get; private set; }
        public ICommand AddNewReserveCommand { get; private set; }
        public ICommand ChangeSearchVisibilityCommand { get; private set; }
        public ICommand ClearSearchBookNameCommand { get; private set; }
        public ICommand ClearSearchAuthorCommand { get; private set; }
        public ICommand ClearSearchGenreCommand { get; private set; }
        public ICommand StartSearchCommand { get; private set; }
        #endregion


        #region Constructor
        /****************************************************************************************/
        public BookPageVM(Context context)
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
            ClearCampaingCommand = new RelayCommand(ClearCampaing);
            ClearPrequelBookCommand = new RelayCommand(ClearPrequelBook);
            AddNewArrivalCommand = new RelayCommand(AddNewArrival);
            AddNewSaleCommand = new RelayCommand(AddNewSale);
            AddNewReserveCommand = new RelayCommand(AddNewReserve);
            ChangeSearchVisibilityCommand = new RelayCommand(ChangeSearchVisibility);
            ClearSearchBookNameCommand = new RelayCommand(() => { BookNameForSerach = string.Empty; });
            ClearSearchAuthorCommand = new RelayCommand(() => { AuthorForSearch = null; });
            ClearSearchGenreCommand = new RelayCommand(() => { GengeForSearch = null; });
            StartSearchCommand = new RelayCommand(StartSearch);
        }
        private void AddNewBook()
        {
            // Create new user
            Book newBook = new Book() { PublicationYear = DateTime.Now };
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
            Books = CollectionViewSource.GetDefaultView(new ObservableCollection<BookVM>(allBooks!.Select(i => new BookVM(i))));
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
            Books.Refresh();
        }
        private void ClearCampaing()
        {
            CurrentBook.Campaing = null;
            Books.Refresh();
        }
        private void ClearPrequelBook()
        {
            CurrentBook.PrequelBook = null;
            Books.Refresh();
        }
        private void AddNewArrival()
        {
            // invoke Add method from ArrivalPageVM
            OnNewArrivalNeeded?.Invoke(SelectedBook.Model);
            // refresh the view to update amount values in datagrid tabel
            Books.Refresh();
        }
        private void AddNewSale()
        {
            // invoke Add method from SalePageVM
            OnNewSaleNeeded?.Invoke(SelectedBook.Model);
            // refresh the view to update amount values in datagrid tabel
            Books.Refresh();
        }
        private void AddNewReserve()
        {
            // invoke Add method from ReservePageVM
            OnNewReserveNeeded?.Invoke(SelectedBook.Model);
            // refresh the view to update amount values in datagrid tabel
            Books.Refresh();
        }
        private void ChangeSearchVisibility()
        {
            if (SearchIsVisible)
            {
                SearchIsVisible = false;
            }
            else
            {
                SearchIsVisible = true;
            }
        }
        private void StartSearch()
        {
            Books.Filter = BookFolterCriteries;
            Books.Refresh();
        }
        private bool BookFolterCriteries(object obj)
        {
            if (obj is BookVM book)
            {
                bool nameCondition = string.IsNullOrEmpty(BookNameForSerach) ? true : book.Name.Contains(BookNameForSerach);
                bool authorCondition = AuthorForSearch == null ? true : book.Author.Equals(AuthorForSearch);
                bool genreCondition = GengeForSearch == null ? true : book.Genre.Equals(GengeForSearch);
                bool newArrivalCondition = IsNewArrivalForSearch == false ? true : book.IsNewArrival;

                return nameCondition && authorCondition && genreCondition && newArrivalCondition;
            }

            return true;
        }
        #endregion
    }
}
