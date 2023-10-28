using AutoMapper;
using Core.Entities;
using WebApi.ViewModels;

namespace WebApi.Profiles
{
    public class SheetProfile:Profile
    {
        public SheetProfile()
        {
            CreateMap<SheetViewModel, Sheet>();
        }
       
    }
}
