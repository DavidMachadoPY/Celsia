using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;
using ServeBooks.DTOs;
using ServeBooks.Models;

namespace ServeBooks.Controllers.Users
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    [Authorize(Roles = "Admin")]
    public class UsersUpdateController : ControllerBase
    {
        private readonly IUsersRepository _repository;
        public UsersUpdateController(IUsersRepository repository)
        {
            _repository = repository;
        }

        [HttpPut]
        [Route("api/users/{id}")]
        public async Task<IActionResult> Update(int id, UserDTO user)
        {
            var (result, message, statusCode) = await _repository.Update(id, user);
            return StatusCode((int)statusCode, new { Message = message });
        }
    }
}