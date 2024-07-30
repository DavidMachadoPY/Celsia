using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;
using ServeBooks.DTOs;
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
        public async Task<IActionResult> Update(int id, [FromBody] BookDTO bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The book fields cannot be null or invalid.");
            }

            try
            {
                var (result, message, statusCode) = await _repository.Update(id, bookDto);
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
                    Message = $"Error updating book: {ex.Message}"
                });
            }
        }

        [HttpPut]
        [Route("api/books/{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The book fields cannot be null or invalid.");
            }

            try
            {
                var (result, message, statusCode) = await _repository.Delete(id);
                if (statusCode == HttpStatusCode.OK)
                {
                    return Ok(new
                    {
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
            if (!ModelState.IsValid)
            {
                return BadRequest("The book fields cannot be null or invalid.");
            }
            
            try
            {
                var (result, message, statusCode) = await _repository.Restore(id);
                if (statusCode == HttpStatusCode.OK)
                {
                    return Ok(new
                    {
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