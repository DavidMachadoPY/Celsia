using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;
using System.Net;

namespace ServeBooks.Controllers.Invoices
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoicesRepository _repository;

        public InvoicesController(IInvoicesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/invoices")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var (result, message, statusCode) = await _repository.GetAll();
                if (result == null)
                {
                    return NotFound(message);
                }
                return Ok(new
                {
                    Message = message,
                    StatusCode = $"200 {statusCode}",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining invoices: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/invoices/available")]
        public async Task<IActionResult> GetAllAvailable()
        {
            try
            {
                var (result, message, statusCode) = await _repository.GetAllAvailable();
                if (result == null)
                {
                    return NotFound(message);
                }
                return Ok(new
                {
                    Message = message,
                    StatusCode = statusCode,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining invoices: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/invoices/deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDeleted()
        {
            try
            {
                var (result, message, statusCode) = await _repository.GetAllDeleted();
                if (result == null)
                {
                    return NotFound(message);
                }
                return Ok(new
                {
                    Message = message,
                    StatusCode = statusCode,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining invoices: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/invoices/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var (invoice, message, statusCode) = await _repository.GetById(id);

                if (statusCode == HttpStatusCode.OK)
                {
                    return Ok(new { Message = message, Data = invoice });
                }
                else if (statusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(new { Message = message });
                }
                return StatusCode((int)statusCode, new { Message = message });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error obtaining invoice with Id: {id}: {ex.Message}");
            }
        }
    }
}
