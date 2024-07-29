using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;
using ServeBooks.DTOs;
using System.Net;


namespace ServeLoans.Controllers.Loans
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
        public async Task<IActionResult> Update(int id, [FromBody]LoanDTO loan)
        {
            if (loan == null)
            {
                return BadRequest("The loan fields are invalid.");
            }
            
            try
            {
                var (result, message, statusCode) = await _repository.Update(id, loan);
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
        [Route("api/loans/{id}/delete")]
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the loan: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("api/loans/{id}/restore")]
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error restoring the loan: {ex.Message}");
            }
        }
    }
}