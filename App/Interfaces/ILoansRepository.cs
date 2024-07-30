using System.Net;
using ServeBooks.DTOs;
using ServeBooks.Models;

namespace ServeBooks.App.Interfaces
{
    public interface ILoansRepository
    {
        Task<(Loan loan, string message, HttpStatusCode statusCode)> Add(LoanDTO loan);
        Task<(Loan loan, string message, HttpStatusCode statusCode)> Update(int id, LoanDTO loan);
        Task<(IEnumerable<Loan> loans, string message, HttpStatusCode statusCode)> GetAll();
        Task<(Loan loan, string message, HttpStatusCode statusCode)> GetById(int id);
    }
}