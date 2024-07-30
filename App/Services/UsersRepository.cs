using Microsoft.EntityFrameworkCore;
using System.Net;
using AutoMapper;
using ServeBooks.DTOs;
using ServeBooks.Models;
using ServeBooks.App.Interfaces;
using ServeBooks.Data;

namespace ServeBooks.App.Services
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ServeBooksContext _context;
        private readonly IMapper _mapper;
        public UsersRepository(ServeBooksContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(User user, string message, HttpStatusCode statusCode)> Update(int id, UserDTO user)
        {
            var userUpdate = await _context.Users.FindAsync(id);
            if (userUpdate!= null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                _mapper.Map(user, userUpdate);
                _context.Entry(userUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return (userUpdate, "The user has been updated correctly.", HttpStatusCode.OK);
            }
            else
                return (default(User)!, $"No user found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(IEnumerable<User> users, string message, HttpStatusCode statusCode)> GetAll()
        {
            var users = await _context.Users.Include(u => u.Loans).ToListAsync();
            if (users.Any())
                return (users, "Users have been successfully obtained.", HttpStatusCode.OK);
            else
                return (Enumerable.Empty<User>(), "No users found in the database.", HttpStatusCode.NotFound);
        }

        public async Task<(User user, string message, HttpStatusCode statusCode)> GetById(int id)
        {
            var user = await _context.Users.Include(u => u.Loans).FirstOrDefaultAsync(u => u.Id.Equals(id));
            if (user != null)
                return (user, "User has been successfully obtained.", HttpStatusCode.OK);
            else
                return (default(User)!, $"No user found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }     
    }
}