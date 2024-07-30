using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;
using ServeBooks.DTOs;
using System.Net;

namespace ServeBooks.Controllers.Users
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    public class UsersUpdateController : ControllerBase
    {
        private readonly IUsersRepository _repository;
        public UsersUpdateController(IUsersRepository repository)
        {
            _repository = repository;
        }

        [HttpPut]
        [Route("api/users/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The Users fields cannot be null or invalid.");
            }

            try
            {
                var (result, message, statusCode) = await _repository.Update(id, userDto);
                if (statusCode == HttpStatusCode.OK)
                {
                    return Ok(new
                    {
                        Message = message,
                        Data = result
                    });
                }
                else if (statusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(new
                    {
                        Message = message
                    });
                }
                return StatusCode((int)statusCode, new
                {
                    Message = message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = $"Error updating user: {ex.Message}"
                });
            }
        }
    }
}