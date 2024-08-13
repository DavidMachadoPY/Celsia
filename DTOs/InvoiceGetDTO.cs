namespace ServeBooks.DTOs
{
    public class InvoiceGetDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Relacionada con la tabla de User
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }

    }
}
