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
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly celsiaContext _context;
        private readonly IMapper _mapper;

        public TransactionsRepository(celsiaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(TransactionGetDTO transaction, string message, HttpStatusCode statusCode)> Add(TransactionCreateDTO transaction)
        {
            var newTransaction = _mapper.Map<Transaction>(transaction);
            newTransaction.TransactionStatus = "Pending";
            await _context.Transactions.AddAsync(newTransaction);
            await _context.SaveChangesAsync();
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.Id == newTransaction.InvoiceId);
            if (invoice != null)
            {
                invoice.Status = "Reserved";
                _context.Entry(invoice).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == newTransaction.UserID);
            string textSubject = $"Hello from ServeBooks {user!.Name}, your transaction is pending approval";
            string textBody = $"Hello {user!.Name}!\n\n" +
                              $"We have received your request for the transaction, and it will be verified by a manager.\n\n"+
                              $"Approve: http://localhost:5119/api/transactions/{newTransaction.Id}/approve \n\n"+
                              $"Reject: http://localhost:5119/api/transactions/{newTransaction.Id}/reject";

            var sendEmail = new MailersendUtils();
            await sendEmail.EnviarCorreo(/*user.Email!*/"avidmachado@gmail.com", textSubject, textBody);
            var transactionResponse = _mapper.Map<TransactionGetDTO>(newTransaction);
            return (transactionResponse, "Transaction has been successfully created.", HttpStatusCode.Created);
        }

        public async Task<(Transaction transaction, string message, HttpStatusCode statusCode)> Update(int id, TransactionDTO transaction)
        {
            var transactionUpdate = await _context.Transactions.FindAsync(id);
            if (transactionUpdate != null)
            {
                _mapper.Map(transaction, transactionUpdate);
                _context.Entry(transactionUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return (transactionUpdate, "The transaction has been updated correctly.", HttpStatusCode.OK);
            }
            else
                return (default(Transaction)!, $"No transaction found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(IEnumerable<TransactionGetDTO> transactions, string message, HttpStatusCode statusCode)> GetAll()
        {
            var transactions = await _context.Transactions
                .Include(t => t.PaymentMethod)
                .ToListAsync();

            if (transactions.Any())
            {
                var transactionResponses = _mapper.Map<IEnumerable<TransactionGetDTO>>(transactions);
                return (transactionResponses, "Transactions have been successfully obtained.", HttpStatusCode.OK);
            }
            else
                return (Enumerable.Empty<TransactionGetDTO>(), "No transactions found in the database.", HttpStatusCode.NotFound);
        }
        public async Task<(TransactionGetDTO transaction, string message, HttpStatusCode statusCode)> GetById(int id)
        {
            var transaction = await _context.Transactions.Include(t => t.Invoice).Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id.Equals(id));

            if (transaction != null)
            {
                var transactionResponse = _mapper.Map<TransactionGetDTO>(transaction);
                return (transactionResponse, "Transaction has been successfully obtained.", HttpStatusCode.OK);
            }
            else
                return (null!, $"No transaction found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(TransactionGetDTO transaction, string message, HttpStatusCode statusCode)> ApproveTransaction(int id)
        {
            var transaction = await _context.Transactions.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
            if (transaction != null)
            {
                if (transaction.TransactionStatus == "Approved")
                {
                    var transactionResponse = _mapper.Map<TransactionGetDTO>(transaction);
                    return (transactionResponse, $"The Transaction with Id: {id} is already approved.", HttpStatusCode.OK);
                }
                else
                {
                    transaction.TransactionStatus = "Approved";
                    _context.Entry(transaction).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.Id == transaction.InvoiceId);
                    if (invoice != null)
                    {
                        invoice.Status = "Bad";
                        _context.Entry(invoice).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    var transactionResponse = _mapper.Map<TransactionGetDTO>(transaction);
                    return (transactionResponse, "The transaction has been approved correctly.", HttpStatusCode.OK);
                }
            }
            else
                return (null!, $"No transaction found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

        public async Task<(TransactionGetDTO transaction, string message, HttpStatusCode statusCode)> RejectTransaction(int id)
        {
            var transaction = await _context.Transactions.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
            if (transaction != null)
            {
                if (transaction.TransactionStatus == "Rejected")
                {
                    var transactionResponse = _mapper.Map<TransactionGetDTO>(transaction);
                    return (transactionResponse, $"The Transaction with Id: {id} is already rejected.", HttpStatusCode.OK);
                }
                else
                {
                    transaction.TransactionStatus = "Rejected";
                    _context.Entry(transaction).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.Id == transaction.InvoiceId);
                    if (invoice != null)
                    {
                        invoice.Status = "Available";
                        _context.Entry(invoice).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    var transactionResponse = _mapper.Map<TransactionGetDTO>(transaction);
                    return (transactionResponse, "The transaction has been rejected correctly.", HttpStatusCode.OK);
                }
            }
            else
                return (null!, $"No transaction found in the database with Id: {id}.", HttpStatusCode.NotFound);
        }

    }
}
