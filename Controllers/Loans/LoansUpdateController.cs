using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;
using ServeBooks.DTOs;
using System.Net;


namespace ServeBooks.Controllers.Loans
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    [Authorize(Roles = "Admin")]
    public class LoansUpdateController : ControllerBase
    {
        private readonly ILoansRepository _repository;
        public LoansUpdateController(ILoansRepository repository)
        {
            _repository = repository;
        }

        [HttpPut]
        [Route("api/loans/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LoanDTO loanDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The Loans fields cannot be null or invalid.");
            }

            try
            {
                var (result, message, statusCode) = await _repository.Update(id, loanDto);
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
                    Message = $"Error updating loan: {ex.Message}"
                });
            }
        }
    }
}