using System.Net;
using ServeBooks.DTOs;
using ServeBooks.Models;

namespace ServeBooks.App.Interfaces
{
    public interface IBooksRepository
    { 
        Task<(Book book, string message, HttpStatusCode statusCode)> Add(BookDTO book);
        Task<(Book book, string message, HttpStatusCode statusCode)> Update(int id, BookDTO book);
        Task<(Book book, string message, HttpStatusCode statusCode)> Delete(int id);
        Task<(Book book, string message, HttpStatusCode statusCode)> Restore(int id);
        Task<(IEnumerable<Book> books, string message, HttpStatusCode statusCode)> GetAllDeleted();
        Task<(IEnumerable<Book> books, string message, HttpStatusCode statusCode)> GetAll();
        Task<(IEnumerable<Book> books, string message, HttpStatusCode statusCode)> GetAllAvailable();
        Task<(Book book, string message, HttpStatusCode statusCode)> GetById(int id);
        Task<(IEnumerable<BookStatusDTO> books, string message, HttpStatusCode statusCode)> Getavailable();

    }
}