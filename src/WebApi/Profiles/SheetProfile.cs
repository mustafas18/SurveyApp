using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using WebApi.ViewModels;

namespace WebApi.Profiles
{
    public class SheetProfile:Profile
    {
        public SheetProfile()
        {
            CreateMap<SheetViewModel, Sheet>();
            CreateMap<Sheet, SheetViewModel>();
            CreateMap<SheetDto, SheetViewModel>();
            CreateMap<SheetViewModel, SheetDto>();
        }
       
    }
}
