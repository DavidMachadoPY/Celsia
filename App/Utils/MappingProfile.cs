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
            
            CreateMap<BookStatusDTO, Book>(); 
            CreateMap<Book, BookStatusDTO>().ReverseMap();

            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<LoanDTO, Loan>();
            CreateMap<Loan, LoanDTO>().ReverseMap();
        }
    }
}