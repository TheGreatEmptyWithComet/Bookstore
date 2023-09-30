using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-E3P7TGH;Initial Catalog=Bookstore;Integrated Security=True;Persist Security Info=False;Pooling=False;Connect Timeout=60;Encrypt=False");
        }

        // Models
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Campaing> Campaings { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Arrival> Arrivals { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<Reserve> Reserves { get; set; }

    }
}
