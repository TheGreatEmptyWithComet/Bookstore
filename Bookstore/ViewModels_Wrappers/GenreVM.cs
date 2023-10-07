using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class GenreVM : NotifyPropertyChangeHandler
    {
        public Genre Model { get; set; }

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

        // Calculated properties
        /****************************************************************************************/
        // Properties for statistic
        public int DaySalesAmount
        {
            get => (new List<BookVM>(Model.Books.Where((book) => book.Genre.Equals(Model)).Select((i) => new BookVM(i)))).Sum((book) => book.DaySalesAmount);
        }
        public int WeekSalesAmount
        {
            get => (new List<BookVM>(Model.Books.Where((book) => book.Genre.Equals(Model)).Select((i) => new BookVM(i)))).Sum((book) => book.WeekSalesAmount);
        }
        public int MonthSalesAmount
        {
            get => (new List<BookVM>(Model.Books.Where((book) => book.Genre.Equals(Model)).Select((i) => new BookVM(i)))).Sum((book) => book.MonthSalesAmount);
        }
        public int YearSalesAmount
        {
            get => (new List<BookVM>(Model.Books.Where((book) => book.Genre.Equals(Model)).Select((i) => new BookVM(i)))).Sum((book) => book.YearSalesAmount);
        }



        public GenreVM(Genre genre)
        {
            Model = genre;
        }


        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not GenreVM genreVM || genreVM.Model == null)
            {
                return false;
            }
            return Model.Id.Equals((obj as GenreVM)!.Model.Id);
        }
    }
}
