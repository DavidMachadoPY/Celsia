using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;
using ServeBooks.DTOs;
using System.Net;

namespace ServeBooks.Controllers.Invoices
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    [Authorize(Roles = "Admin")]
    public class InvoicesUpdateController : ControllerBase
    {
        private readonly IInvoicesRepository _repository;
        public InvoicesUpdateController(IInvoicesRepository repository)
        {
            _repository = repository;
        }

        [HttpPut]
        [Route("api/invoices/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] InvoiceDTO invoiceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The invoice fields cannot be null or invalid.");
            }

            try
            {
                var (result, message, statusCode) = await _repository.Update(id, invoiceDto);
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
                    Message = $"Error updating invoice: {ex.Message}"
                });
            }
        }

        [HttpPut]
        [Route("api/invoices/{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The invoice fields cannot be null or invalid.");
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the invoice: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("api/invoices/{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The invoice fields cannot be null or invalid.");
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error restoring the invoice: {ex.Message}");
            }
        }
    }
}
