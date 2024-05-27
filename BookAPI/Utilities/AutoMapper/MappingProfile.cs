using AutoMapper;
using EntityLayer.DTOs;
using EntityLayer.Models;

namespace BookAPI.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DTOBookUpdate, Book>();
            CreateMap<Book,DTOBook>();
        }
    }
}
