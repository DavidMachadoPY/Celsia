using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;

namespace ServeBooks.Controllers.Transactions
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsRepository _repository;
        public TransactionsController(ITransactionsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/transactions")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var (result, message, statusCode) = await _repository.GetAll();
                if (result == null)
                {
                    return NotFound(message);
                }
                return Ok(new {
                    Message = message,
                    StatusCode = statusCode,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining transactions: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/transactions/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var (result, message, statusCode) = await _repository.GetById(id);
                if (result == null)
                {
                    return NotFound(message);
                }
                return Ok(new {
                    Message = message,
                    StatusCode = statusCode,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining transaction with Id: {id}: {ex.Message}");
            }
        }
    }
}
