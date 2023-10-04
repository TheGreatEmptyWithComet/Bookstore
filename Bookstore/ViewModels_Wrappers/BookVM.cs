using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class BookVM : NotifyPropertyChangeHandler
    {
        // Properties
        /****************************************************************************************/
        public Book Model { get; set; }

        public int Id { get => Model.Id; }

        public string Name
        {
            get => Model.Name;
            set
            {
                if (Model.Name != value)
                {
                    Model.Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public AuthorVM Author
        {
            get => new AuthorVM(Model.Author);
            set
            {
                if (value != null && value.Model != Model.Author)
                {
                    Model.Author = value.Model;
                    NotifyPropertyChanged(nameof(Author));
                }
            }
        }
        public PublisherVM Publisher
        {
            get => new PublisherVM(Model.Publisher);
            set
            {
                if (value != null && value.Model != Model.Publisher)
                {
                    Model.Publisher = value.Model;
                    NotifyPropertyChanged(nameof(Book));
                }
            }
        }
        public int PagesNumber
        {
            get => Model.PagesNumber;
            set
            {
                if (Model.PagesNumber != value)
                {
                    Model.PagesNumber = value;
                    NotifyPropertyChanged(nameof(PagesNumber));
                }
            }
        }
        public GenreVM Genre
        {
            get => new GenreVM(Model.Genre);
            set
            {
                if (value != null && value.Model != Model.Genre)
                {
                    Model.Genre = value.Model;
                    NotifyPropertyChanged(nameof(Genre));
                }
            }
        }
        public DateTime PublicationYear
        {
            get => Model.PublicationYear;
            set
            {
                if (Model.PublicationYear != value)
                {
                    Model.PublicationYear = value;
                    NotifyPropertyChanged(nameof(PublicationYear));
                }
            }
        }
        public double SalesPrice
        {
            get => Model.SalesPrice;
            set
            {
                if (Model.SalesPrice != value)
                {
                    Model.SalesPrice = value;
                    NotifyPropertyChanged(nameof(SalesPrice));
                }
            }
        }
        public CampaingVM? Campaing
        {
            get => Model.Campaing != null ? new CampaingVM(Model.Campaing) : null;
            set
            {
                if (value != null && value.Model != Model.Campaing)
                {
                    Model.Campaing = value.Model;
                    NotifyPropertyChanged(nameof(Campaing));
                }
            }
        }
        public bool IsSequel
        {
            get => Model.IsSequel ?? false;
            set
            {
                if (Model.IsSequel != value)
                {
                    Model.IsSequel = value;
                    NotifyPropertyChanged(nameof(IsSequel));
                }
            }
        }
        public BookVM? PrequelBook
        {
            get => Model.PrequelBook != null ? new BookVM(Model.PrequelBook) : null;
            set
            {
                if (value != null && value.Model != Model.PrequelBook)
                {
                    Model.PrequelBook = value.Model;
                    NotifyPropertyChanged(nameof(PrequelBook));
                }
            }
        }
        public bool IsNewArrival
        {
            get => Model.IsNewArrival ?? false;
            set
            {
                if (Model.IsNewArrival != value)
                {
                    Model.IsNewArrival = value;
                    NotifyPropertyChanged(nameof(IsNewArrival));
                }
            }
        }
        public CustomerVM? ReservedForCustomer
        {
            get => Model.ReservedForCustomer != null ? new CustomerVM(Model.ReservedForCustomer) : null;
            set
            {
                if (value != null && value.Model != Model.ReservedForCustomer)
                {
                    Model.ReservedForCustomer = value.Model;
                    NotifyPropertyChanged(nameof(ReservedForCustomer));
                }
            }
        }
        public BookVM? OriginBook
        {
            get => Model.OriginBook != null ? new BookVM(Model.OriginBook) : null;
            set
            {
                if (value != null && value.Model != Model.OriginBook)
                {
                    Model.OriginBook = value.Model;
                    NotifyPropertyChanged(nameof(OriginBook));
                }
            }
        }

        // Calculated properties
        /****************************************************************************************/
        public int StockAmount
        {
            // Stock amount = Arrivals - Sales - Reserves
            get => Model.Arrivals.Where((arr) => arr.Book.Id == Model.Id).Sum((arr) => arr.Amount) -
                Model.Sales.Where((sale) => sale.Book.Id == Model.Id).Sum((sale) => sale.Amount) -
                Model.Reserves.Where((res) => res.Book.Id == Model.Id).Sum((res) => res.Amount);
        }


        // Constructor
        /****************************************************************************************/
        public BookVM(Book book)
        {
            Model = book;
        }


        // Methods
        /****************************************************************************************/
        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not BookVM bookVM || bookVM.Model == null)
            {
                return false;
            }
            return Model.Id.Equals((obj as BookVM)!.Model.Id);
        }
    }
}
