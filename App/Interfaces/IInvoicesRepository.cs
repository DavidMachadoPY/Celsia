using System.Net;
using ServeBooks.DTOs;
using ServeBooks.Models;

namespace ServeBooks.App.Interfaces
{
    public interface IInvoicesRepository
    { 
        Task<(Invoice invoice, string message, HttpStatusCode statusCode)> Add(InvoiceDTO invoice);
        Task<(Invoice invoice, string message, HttpStatusCode statusCode)> Update(int id, InvoiceDTO invoice);
        Task<(Invoice invoice, string message, HttpStatusCode statusCode)> Delete(int id);
        Task<(Invoice invoice, string message, HttpStatusCode statusCode)> Restore(int id);
        Task<(IEnumerable<Invoice> invoices, string message, HttpStatusCode statusCode)> GetAllDeleted();
        Task<(IEnumerable<Invoice> invoices, string message, HttpStatusCode statusCode)> GetAll();
        Task<(IEnumerable<InvoiceGetDTO> invoices, string message, HttpStatusCode statusCode)> GetAllAvailable();
        Task<(Invoice invoice, string message, HttpStatusCode statusCode)> GetById(int id);
    }
}
