﻿using Domain.Dtos;
using Domain.Entities;

namespace WebApi.ViewModels
{
    public class UserFullNameViewModel
    {
        public UserFullNameViewModel(int id,string userName,string fullName)
        {
            Id = id;
            UserName = userName;
            FullName = fullName;
        }
        public UserFullNameViewModel(UserDto appUser)
        {
            Id = appUser.Id;
            UserName = appUser.UserName;
            FullName = appUser.FullName;
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
