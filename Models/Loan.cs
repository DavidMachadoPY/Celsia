using System.ComponentModel.DataAnnotations;

namespace ServeBooks.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int BookId { get; set; }
        

        [Required(ErrorMessage = "Loan Date  is required.")]
        public DateTime LoanDate { get; set; }


        [Required(ErrorMessage = "Return Date  is required.")]
        public DateTime ReturnDate { get; set; }

        
        [Required(ErrorMessage = "Status is required.")]
        public string? Status { get; set; }
        public User? User { get; set; }
        public Book? Book { get; set; }
    }
}