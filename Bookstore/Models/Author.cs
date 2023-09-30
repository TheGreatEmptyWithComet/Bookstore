using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class Author
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        [StringLength(100)]
        public string? MiddleName { get; set; }


        // navigation property
        public virtual ICollection<Book> Books { get; set; } = null!;
    }
}
