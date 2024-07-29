using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServeBooks.Models;

namespace ServeBooks.App.Interfaces
{
    public interface IExcelRepository
    {
        //Interface to obtain all register of customers and return excel file
        Byte[] DowloadCustomerFile(int CustomerId);

        //Interface to obtain all register of books and customers, and return excel file

        Byte[] DowloadStadisticsFile();
    }
}