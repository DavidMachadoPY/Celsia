namespace ServeBooks.DTOs
{
    public class InvoiceDTO
    {
        public int UserId { get; set; } // Relacionada con la tabla de Customers

        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }

    }
}
