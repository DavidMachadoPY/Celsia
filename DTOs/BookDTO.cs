namespace ServeBooks.DTOs
{
    public class BookDTO
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Gender { get; set; }
        public DateOnly? PublicationDate { get; set; }
        public int NumberOfCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string? Status { get; set; }
    }
}