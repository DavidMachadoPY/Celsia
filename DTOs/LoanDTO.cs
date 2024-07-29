namespace ServeBooks.DTOs
{
    public class LoanDTO
    {
        public int UserID { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? Status { get; set; }
    }
}