﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public DateOnly PublicationYear { get; set; }
        public Double SalesPrice { get; set; }
        public virtual Campaing? Campaing { get; set; }
        public bool? IsSequel { get; set; }
        public virtual Book? PrequelBook { get; set; }
        public bool? IsNewArrival { get; set; }
        public virtual Customer? ReservedForCustomer { get; set; }
        // reference to the origin book when book is reserved
        public virtual Book? OriginBook { get; set; }


        // navigation properties
        public virtual ICollection<Arrival> Arrivals { get; set; } = null!;
        public virtual ICollection<Sale> Sales { get; set; } = null!;
        public virtual ICollection<Reserve> Reserves { get; set; } = null!;
    }
}
