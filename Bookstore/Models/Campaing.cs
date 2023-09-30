using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class Campaing
    {
        public class Genre
        {
            public int Id { get; set; }
            [StringLength(200)]
            public string Name { get; set; } = string.Empty;
            public double DiscountPercent { get; set; }


            // navigation property
            public virtual ICollection<Book> Books { get; set; } = null!;
        }
}
