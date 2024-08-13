using System.Net;
using ServeBooks.DTOs;
using ServeBooks.Models;

namespace ServeBooks.App.Interfaces
{
    public interface ITransactionsRepository
    {
        Task<(TransactionGetDTO transaction, string message, HttpStatusCode statusCode)> Add(TransactionCreateDTO transaction);
        Task<(Transaction transaction, string message, HttpStatusCode statusCode)> Update(int id, TransactionDTO transaction);
        Task<(TransactionGetDTO transaction, string message, HttpStatusCode statusCode)> ApproveTransaction(int id);
        Task<(TransactionGetDTO transaction, string message, HttpStatusCode statusCode)> RejectTransaction(int id);
        Task<(IEnumerable<TransactionGetDTO> transactions, string message, HttpStatusCode statusCode)> GetAll();
        Task<(TransactionGetDTO transaction, string message, HttpStatusCode statusCode)> GetById(int id);
    }
}
