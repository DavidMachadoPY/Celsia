using System.Net;
using ServeBooks.Models;

namespace ServeBooks.App.Interfaces.Auth
{
    public interface IAuthService
    {
        // Registers a new user with the provided details
        Task<(User user, string message, HttpStatusCode statusCode)> RegisterUser(string name, string email, string password);

        // Authenticates a user with the provided credentials
        Task<(User login, string message, HttpStatusCode statusCode)> AuthenticateUser(string email, string password);

        // Generate an authentication token for a specific user
        (string token, string message) GenerateAuthToken(User user);
    }
}