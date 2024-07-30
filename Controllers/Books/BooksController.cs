using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;

namespace ServeBooks.Controllers.Books
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _repository;
        public BooksController(IBooksRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/books")]
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining books: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/books/deleted")]
        [Authorize(Roles = "Admin")]
         public async Task<IActionResult> GetAllDeleted()
        {
            try
            {
                var (result,message,statusCode) = await _repository.GetAllDeleted();
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining books: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/books/{id}")]
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining book with Id: {id}: {ex.Message}");
            }
        }
        
        [HttpGet]
        [Route("api/books/available")]
         public async Task<IActionResult> Getavailable()
        {
            try
            {
                var (result,message,statusCode) = await _repository.Getavailable();
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining books: {ex.Message}");
            }
        }
    }
}