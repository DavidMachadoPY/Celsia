using Microsoft.EntityFrameworkCore;
using System.Net;
using AutoMapper;
using ServeBooks.DTOs;
using ServeBooks.Models;
using ServeBooks.App.Interfaces;
using ServeBooks.Data;
using ServeBooks.App.Utils.Email;

namespace ServeBooks.App.Services
{
    public class LoansRepository : ILoansRepository
    {
        
        private readonly ServeBooksContext _context;
        private readonly IMapper _mapper;
        public LoansRepository(ServeBooksContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(Loan loan, string message, HttpStatusCode statusCode)> Add(LoanCreateDTO loan)
        {
            var newLoan = _mapper.Map<Loan>(loan);
            newLoan.Status = "Pending";
            await _context.Loans.AddAsync(newLoan);
            await _context.SaveChangesAsync();
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == newLoan.BookId);
            if (book != null)
            {
                book.Status = "Reserved";
                _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == newLoan.UserID);
            string textSubject = $"Hellow from ServeBooks {user!.Name}, your loan its pending to be approved";
            string textBody = $"Hello {user!.Name}!\n\n" +
                              $"we have received your request to borrow the book, the request will be verified by a manager.";

            var sendEmail = new MailersendUtils();
            await sendEmail.EnviarCorreo(/*user.Email!*/"avidmachado@gmail.com", textSubject, textBody);

            return (newLoan, "Loan has been successfully created.", HttpStatusCode.Created);
        }

        public async Task<(Loan loan, string message, HttpStatusCode statusCode)> Update(int id, LoanDTO loan)
        {
            var loanUpdate = await _context.Loans.FindAsync(id);
            if (loanUpdate!= null)
            {
                _mapper.Map(loan, loanUpdate);
                _context.Entry(loanUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return (loanUpdate, "The loan has been updated correctly.", HttpStatusCode.OK);
            }
            else
                return (default(Loan)!, $"No loan found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(IEnumerable<Loan> loans, string message, HttpStatusCode statusCode)> GetAll()
        {
            var loans = await _context.Loans.Include(l => l.Book).Include(l => l.User)
            .Where(l => l.UserID == l.User!.Id).ToListAsync();
            if (loans.Any())
                return (loans, "Loans have been successfully obtained.", HttpStatusCode.OK);
            else
                return (Enumerable.Empty<Loan>(), "No loans found in the database.", HttpStatusCode.NotFound);
        }

        public async Task<(Loan loan, string message, HttpStatusCode statusCode)> GetById(int id)
        {
            var loan = await _context.Loans.Include(l => l.Book).Include(l => l.User).Where(l => l.UserID == l.User!.Id).FirstOrDefaultAsync(l => l.Id.Equals(id));
            if (loan != null)
                return (loan, "Loan has been successfully obtained.", HttpStatusCode.OK);
            else
                return (default(Loan)!, $"No loan found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(Loan loan, string message, HttpStatusCode statusCode)> ApproveLoan(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                if (loan.Status == "Approved")
                {
                    return (loan, $"The Loan with Id: {id} is already approved.", HttpStatusCode.NotFound);
                }
                else
                {
                    loan.Status = "Approved";
                    _context.Entry(loan).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return (loan, "The loan has been approved correctly.", HttpStatusCode.OK);
                }
            }
            else
                return (default(Loan)!, $"No loan found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(Loan loan, string message, HttpStatusCode statusCode)> RejectLoan(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                if (loan.Status == "Rejected")
                {
                    return (loan, $"The Loan with Id: {id} is already rejected.", HttpStatusCode.NotFound);
                }
                else
                {
                    loan.Status = "Rejected";
                    _context.Entry(loan).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return (loan, "The loan has been rejected correctly.", HttpStatusCode.OK);
                }
            }
            else
                return (default(Loan)!, $"No loan found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }
    }
}