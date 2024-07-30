using Microsoft.EntityFrameworkCore;
using System.Net;
using AutoMapper;
using ServeBooks.DTOs;
using ServeBooks.Models;
using ServeBooks.App.Interfaces;
using ServeBooks.Data;

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
            var books = await _context.Books.Include(b => b.Loans).ToListAsync();
            if (books.Any())
                return (books, "Books have been successfully obtained.", HttpStatusCode.OK);
            else
                return (Enumerable.Empty<Book>(), "No books found in the database.", HttpStatusCode.NotFound);
        }

        public async Task<(IEnumerable<Book> books, string message, HttpStatusCode statusCode)> GetAllAvailable()
        {
            var books = await _context.Books.Include(b => b.Loans).Where(b => b.Status!.ToLower() == "available").ToListAsync();
            if (books.Any())
                return (books, "Books have been successfully obtained.", HttpStatusCode.OK);
            else
                return (Enumerable.Empty<Book>(), "No books found in the database.", HttpStatusCode.NotFound);
        }

        public async Task<(IEnumerable<Book> books, string message, HttpStatusCode statusCode)> GetAllDeleted()
        {
            var books = await _context.Books.Include(b => b.Loans).Where(b => b.Status!.ToLower() == "inactive").ToListAsync();
            if (books.Any())
                return (books, "Deleted books have been successfully obtained.", HttpStatusCode.OK);
            else
                return (Enumerable.Empty<Book>(), "No deleted books found in the database.", HttpStatusCode.NotFound);
        }

        public async Task<(Book book, string message, HttpStatusCode statusCode)> GetById(int id)
        {
            var book = await _context.Books.Include(b => b.Loans).FirstOrDefaultAsync(b => b.Id.Equals(id));
            if (book != null)
                return (book, "Book has been successfully obtained.", HttpStatusCode.OK);
            else
                return (default(Book)!, $"No book found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(Book book, string message, HttpStatusCode statusCode)> Restore(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                if (book.Status == "available")
                {
                    return (book, $"The Book with Id: {id} is already available.", HttpStatusCode.NotFound);
                }
                else
                {
                    book.Status = "available";
                    _context.Entry(book).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return (book, "The book has been restored correctly.", HttpStatusCode.OK);
                }
            }
            else
                return (default(Book)!, $"No book found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(Book book, string message, HttpStatusCode statusCode)> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            var loan = await _context.Loans.FirstOrDefaultAsync(l => l.BookId == id && l.Status == "Approved");

            if (book != null && loan == null)
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

      public async Task<(IEnumerable<BookStatusDTO> books, string message, HttpStatusCode statusCode)> Getavailable()
        {

            var books= await _context.Books.Include(l => l.Loans).Where(f => f.Status!.ToLower() ==   "available" ||  f.Status!.ToLower() ==   "checkedout").ToListAsync();
            var mapper = _mapper.Map<IEnumerable<BookStatusDTO>>(books);

            if (books.Any())
                return (mapper, "Books have been successfully obtained.", HttpStatusCode.OK);


            else
                return (Enumerable.Empty<BookStatusDTO>(), "No books found in the database.", HttpStatusCode.NotFound);
        }

    }
}