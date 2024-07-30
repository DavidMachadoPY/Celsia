using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;
using ServeBooks.DTOs;
using System.Net;

namespace ServeBooks.Controllers.Loans
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    public class LoansCreateController : ControllerBase
    {
        private readonly ILoansRepository _repository;
        public LoansCreateController(ILoansRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("api/loans")]
        public async Task<IActionResult> Create([FromBody]LoanCreateDTO loanDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new{
                    Message = "The Loans fields cannot be null.",
                    ModelState = ModelState
                });
            }
            
            try
            {
                var (result, message, statusCode) = await _repository.Add(loanDto);
                if(statusCode != HttpStatusCode.Created)
                {
                    return StatusCode((int)statusCode, new {
                        Message = message
                    });
                }
                return CreatedAtAction(nameof(LoansController.GetById),"Loans", new { id = result.Id }, new {
                    Message = message,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    Message = $"Error creating loan: {ex.Message}"
                });
            }
        }
    }
}