using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<AppUser> _userRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;

        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenClaimsService token,
            IRepository<AppUser> userRepository,
            IRepository<UserInfo> userInfoRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _token = token;
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
        }
        public async Task<AppUser> Register(UserRegisterDto userRegister)
        {
            AppUser user = new AppUser();
            user.UserName = userRegister.UserName;
            await _userInfoRepository.AddAsync(new UserInfo(userRegister.UserName, userRegister.FirstName, userRegister.LastName));
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

        public async Task<List<UserDto>> GetUserInfoNameList()
        {
            var users = await _userInfoRepository.AsNoTracking().ToListAsync();
            List<UserDto> result = new List<UserDto>();
            users.ForEach(u =>
            {
                result.Add(new UserDto
                {
                    FullName = u.FullName,
                    UserName = u.UserName,
                    Id=u.Id
                });
            });
            return result;
        }

        
    }
}
