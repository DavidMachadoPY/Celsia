using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServeBooks.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Invoice ID is required.")]
        public int InvoiceId { get; set; } // Relacionada con la tabla de Invoices

        [Required(ErrorMessage = "Payment method ID is required.")]
        public int PaymentMethodId { get; set; } // Relacionada con la tabla de PaymentMethods

        [Required(ErrorMessage = "Transaction date is required.")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction status is required.")]
        public string? TransactionStatus { get; set; }

        public Invoice? Invoice { get; set; } // Relación con la entidad Invoice
        public User? User { get; set; } // Relación con la entidad User
        public PaymentMethod? PaymentMethod { get; set; } // Relación con la entidad PaymentMethod
    }
}
