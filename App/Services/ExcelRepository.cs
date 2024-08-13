using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using ServeBooks.App.Interfaces;
using ServeBooks.Data;
using ServeBooks.Models;

namespace ServeBooks.App.Services
{
    public class ExcelRepository : IExcelRepository
    {
        private readonly celsiaContext _context;

        // Corrected constructor to remove the extra parenthesis
        public ExcelRepository(celsiaContext context)
        {
            _context = context;
        }

        // Implementation of IExcelRepository interface function to create a new Excel file with the data of a user's transactions
        public Byte[] DownloadUserFile(int userId)
        {
            // Obtain the transactions by user ID
            ICollection<Transaction> registers = _context.Transactions
                .Where(t => t.Invoice!.User!.Id == userId)
                .Include(t => t.Invoice)
                .Include(t => t.Invoice!.User)  // Replaced Customer with User
                .Include(t => t.PaymentMethod)
                .ToList();

            // Create a new DataTable to store the data
            DataTable table = new DataTable();

            // Add columns to the DataTable
            table.Columns.AddRange(new DataColumn[] {
                new DataColumn("User Name"),
                new DataColumn("Invoice Date"),
                new DataColumn("Invoice Total Amount"),
                new DataColumn("Invoice Status"),
                new DataColumn("Transaction Date"),
                new DataColumn("Transaction Amount"),
                new DataColumn("Transaction Status"),
                new DataColumn("Payment Method")
            });

            // Fill the DataTable with the data
            foreach (var register in registers)
            {
                table.Rows.Add(
                    register.Invoice!.User!.Name,  // Replaced Customer with User
                    register.Invoice.InvoiceDate, 
                    register.Invoice.TotalAmount, 
                    register.Invoice.Status,
                    register.TransactionDate,
                    register.Amount,
                    register.TransactionStatus,
                    register.PaymentMethod!.MethodName
                );
            }

            // Create a new Excel workbook and add the DataTable to it
            using (XLWorkbook workbook = new XLWorkbook())
            {
                // Create the sheet with the data
                workbook.Worksheets.Add(table, "UserTransactions");

                // Save the Excel file to a memory stream and return it as a byte array
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }

        // Implementation of IExcelRepository interface function to create a new Excel file with the data of invoices and users
        public byte[] DownloadStatisticsFile()
        {
            // Obtain all invoices and users
            ICollection<Invoice> invoices = _context.Invoices.Include(i => i.User).ToList(); // Replaced Customer with User
            ICollection<User> users = _context.Users.ToList();

            // Create new DataTables to store the data
            DataTable tableUsers = new DataTable();
            DataTable tableInvoices = new DataTable();

            // Add Columns to the table users
            tableUsers.Columns.AddRange(new DataColumn[] {
                new DataColumn("User Name"),
                new DataColumn("User Email"),
                new DataColumn("User Phone"),
                new DataColumn("User Created At")
            });

            // Fill the table users with the data
            foreach (var register in users)
            {
                tableUsers.Rows.Add(
                    register.Name, 
                    register.Email, 
                    register.Phone, 
                    register.RegistrationDate  // Replaced CreatedAt with RegistrationDate
                );
            }

            // Add Columns to the table invoices
            tableInvoices.Columns.AddRange(new DataColumn[] {
                new DataColumn("Invoice ID"),
                new DataColumn("User Name"),  // Replaced Customer with User
                new DataColumn("Invoice Date"),
                new DataColumn("Total Amount"),
                new DataColumn("Status"),
                new DataColumn("Created At")
            });

            // Fill the table invoices with the data
            foreach (var register in invoices)
            {
                tableInvoices.Rows.Add(
                    register.Id, 
                    register.User!.Name,  // Replaced Customer with User
                    register.InvoiceDate, 
                    register.TotalAmount, 
                    register.Status
                );
            }

            // Create a new Excel workbook and add the tables to it
            using (XLWorkbook workbook = new XLWorkbook())
            {
                // Create the sheet Users with the data of the users
                workbook.Worksheets.Add(tableUsers, "Users");
                // Create the sheet Invoices with the data of the invoices
                workbook.Worksheets.Add(tableInvoices, "Invoices");

                // Save the Excel file to a memory stream and return it as a byte array
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }
    }
}
