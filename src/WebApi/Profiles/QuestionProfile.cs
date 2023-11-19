using AutoMapper;
using Core.Entities;
using WebApi.ViewModels;

namespace WebApi.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            //CreateMap<Question, QuestionViewModel>();
            //CreateMap<QuestionViewModel, Question>();

            CreateMap<AnswerViewModel, QuestionAnswer>().
                ForMember(d => d.Value, opt => opt.MapFrom(src => (int)src.Value));
            
        }
    }
}
