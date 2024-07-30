using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServeBooks.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public byte[]? Password { get; set; }
        
        [Required (ErrorMessage = "RegistrationDate is Required")]
        public DateTime? RegistrationDate { get; set; }
      
        public string? Role { get; set; }
      
        [JsonIgnore]
        public ICollection<Loan>? Loans { get; set; }
    }
}