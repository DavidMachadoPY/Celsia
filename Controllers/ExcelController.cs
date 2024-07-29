using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.App.Interfaces;

namespace ServeBooks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelRepository _Repository;

        public ExcelController(IExcelRepository Repository)
        {
            _Repository = Repository;
        }

        //Method to download Excel file with the data of customer
        [HttpGet("{CustomerId}")]
        public IActionResult Get(int CustomerId)
        {
            var fileBytes = _Repository.DowloadCustomerFile(CustomerId);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Estadisticas.xlsx");
        }

        //Method to download Excel file with the data of books and customers
        [HttpGet]
        public IActionResult GetStadistics()
        {
            var fileBytes = _Repository.DowloadStadisticsFile();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Estadisticas.xlsx");
        }
    }
}