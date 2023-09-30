using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{

    [Index("Login", IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;


        // navigation properties
        public virtual ICollection<Arrival> Arrivals { get; set; } = null!;
        public virtual ICollection<Sale> Sales { get; set; } = null!;
        public virtual ICollection<Reserve> Reserves { get; set; } = null!;
    }
}
