using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class SaleVM : NotifyPropertyChangeHandler
    {
        public Sale Model { get; set; }

        public int Id { get => Model.Id; }
        public BookVM Book
        {
            get => new BookVM(Model.Book);
            set
            {
                if (value != null && value.Model != Model.Book)
                {
                    Model.Book = value.Model;
                    NotifyPropertyChanged(nameof(Book));
                }
            }
        }
        public DateTime Date
        {
            get => Model.DateTime;
            set
            {
                if (Model.DateTime != value)
                {
                    Model.DateTime = value;
                    NotifyPropertyChanged(nameof(Date));
                }
            }
        }
        public int Amount
        {
            get => Model.Amount;
            set
            {
                if (Model.Amount != value)
                {
                    Model.Amount = value;
                    NotifyPropertyChanged(nameof(Amount));
                }
            }
        }
        public double Price
        {
            get => Model.Price;
            set
            {
                if (Model.Price != value)
                {
                    Model.Price = value;
                    NotifyPropertyChanged(nameof(Price));
                }
            }
        }


        public SaleVM(Sale sale)
        {
            Model = sale;
        }
    }
}
