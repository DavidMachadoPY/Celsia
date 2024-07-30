using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ServeBooks.DTOs;

namespace ServeBooks.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title location is required.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Author location is required.")]
        public string? Author { get; set; }


        [Required(ErrorMessage = "Gender location is required.")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Publication date is required.")]
        public DateTime? PublicationDate { get; set; }

        [Required(ErrorMessage = "Number of copies is required.")]
        public int NumberOfCopies { get; set; }


        [Required(ErrorMessage = "Available copies is required.")]
        public int AvailableCopies { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string? Status { get; set; }
        //[JsonIgnore]
        public ICollection<Loan>? Loans { get; set; }
    }
}