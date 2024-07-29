using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;
using ServeBooks.DTOs;
using ServeBooks.Models;
using System.Net;


namespace ServeBooks.Controllers.Books
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    [Authorize(Roles = "Admin")]
    public class BooksUpdateController : ControllerBase
    {
        private readonly IBooksRepository _repository;
        public BooksUpdateController(IBooksRepository repository)
        {
            _repository = repository;
        }

        [HttpPut]
        [Route("api/books/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]BookDTO book)
        {
            if (book == null)
            {
                return BadRequest("The book fields are invalid.");
            }
            
            try
            {
                var (result, message, statusCode) = await _repository.Update(id, book);
                if(statusCode == HttpStatusCode.OK)
                {
                    return Ok(new {
                        Message = message,
                        Result = result
                    });
                }
                else if (statusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(message);
                }
                return StatusCode((int)statusCode, message);
            
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the owner: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("api/books/{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var (result, message, statusCode) = await _repository.Delete(id);
                if(statusCode == HttpStatusCode.OK)
                {
                    return Ok(new {
                        Message = message,
                        Result = result
                    });
                }
                else if (statusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(message);
                }
                return StatusCode((int)statusCode, message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the book: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("api/books/{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {
                var (result, message, statusCode) = await _repository.Restore(id);
                if(statusCode == HttpStatusCode.OK)
                {
                    return Ok(new {
                        Message = message,
                        Result = result
                    });
                }
                else if (statusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(message);
                }
                return StatusCode((int)statusCode, message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error restoring the book: {ex.Message}");
            }
        }
    }
}