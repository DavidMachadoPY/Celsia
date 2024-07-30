using AutoMapper;
using ServeBooks.DTOs;
using ServeBooks.Models;

namespace ServeBooks.App.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookDTO, Book>();
            CreateMap<Book, BookDTO>().ReverseMap();
            
            CreateMap<BookGetDTO, Book>(); 
            CreateMap<Book, BookGetDTO>().ReverseMap();

            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<LoanDTO, Loan>();
            CreateMap<Loan, LoanDTO>().ReverseMap();

            CreateMap<LoanGetDTO, Loan>(); 
            CreateMap<Loan, LoanGetDTO>().ReverseMap();

            CreateMap<LoanCreateDTO, Loan>(); 
            CreateMap<Loan, LoanCreateDTO>().ReverseMap();
        }
    }
}