using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;

namespace ServeBooks.Controllers.Loans
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    public class LoansController : ControllerBase
    {
        private readonly ILoansRepository _repository;
        public LoansController(ILoansRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/loans")]
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining loans: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/loans/{id}")]
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining loan with Id: {id}: {ex.Message}");
            }
        }
    }
}