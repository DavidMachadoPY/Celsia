using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces.Auth;
using ServeBooks.Dtos;

namespace ServeBooks.Controllers.Auth
{

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _service;

        public AuthController(IAuthRepository AuthRepository)
        {
            _service = AuthRepository;
        }

        [AllowAnonymous]
        [HttpPost("api/users")]
        public async Task<ActionResult> RegisterUsers([FromBody] UserRegisterDTO model)
        {
            // Check that the name, email, and password are not empty
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Name, Email, and Password must not be empty");
            }

            try
            {
                // Register the user
                var (user, message, statusCode) = await _service.RegisterUser(model.Name, model.Email, model.Password);

                // If registration fails, return appropriate status code and message
                if (user == null)
                {
                    return StatusCode((int)statusCode, message);
                }

                return CreatedAtAction(nameof(RegisterUsers), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a message
                return StatusCode(500, new { Message = "Internal Server Error", StatusCode = 500, CurrentDate = DateTime.Now, Error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("api/admins")]
        public async Task<ActionResult> RegisterAdmin([FromBody] UserRegisterDTO model)
        {
            // Check that the name, email, and password are not empty
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Name, Email, and Password must not be empty");
            }

            try
            {
                // Register the user
                var (user, message, statusCode) = await _service.RegisterAdmin(model.Name, model.Email, model.Password);

                // If registration fails, return appropriate status code and message
                if (user == null)
                {
                    return StatusCode((int)statusCode, message);
                }

                return CreatedAtAction(nameof(RegisterAdmin), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a message
                return StatusCode(500, new { Message = "Internal Server Error", StatusCode = 500, CurrentDate = DateTime.Now, Error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("api/users/login")]
        public async Task<ActionResult> LoginRequest([FromBody] UserLoginDTO model)
        {
            // Check that the email and password are not empty
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Email and Password must not be empty");
            }

            try
            {
                // Authenticate the user
                var (login, authMessage, authStatusCode) = await _service.AuthenticateUser(model.Email, model.Password);

                // If authentication fails, return appropriate status code and message
                if (login == null)
                {
                    return StatusCode((int)authStatusCode, authMessage);
                }

                // Check if the user has the "Admin" role
                if (login.Role != "Admin")
                {
                    return Ok("Login successful, but no token provided as the user is not an Admin.");
                }

                // Generate an authentication token
                var (token, tokenMessage, role) = _service.GenerateAuthToken(login);

                // Return the token and roles
                return Ok(new { tokenBearer = token, Message = tokenMessage });
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a message
                return StatusCode(500, new { Message = "Internal Server Error", StatusCode = 500, CurrentDate = DateTime.Now, Error = ex.Message });
            }
        }
    }
}