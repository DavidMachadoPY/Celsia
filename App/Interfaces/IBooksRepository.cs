using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ServeBooks.DTOs;
using ServeBooks.Models;

namespace ServeBooks.App.Interfaces
{
    public interface IBooksRepository
    { Task<(Book book, string message, HttpStatusCode statusCode)> Add(BookDTO book);
        Task<(Book book, string message, HttpStatusCode statusCode)> Update(int id, BookDTO book);
        Task<(Book book, string message, HttpStatusCode statusCode)> Delete(int id);
        Task<(Book book, string message, HttpStatusCode statusCode)> Restore(int id);
        Task<(IEnumerable<Book> books, string message, HttpStatusCode statusCode)> GetAllDeleted();
        Task<(IEnumerable<Book> books, string message, HttpStatusCode statusCode)> GetAll();
        Task<(Book book, string message, HttpStatusCode statusCode)> GetById(int id);
    }
}