﻿using AutoMapper;
using Core.Dtos;
using WebApi.ViewModels.Acconut;

namespace WebApi.Profiles
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterViewModel, UserRegisterDto>();
        }
    }
}
