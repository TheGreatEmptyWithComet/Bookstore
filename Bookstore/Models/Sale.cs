using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class Sale
    {
        public int Id { get; set; }
        public virtual Book Book { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public int Amount { get; set; }
        // actual price at which the book was sold, while price in Book class can be changed any time
        public double Price { get; set; }


        // navigation property
        public virtual User User { get; set; } = null!;
    }
}
