using System.Net;
using ServeBooks.DTOs;
using ServeBooks.Models;

namespace ServeBooks.App.Interfaces
{
    public interface ILoansRepository
    {
        Task<(Loan loan, string message, HttpStatusCode statusCode)> Add(LoanCreateDTO loan);
        Task<(Loan loan, string message, HttpStatusCode statusCode)> Update(int id, LoanDTO loan);
        Task<(Loan loan, string message, HttpStatusCode statusCode)> ApproveLoan(int id);
        Task<(Loan loan, string message, HttpStatusCode statusCode)> RejectLoan(int id);
        Task<(IEnumerable<Loan> loans, string message, HttpStatusCode statusCode)> GetAll();
        Task<(Loan loan, string message, HttpStatusCode statusCode)> GetById(int id);
    }
}