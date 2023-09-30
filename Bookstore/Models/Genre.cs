using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    [Index("Name", IsUnique = true)]
    public class Genre
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;


        // navigation property
        public virtual ICollection<Book> Books { get; set; } = null!;
    }
}
