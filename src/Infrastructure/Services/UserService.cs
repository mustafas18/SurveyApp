using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.IRepositories;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserRepository _userRpository;
        private readonly ITokenClaimsService _token;

        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            UserRepository userRpository,
            ITokenClaimsService token)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRpository = userRpository;
            _token = token;
        }
        public async Task<AppUser> Register(UserRegisterDto userRegister)
        {
                AppUser user = new AppUser();
                user.UserName = userRegister.UserName;
                await _userManager.CreateAsync(user, userRegister.Password);
                await _userManager.AddToRoleAsync(user, "client");
                return user;
        }
        async Task<LoginResultDto> Login(LoginInfoDto userDto)
        {
            var accessToken = await _token.GetTokenAsync(userDto);
            var userRoles = await _token.GetUserRolesAsync(userDto.UserName);

            return new LoginResultDto
            {
                UserName = userDto?.UserName,
                UserRoles = userRoles,
                AccessToken = accessToken,
                IssuedDate = DateTime.UtcNow,
            };
        }
        public async Task<bool> SignOut()
        {
            _signInManager.SignOutAsync();
            return true;
        }
    }
}
