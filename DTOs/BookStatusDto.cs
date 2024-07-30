using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServeBooks.DTOs
{
    public class BookStatusDto
    {
         public string? Title { get; set; }
         public string? Status { get; set; }
         public DateOnly ReturnDate { get; set; } 
    }
}