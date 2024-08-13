using ServeBooks.Models;

namespace ServeBooks.DTOs
{
    public class TransactionGetDTO
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int InvoiceId { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string? TransactionStatus { get; set; }
        public PaymentMethod? PaymentMethods { get; set; }

    }
}
