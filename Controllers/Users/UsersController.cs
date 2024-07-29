using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;

namespace ServeBooks.Controllers.Users
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _repository;
        public UsersController(IUsersRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/users")]
         public async Task<IActionResult> GetAll()
        {
            try
            {
                var (result,message,statusCode) = await _repository.GetAll();
                if(result == null)
                {
                    return NotFound(message);
                }
                return Ok(new {
                    Message = message,
                    StatusCode = statusCode,
                    Data = result
                });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining users: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/users/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var (result, message, statusCode) = await _repository.GetById(id);
                if(result == null)
                {
                    return NotFound(message);
                }
                return Ok(new {
                    Message = message,
                    StatusCode = statusCode,
                    Data = result
                });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining user with Id: {id}: {ex.Message}");
            }
        }
    }
}