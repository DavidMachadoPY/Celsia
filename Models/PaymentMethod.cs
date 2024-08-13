using System.ComponentModel.DataAnnotations;

namespace ServeBooks.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Method name is required.")]
        public string? MethodName { get; set; }

        public ICollection<Transaction>? Transactions { get; set; } // Relación con la entidad Transaction
    }
}
