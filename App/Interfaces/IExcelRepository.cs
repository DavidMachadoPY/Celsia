using System;

namespace ServeBooks.App.Interfaces
{
    public interface IExcelRepository
    {
        // Interface to obtain all transactions of a user and return an Excel file
        Byte[] DownloadUserFile(int userId);

        // Interface to obtain all registers of invoices and users, and return an Excel file
        Byte[] DownloadStatisticsFile();
    }
}
