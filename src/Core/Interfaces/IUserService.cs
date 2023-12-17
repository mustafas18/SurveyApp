using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> Register(UserRegisterDto userRegister);
        Task<LoginResultDto> Login(LoginInfoDto userDto);
        Task<bool> SignOut();
        Task<AppUser> GetCurrentUserAsync();
        Task<List<UserDto>> GetUserInfoNameList();
    }
}
