using AutoMapper;
using ServeBooks.DTOs;
using ServeBooks.Models;

namespace ServeBooks.App.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mappings for Invoice and related DTOs
            CreateMap<InvoiceDTO, Invoice>();
            CreateMap<Invoice, InvoiceDTO>().ReverseMap();
            
            CreateMap<InvoiceGetDTO, Invoice>(); 
            CreateMap<Invoice, InvoiceGetDTO>().ReverseMap();

            // Mappings for User and related DTOs
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>().ReverseMap();

            // Mappings for Transaction and related DTOs
            CreateMap<TransactionDTO, Transaction>();
            CreateMap<Transaction, TransactionDTO>().ReverseMap();

            CreateMap<TransactionGetDTO, Transaction>(); 
            CreateMap<Transaction, TransactionGetDTO>();

            CreateMap<TransactionCreateDTO, Transaction>(); 
            CreateMap<Transaction, TransactionCreateDTO>().ReverseMap();
        }
    }
}
