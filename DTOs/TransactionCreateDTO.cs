using ServeBooks.Models;

namespace ServeBooks.DTOs
{
    public class TransactionCreateDTO
    {
        public int UserID { get; set; }
        public int InvoiceId { get; set; } 
        public int PaymentMethodId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now; // Renombrado desde LoanDate
        public decimal Amount { get; set; }       
    }
}
