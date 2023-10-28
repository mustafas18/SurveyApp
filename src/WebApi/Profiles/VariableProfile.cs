using AutoMapper;
using Core.Entities;
using WebApi.ViewModels;

namespace WebApi.Profiles
{
    public class VariableProfile : Profile
    {
        public VariableProfile()
        {
            CreateMap<VariableViewModel, Variable>();
            CreateMap<Variable, VariableViewModel>();
        }
    }
}
