using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServeBooks.Models;

namespace ServeBooks.Data
{
    public class ServeBooksContext : DbContext 
    {
        public ServeBooksContext(DbContextOptions<ServeBooksContext> options) : base (options){
            
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<User> Users { get; set; }
    }
}