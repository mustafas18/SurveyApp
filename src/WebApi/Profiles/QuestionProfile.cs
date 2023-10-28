using AutoMapper;
using Core.Entities;
using WebApi.ViewModels;

namespace WebApi.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionViewModel>();
            CreateMap<QuestionViewModel, Question>();
        }
    }
}
