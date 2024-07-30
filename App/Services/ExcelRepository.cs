using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServeBooks.App.Interfaces;
using ServeBooks.Data;
using ServeBooks.Models;

namespace ServeBooks.App.Services
{
    public class ExcelRepository : IExcelRepository
    {
        private readonly ServeBooksContext _context;

        public ExcelRepository(ServeBooksContext context)
        {
            _context = context;
        }

        //Implementation of IExcelRepository interface funtion to create a new Excel file with the data of customer
        public Byte[] DowloadCustomerFile(int CustomerId)
        {
            //Obtain the loans by customer id
            ICollection<Loan> registers = _context.Loans.Where(option => option.UserID == CustomerId).Include(u => u.User).Include(b => b.Book).ToList();

            //Create a new DataTable to store the data
            DataTable table = new DataTable();

            //Add columns to the DataTable
            table.Columns.AddRange(new DataColumn[] {
                new DataColumn("User Name"),
                new DataColumn("Book Title"),
                new DataColumn("Loan Date"),
                new DataColumn("Return Date")
            });

            //Fill the DataTable with the data
            foreach (var register in registers)
            {
                table.Rows.Add(register.User!.Name, register.Book!.Title, register.LoanDate, register.ReturnDate);
            }

            //Create a new Excel workbook and add the DataTable to it
            using (XLWorkbook workbook = new XLWorkbook())
            {
                //Create the sheet with the data
                workbook.Worksheets.Add(table, "Estadisticas");

                //Save the Excel file to a memory stream and return it as a byte array
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }

        //Implementation of IExcelRepository interface funtion to create a new Excel file with the data of books and customers
        public byte[] DowloadStadisticsFile()
        {
            //Obtain all books and customers
            ICollection<Book> books = _context.Books.ToList();
            ICollection<User> users = _context.Users.ToList();

            //Create new DataTables to store the data
            DataTable tableUsers = new DataTable();
            DataTable tableBooks = new DataTable();

            //Add Columns to the table users
            tableUsers.Columns.AddRange(new DataColumn[] {
            new DataColumn("User Name"),
            new DataColumn("User Email"),
            new DataColumn("User RegistrationDate"),
            new DataColumn("User Role")
        });

            //Fill the table user with the data
            foreach (var register in users)
            {
                tableUsers.Rows.Add(register.Name, register.Email, register.RegistrationDate,register.Role);
            }

            //Add Columns to the table books
            tableBooks.Columns.AddRange(new DataColumn[] {
            new DataColumn("Book Title"),
            new DataColumn("Book Author"),
            new DataColumn("Book Gender"),
            new DataColumn("Book NumberOfCopies"),
            new DataColumn("Book AvailableCopies"),
            new DataColumn("Book PublicationDate"),
            new DataColumn("Book Status")
        });
            

            //Fill the table books with the data
            foreach (var register in books)
            {
                tableBooks.Rows.Add(register.Title, register.Author, register.Gender, register.NumberOfCopies, register.AvailableCopies, register.PublicationDate, register.Status);
            }

            //Create a new Excel workbook and add the tables to it
            using (XLWorkbook workbook = new XLWorkbook())
            {
                //Create the sheet Users with the data of the users
                workbook.Worksheets.Add(tableUsers, "Users");
                //Create the sheet Books with the data of the books
                workbook.Worksheets.Add(tableBooks, "Books");

                //Save the Excel file to a memory stream and return it as a byte array
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