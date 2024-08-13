using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ServeBooks.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        
        [Required(ErrorMessage = "Registration Date is required")]
        public DateTime? RegistrationDate { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string? Phone { get; set; }
        public string? Role { get; set; }

        public DateTime? CreatedAt { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        public ICollection<Transaction>? Transactions { get; set; } // Relación con la entidad Transaction
        public ICollection<Invoice>? Invoices { get; set; } // Relación con la entidad Invoice
    }
}
