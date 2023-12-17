using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using WebApi.ViewModels;

namespace WebApi.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<QuestionOrderViewModel, QuestionOrderDto>();
            //CreateMap<QuestionViewModel, Question>();

            CreateMap<AnswerViewModel, QuestionAnswer>().
                ForMember(d => d.Value, opt => opt.MapFrom(src => (int)src.Value));
            
        }
    }
}
