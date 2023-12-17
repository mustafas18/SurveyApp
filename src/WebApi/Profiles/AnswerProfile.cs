using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
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
