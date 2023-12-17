using AutoMapper;
using Domain.Entities;
using WebApi.ViewModels;

namespace WebApi.Profiles
{
    public class VariableProfile : Profile
    {
        public VariableProfile()
        {
            CreateMap<VariableViewModel, Variable>();
            //    .ForMember(d => d.ValuesAsString, opt => opt.MapFrom(src => src.ValuesAsString));
            ;
            CreateMap<Variable, VariableViewModel>();
        }
    }
}
