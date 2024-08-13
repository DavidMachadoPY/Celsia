using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServeBooks.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        public int UserId { get; set; } // Relacionada con la tabla de Customers

        [Required(ErrorMessage = "Invoice date is required.")]
        public DateTime InvoiceDate { get; set; }

        [Required(ErrorMessage = "Total amount is required.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string? Status { get; set; }
        public User? User { get; set; } 
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
