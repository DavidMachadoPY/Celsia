using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServeBooks.App.Interfaces.Auth;
using ServeBooks.Data;
using ServeBooks.Models;
using BCrypt.Net;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace ServeBooks.App.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ServeBooksContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ServeBooksContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<(User user, string message, HttpStatusCode statusCode)> RegisterUser(string name, string email, string password)
        {
            // Check if the email already exists
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return (null!, "Email already in use.", HttpStatusCode.Conflict);
            }

            // Hash the password using BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create the user
            var user = new User
            {
                Name = name,
                Email = email,
                Password = Encoding.UTF8.GetBytes(hashedPassword),
                RegistrationDate = DateTime.Now
            };

            // Add and save the user
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (user, "User registered successfully.", HttpStatusCode.Created);
        }

        public async Task<(User login, string message, HttpStatusCode statusCode)> AuthenticateUser(string email, string password)
        {
            var user = await _context.Users.Include(u => u.Loans!).SingleOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, Encoding.UTF8.GetString(user.Password!)))
            {
                return (null!, "The email or password is incorrect.", HttpStatusCode.NotFound);
            }

            return (user, "Email and password found, login successful.", HttpStatusCode.OK);
        }

        public (string token, string message) GenerateAuthToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, "Token generated successfully.");
        }
    }
}
