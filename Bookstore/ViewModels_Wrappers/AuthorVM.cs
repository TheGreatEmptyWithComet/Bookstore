﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class AuthorVM : NotifyPropertyChangeHandler
    {
        public Author Model { get; set; }

        public int Id { get => Model.Id; }

        public string FirstName
        {
            get => Model.FirstName;
            set
            {
                if (Model.FirstName != value)
                {
                    Model.FirstName = value;
                    NotifyPropertyChanged(nameof(FirstName));
                }
            }
        }
        public string LastName
        {
            get => Model.LastName;
            set
            {
                if (Model.LastName != value)
                {
                    Model.LastName = value;
                    NotifyPropertyChanged(nameof(LastName));
                }
            }
        }
        public string MiddleName
        {
            get => Model.MiddleName ?? string.Empty;
            set
            {
                if (Model.MiddleName != null && Model.MiddleName != value)
                {
                    Model.MiddleName = value;
                    NotifyPropertyChanged(nameof(MiddleName));
                }
            }
        }


        // Calculated properties
        /****************************************************************************************/
        // Properties for statistic
        public int DaySalesAmount
        {
            get => (new List<BookVM>(Model.Books.Where((book) => book.Author.Equals(Model)).Select((i) => new BookVM(i)))).Sum((book) => book.DaySalesAmount);
        }
        public int WeekSalesAmount
        {
            get => (new List<BookVM>(Model.Books.Where((book) => book.Author.Equals(Model)).Select((i) => new BookVM(i)))).Sum((book) => book.WeekSalesAmount);
        }
        public int MonthSalesAmount
        {
            get => (new List<BookVM>(Model.Books.Where((book) => book.Author.Equals(Model)).Select((i) => new BookVM(i)))).Sum((book) => book.MonthSalesAmount);
        }
        public int YearSalesAmount
        {
            get => (new List<BookVM>(Model.Books.Where((book) => book.Author.Equals(Model)).Select((i) => new BookVM(i)))).Sum((book) => book.YearSalesAmount);
        }


        public AuthorVM(Author author)
        {
            Model = author;
        }


        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not AuthorVM authorVM || authorVM.Model == null)
            {
                return false;
            }
            return Model.Id.Equals((obj as AuthorVM)!.Model.Id);
        }
    }
}
