namespace ServeBooks.DTOs
{
    public class TransactionDTO
    {
        public int UserID { get; set; }
        public int InvoiceId { get; set; } // Renombrado desde BookId para reflejar la relación con Invoice
        public DateTime TransactionDate { get; set; } = DateTime.Now; // Renombrado desde LoanDate
        public string? Status { get; set; } // Manteniendo el campo Status sin cambios
    }
}
