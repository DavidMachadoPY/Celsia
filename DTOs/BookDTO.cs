namespace ServeBooks.DTOs
{
    public class BookDTO
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Gender { get; set; }
        public DateTime? PublicationDate { get; set; } = DateTime.Now;
        public int NumberOfCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string? Status { get; set; }
    }

}