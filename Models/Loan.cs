using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServeBooks.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? Status { get; set; }

        public User? User { get; set; }
        public Book? Book { get; set; }
    }
}