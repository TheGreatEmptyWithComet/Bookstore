using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class PublisherVM : NotifyPropertyChangeHandler
    {
        public Publisher Model { get; set; }

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

        public PublisherVM(Publisher publisher)
        {
            Model = publisher;
        }
    }
}
