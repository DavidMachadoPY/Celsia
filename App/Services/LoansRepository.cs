using Microsoft.EntityFrameworkCore;
using System.Net;
using AutoMapper;
using ServeBooks.DTOs;
using ServeBooks.Models;
using ServeBooks.App.Interfaces;
using ServeBooks.Data;

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

        public async Task<(Loan loan, string message, HttpStatusCode statusCode)> Add(LoanDTO loan)
        {
            var newLoan = _mapper.Map<Loan>(loan);
            await _context.Loans.AddAsync(newLoan);
            await _context.SaveChangesAsync();
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
            var loans = await _context.Loans.Include(l => l.Book).Include(l => l.User).Include(l => l.User).Where(l => l.Status!.ToLower() == "available").ToListAsync();
            if (loans.Any())
                return (loans, "Loans have been successfully obtained.", HttpStatusCode.OK);
            else
                return (Enumerable.Empty<Loan>(), "No loans found in the database.", HttpStatusCode.NotFound);
        }

        public async Task<(IEnumerable<Loan> loans, string message, HttpStatusCode statusCode)> GetAllDeleted()
        {
            var loans = await _context.Loans.Include(l => l.Book).Include(l => l.User).Where(l => l.Status!.ToLower() == "inactive").ToListAsync();
            if (loans.Any())
                return (loans, "Deleted loans have been successfully obtained.", HttpStatusCode.OK);
            else
                return (Enumerable.Empty<Loan>(), "No deleted loans found in the database.", HttpStatusCode.NotFound);
        }

        public async Task<(Loan loan, string message, HttpStatusCode statusCode)> GetById(int id)
        {
            var loan = await _context.Loans.Include(l => l.Book).Include(l => l.User).FirstOrDefaultAsync(l => l.Id.Equals(id));
            if (loan != null)
                return (loan, "Loan has been successfully obtained.", HttpStatusCode.OK);
            else
                return (default(Loan)!, $"No loan found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(Loan loan, string message, HttpStatusCode statusCode)> Delete(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                if (loan.Status == "available")
                {
                    return (loan, $"The Loan with Id: {id} is already available.", HttpStatusCode.NotFound);
                }
                else
                {
                    loan.Status = "available";
                    _context.Entry(loan).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return (loan, "The loan has been restored correctly.", HttpStatusCode.OK);
                }
            }
            else
                return (default(Loan)!, $"No loan found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(Loan loan, string message, HttpStatusCode statusCode)> Restore(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                if (loan.Status == "inactive")
                {
                    return (loan, $"The Loan with Id: {id} is already deleted.", HttpStatusCode.NotFound);
                }
                else
                {
                    loan.Status = "inactive";
                    _context.Entry(loan).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return (loan, "The loan has been deleted correctly.", HttpStatusCode.OK);
                }
            }
            else
                return (default(Loan)!, $"No loan found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }
    }
}