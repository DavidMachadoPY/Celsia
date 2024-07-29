using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ServeBooks.Data
{
    public class ServeBooksContext : DbContext 
    {
        public ServeBooksContext(DbContextOptions<ServeBooksContext> options) : base (options){
            
        }
    }
}