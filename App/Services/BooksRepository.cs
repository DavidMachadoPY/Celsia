using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServeBooks.App.Interfaces;
using ServeBooks.Data;
using ServeBooks.DTOs;
using ServeBooks.Models;

namespace ServeBooks.App.Services
{
    public class BooksRepository : IBooksRepository
    {
        
        private readonly ServeBooksContext _context;
        private readonly IMapper _mapper;
        public BooksRepository(ServeBooksContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(Book book, string message, HttpStatusCode statusCode)> Add(BookDTO book)
        {
            var newBook = _mapper.Map<Book>(book);
            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();
            return (newBook, "Book has been successfully created.", HttpStatusCode.Created);
        }

        public async Task<(Book book, string message, HttpStatusCode statusCode)> Update(int id, BookDTO book)
        {
            var bookUpdate = await _context.Books.FindAsync(id);
            if (bookUpdate!= null)
            {
                _mapper.Map(book, bookUpdate);
                _context.Entry(bookUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return (bookUpdate, "The book has been updated correctly.", HttpStatusCode.OK);
            }
            else
                return (default(Book)!, $"No book found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(IEnumerable<Book> books, string message, HttpStatusCode statusCode)> GetAll()
        {
            var books = await _context.Books.Include(b => b.Loans).Where(f => f.Status!.ToLower() == "active").ToListAsync();
            if (books.Any())
                return (books, "Books have been successfully obtained.", HttpStatusCode.OK);
            else
                return (Enumerable.Empty<Book>(), "No books found in the database.", HttpStatusCode.NotFound);
        }

        public async Task<(IEnumerable<Book> books, string message, HttpStatusCode statusCode)> GetAllDeleted()
        {
            var books = await _context.Books.Include(b => b.Loans).Where(f => f.Status!.ToLower() == "inactive").ToListAsync();
            if (books.Any())
                return (books, "Deleted books have been successfully obtained.", HttpStatusCode.OK);
            else
                return (Enumerable.Empty<Book>(), "No deleted books found in the database.", HttpStatusCode.NotFound);
        }

        public async Task<(Book book, string message, HttpStatusCode statusCode)> GetById(int id)
        {
            var book = await _context.Books.Include(b=> b.Loans).FirstOrDefaultAsync(f => f.Id.Equals(id));
            if (book != null)
                return (book, "Book has been successfully obtained.", HttpStatusCode.OK);
            else
                return (default(Book)!, $"No book found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(Book book, string message, HttpStatusCode statusCode)> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                if (book.Status == "active")
                {
                    return (book, $"The Book with Id: {id} is already active.", HttpStatusCode.NotFound);
                }
                else
                {
                    book.Status = "active";
                    _context.Entry(book).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return (book, "The book has been restored correctly.", HttpStatusCode.OK);
                }
            }
            else
                return (default(Book)!, $"No book found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(Book book, string message, HttpStatusCode statusCode)> Restore(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                if (book.Status == "inactive")
                {
                    return (book, $"The Book with Id: {id} is already deleted.", HttpStatusCode.NotFound);
                }
                else
                {
                    book.Status = "inactive";
                    _context.Entry(book).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return (book, "The book has been deleted correctly.", HttpStatusCode.OK);
                }
            }
            else
                return (default(Book)!, $"No book found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

         //Usuarios pueden consultar disponibilidad de libros y fechas de vencimiento de préstamos. Proveer información precisa y actualizada.

      public async Task<(IEnumerable<Book> books, string message, HttpStatusCode statusCode)> Getavailable()
        {

            var books = await _context.Books.Include(l => l.Loans).Where(f => f.Status!.ToLower() == "available").ToListAsync();
            if (books.Any())
                return (books, "Books have been successfully obtained.", HttpStatusCode.OK);


            else
                return (Enumerable.Empty<Book>(), "No books found in the database.", HttpStatusCode.NotFound);
        }    

    }
}