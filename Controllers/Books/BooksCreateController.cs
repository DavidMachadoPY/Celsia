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
    public class BooksCreateController : ControllerBase
    {
        private readonly IBooksRepository _repository;
        public BooksCreateController(IBooksRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("api/books")]
        public async Task<IActionResult> Create([FromBody] BookDTO bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The Books fields cannot be null or invalid.");
            }
            
            try
            {
                var (result, message, statusCode) = await _repository.Add(bookDto);
                if(statusCode != HttpStatusCode.Created)
                {
                    return StatusCode((int)statusCode, new {
                        Message = message
                    });
                }
                return CreatedAtAction(nameof(BooksController.GetById),"Books", new { id = result.Id }, new {
                    Message = message,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    Message = $"Error creating book: {ex.Message}"
                });
            }
        }
    }
}