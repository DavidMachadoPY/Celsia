using Microsoft.EntityFrameworkCore;
using ServeBooks.Models;

namespace ServeBooks.Data
{
    public class celsiaContext : DbContext 
    {
        public celsiaContext(DbContextOptions<celsiaContext> options) : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
