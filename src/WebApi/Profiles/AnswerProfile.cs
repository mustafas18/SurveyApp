using AutoMapper;
using Core.Dtos;
using Core.Entities;
using WebApi.ViewModels;

namespace WebApi.Profiles
{
    public class AnswerProfile : Profile
    {
        public AnswerProfile()
        {
            CreateMap<UserAnswer, UserAnswerDto>();
        }
    }
}
