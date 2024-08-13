using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;

namespace ServeBooks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelRepository _repository;

        public ExcelController(IExcelRepository repository)
        {
            _repository = repository;
        }

        // Method to download Excel file with the data of a specific user's transactions
        [HttpGet("{userId}")]
        public IActionResult GetUserTransactions(int userId)
        {
            var fileBytes = _repository.DownloadUserFile(userId);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"UserTransactions_{userId}.xlsx");
        }

        // Method to download Excel file with the data of all invoices and users
        [Authorize(Roles = "Admin")]
        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        {
            var fileBytes = _repository.DownloadStatisticsFile();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Statistics.xlsx");
        }
    }
}
