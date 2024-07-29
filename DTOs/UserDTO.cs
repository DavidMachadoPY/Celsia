namespace ServeBooks.DTOs
{
    public class UserDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public byte[]? Password { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}