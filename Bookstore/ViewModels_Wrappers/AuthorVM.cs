using System;
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


        public AuthorVM(Author author)
        {
            Model = author;
        }
    }
}
