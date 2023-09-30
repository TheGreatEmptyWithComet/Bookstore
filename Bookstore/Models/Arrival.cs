using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class Arrival
    {
        public int Id { get; set; }
        public virtual Book Book { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public int Amount { get; set; }
        public double Cost { get; set; }


        // navigation property
        public virtual User User { get; set; } = null!;
    }
}
