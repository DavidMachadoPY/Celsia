using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;
using ServeBooks.DTOs;
using System.Net;

namespace ServeBooks.Controllers.Invoices
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    public class InvoicesCreateController : ControllerBase
    {
        private readonly IInvoicesRepository _repository;
        public InvoicesCreateController(IInvoicesRepository repository)
        {
            _repository = repository;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/invoices")]
        public async Task<IActionResult> Create([FromBody] InvoiceDTO invoiceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The invoice fields cannot be null or invalid.");
            }
            
            try
            {
                var (result, message, statusCode) = await _repository.Add(invoiceDto);
                if(statusCode != HttpStatusCode.Created)
                {
                    return StatusCode((int)statusCode, new {
                        Message = message
                    });
                }
                return CreatedAtAction(nameof(InvoicesController.GetById), "Invoices", new { id = result.Id }, new {
                    Message = message,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    Message = $"Error creating invoice: {ex.Message}"
                });
            }
        }
    }
}
