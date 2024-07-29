using System.Net;
using ServeBooks.DTOs;
using ServeBooks.Models;

namespace ServeLoans.App.Interfaces
{
    public interface ILoansRepository
    {
        Task<(Loan loan, string message, HttpStatusCode statusCode)> Add(LoanDTO loan);
        Task<(Loan loan, string message, HttpStatusCode statusCode)> Update(int id, LoanDTO loan);
        Task<(Loan loan, string message, HttpStatusCode statusCode)> Delete(int id);
        Task<(Loan loan, string message, HttpStatusCode statusCode)> Restore(int id);
        Task<(IEnumerable<Loan> loans, string message, HttpStatusCode statusCode)> GetAllDeleted();
        Task<(IEnumerable<Loan> loans, string message, HttpStatusCode statusCode)> GetAll();
        Task<(Loan loan, string message, HttpStatusCode statusCode)> GetById(int id);
    }
}