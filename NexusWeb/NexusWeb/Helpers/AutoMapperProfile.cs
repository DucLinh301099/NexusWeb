using AutoMapper;
using NexusWeb.Models;
using NexusWeb.ViewModels;

namespace NexusWeb.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterVM, User>();
            //.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM => RegisterVM.HoTen))
            //.ReverseMap();
        }
    }
}