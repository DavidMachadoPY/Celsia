using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;
using ServeBooks.DTOs;
using System.Net;

namespace ServeBooks.Controllers.Transactions
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    public class TransactionsCreateController : ControllerBase
    {
        private readonly ITransactionsRepository _repository;
        public TransactionsCreateController(ITransactionsRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("api/transactions")]
        public async Task<IActionResult> Create([FromBody] TransactionCreateDTO transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new {
                    Message = "The transaction fields cannot be null.",
                    ModelState = ModelState
                });
            }
            
            try
            {
                var (result, message, statusCode) = await _repository.Add(transactionDto);
                if (statusCode != HttpStatusCode.Created)
                {
                    return StatusCode((int)statusCode, new {
                        Message = message
                    });
                }
                return CreatedAtAction(nameof(TransactionsController.GetById), "Transactions", new { id = result.Id }, new {
                    Message = message,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    Message = $"Error creating transaction: {ex.Message}"
                });
            }
        }
    }
}
