using Core.Dtos;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> Register(UserRegisterDto userRegister);
        Task<LoginResultDto> Login(LoginInfoDto userDto);
        Task<bool> SignOut();
    }
}
