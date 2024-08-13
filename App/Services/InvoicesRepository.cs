using System.Net;
using AutoMapper;
using ServeBooks.DTOs;
using ServeBooks.Models;
using ServeBooks.App.Interfaces;
using ServeBooks.Data;
using Microsoft.EntityFrameworkCore;

namespace ServeBooks.App.Services
{
    public class InvoicesRepository : IInvoicesRepository
    {
        private readonly celsiaContext _context;
        private readonly IMapper _mapper;

        public InvoicesRepository(celsiaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(Invoice invoice, string message, HttpStatusCode statusCode)> Add(InvoiceDTO invoice)
        {
            var newInvoice = _mapper.Map<Invoice>(invoice);
            await _context.Invoices.AddAsync(newInvoice);
            await _context.SaveChangesAsync();
            return (newInvoice, "Invoice has been successfully created.", HttpStatusCode.Created);
        }

        public async Task<(Invoice invoice, string message, HttpStatusCode statusCode)> Update(int id, InvoiceDTO invoice)
        {
            var invoiceUpdate = await _context.Invoices.FindAsync(id);
            if (invoiceUpdate != null)
            {
                _mapper.Map(invoice, invoiceUpdate);
                _context.Entry(invoiceUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return (invoiceUpdate, "The invoice has been updated correctly.", HttpStatusCode.OK);
            }
            else
                return (default(Invoice)!, $"No invoice found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(IEnumerable<Invoice> invoices, string message, HttpStatusCode statusCode)> GetAll()
        {
            var invoices = await _context.Invoices.Include(i => i.Transactions).ToListAsync();
            if (invoices.Any())
                return (invoices, "Invoices have been successfully obtained.", HttpStatusCode.OK);
            else
                return (Enumerable.Empty<Invoice>(), "No invoices found in the database.", HttpStatusCode.NotFound);
        }

        public async Task<(IEnumerable<InvoiceGetDTO> invoices, string message, HttpStatusCode statusCode)> GetAllAvailable()
        {
            var invoices = await _context.Invoices.Include(i => i.Transactions).Where(i => i.Status!.ToLower() == "available").ToListAsync();
            if (invoices.Any())
            {
                var invoicesDTO = _mapper.Map<List<InvoiceGetDTO>>(invoices);
                return (invoicesDTO, "Invoices have been successfully obtained.", HttpStatusCode.OK);
            }
            else
            {
                return (Enumerable.Empty<InvoiceGetDTO>(), "No invoices found in the database.", HttpStatusCode.NotFound);
            }
        }

        public async Task<(IEnumerable<Invoice> invoices, string message, HttpStatusCode statusCode)> GetAllDeleted()
        {
            var invoices = await _context.Invoices.Include(i => i.Transactions).Where(i => i.Status!.ToLower() == "inactive").ToListAsync();
            if (invoices.Any())
                return (invoices, "Deleted invoices have been successfully obtained.", HttpStatusCode.OK);
            else
                return (Enumerable.Empty<Invoice>(), "No deleted invoices found in the database.", HttpStatusCode.NotFound);
        }

        public async Task<(Invoice invoice, string message, HttpStatusCode statusCode)> GetById(int id)
        {
            var invoice = await _context.Invoices.Include(i => i.Transactions).FirstOrDefaultAsync(i => i.Id.Equals(id));
            if (invoice != null)
                return (invoice, "Invoice has been successfully obtained.", HttpStatusCode.OK);
            else
                return (default(Invoice)!, $"No invoice found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(Invoice invoice, string message, HttpStatusCode statusCode)> Restore(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                if (invoice.Status == "available")
                {
                    return (invoice, $"The Invoice with Id: {id} is already available.", HttpStatusCode.NotFound);
                }
                else
                {
                    invoice.Status = "available";
                    _context.Entry(invoice).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return (invoice, "The invoice has been restored correctly.", HttpStatusCode.OK);
                }
            }
            else
                return (default(Invoice)!, $"No invoice found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(Invoice invoice, string message, HttpStatusCode statusCode)> Delete(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.InvoiceId == id && t.TransactionStatus == "Approved");

            if (invoice != null && transaction == null)
            {
                if (invoice.Status == "inactive")
                {
                    return (invoice, $"The Invoice with Id: {id} is already deleted.", HttpStatusCode.NotFound);
                }
                else
                {
                    invoice.Status = "inactive";
                    _context.Entry(invoice).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return (invoice, "The invoice has been deleted correctly.", HttpStatusCode.OK);
                }
            }
            else
                return (default(Invoice)!, $"No invoice found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }
    }
}
