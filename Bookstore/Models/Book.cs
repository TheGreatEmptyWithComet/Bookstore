using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class Book
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        public virtual Author Author { get; set; } = null!;
        public virtual Publisher Publisher { get; set; } = null!;
        public int PagesNumber { get; set; }
        public virtual Genre Genre { get; set; } = null!;
        public DateTime PublicationYear { get; set; }
        public double SalesPrice { get; set; }
        public virtual Campaing? Campaing { get; set; }
        public bool? IsSequel { get; set; }
        [ForeignKey("BookAsPrequel")]
        public virtual Book? PrequelBook { get; set; }
        public bool? IsNewArrival { get; set; }
        public virtual Customer? ReservedForCustomer { get; set; }
        // reference to the origin book when book is reserved
        [ForeignKey("BookAsOriginReference")]
        //public virtual Book? OriginBook { get; set; }


        // navigation properties
        public virtual ICollection<Arrival> Arrivals { get; set; } = new ObservableCollection<Arrival>();
        public virtual ICollection<Sale> Sales { get; set; } = new ObservableCollection<Sale>();
        public virtual ICollection<Reserve> Reserves { get; set; } = new ObservableCollection<Reserve>();
    }
}
