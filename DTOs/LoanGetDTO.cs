namespace ServeBooks.DTOs
{
    public class LoanGetDTO
    {
        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime ReturnDate { get; set; } = DateTime.Now;
        public string? Status { get; set; }
    }
}