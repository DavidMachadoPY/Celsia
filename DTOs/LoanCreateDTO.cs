using ServeBooks.Models;

namespace ServeBooks.DTOs
{
    public class LoanCreateDTO
    {
        public int UserID { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime ReturnDate { get; set; } = DateTime.Now;

        public Book? Book { get; set; }
    }
}