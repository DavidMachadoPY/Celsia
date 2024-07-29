using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServeBooks.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Gender { get; set; }
        public DateOnly? PublicationDate { get; set; }
        public int NumberOfCopies { get; set; }
        public int AvailableCopies { get; set; }
        [JsonIgnore]
        public ICollection<Loan>? Loans { get; set; }
    }
}