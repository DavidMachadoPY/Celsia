using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;
using ServeBooks.DTOs;
using System.Net;


namespace ServeBooks.Controllers.Loans
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    public class LoansUpdateController : ControllerBase
    {
        private readonly ILoansRepository _repository;
        public LoansUpdateController(ILoansRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/loans/{id}")]
        [Authorize(Roles = "Admin")]
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

        [HttpGet]
        [Route("api/loans/{id}/approve")]
        public async Task<IActionResult> ApproveLoan(int id)
        {
            try
            {
                var (result, message, statusCode) = await _repository.ApproveLoan(id);
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
                    Message = $"Error approving loan: {ex.Message}"
                });
            }
        }

        [HttpPut]
        [Route("api/loans/{id}/reject")]
        public async Task<IActionResult> RejectLoan(int id)
        {
            try
            {
                var (result, message, statusCode) = await _repository.RejectLoan(id);
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
                    Message = $"Error rejecting loan: {ex.Message}"
                });
            }
        }
    }
}