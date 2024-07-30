namespace ServeBooks.DTOs
{
    public class LoanDTO
    {
        public int UserID { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime ReturnDate { get; set; } = DateTime.Now;
        public string? Status { get; set; }
    }
}