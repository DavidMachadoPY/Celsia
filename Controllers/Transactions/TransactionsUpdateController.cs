using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;
using ServeBooks.DTOs;
using System.Net;

namespace ServeBooks.Controllers.Transactions
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    public class TransactionsUpdateController : ControllerBase
    {
        private readonly ITransactionsRepository _repository;
        public TransactionsUpdateController(ITransactionsRepository repository)
        {
            _repository = repository;
        }

        [HttpPut]
        [Route("api/transactions/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] TransactionDTO transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The transaction fields cannot be null or invalid.");
            }

            try
            {
                var (result, message, statusCode) = await _repository.Update(id, transactionDto);
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
                    Message = $"Error updating transaction: {ex.Message}"
                });
            }
        }

        [HttpPut]
        [Route("api/transactions/{id}/approve")]
        public async Task<IActionResult> ApproveTransaction(int id)
        {
            try
            {
                var (result, message, statusCode) = await _repository.ApproveTransaction(id);
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
                    Message = $"Error approving transaction: {ex.Message}"
                });
            }
        }

        [HttpPut]
        [Route("api/transactions/{id}/reject")]
        public async Task<IActionResult> RejectTransaction(int id)
        {
            try
            {
                var (result, message, statusCode) = await _repository.RejectTransaction(id);
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
                    Message = $"Error rejecting transaction: {ex.Message}"
                });
            }
        }
    }
}
