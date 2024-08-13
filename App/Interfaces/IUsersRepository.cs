using System.Net;
using ServeBooks.DTOs;
using ServeBooks.Models;

namespace ServeBooks.App.Interfaces
{
    public interface IUsersRepository
    {
        Task<(User user, string message, HttpStatusCode statusCode)> Update(int id, UserDTO user);
        Task<(IEnumerable<User> users, string message, HttpStatusCode statusCode)> GetAll();
        Task<(User user, string message, HttpStatusCode statusCode)> GetById(int id);
    }
}
