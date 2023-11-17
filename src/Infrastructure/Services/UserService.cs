using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.IRepositories;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenClaimsService _token;


        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenClaimsService token,
            IRepository<AppUser> userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
        public async Task<LoginResultDto> Login(LoginInfoDto userDto)
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


        public async Task<AppUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(_signInManager.Context.User);
        }
    }
}
