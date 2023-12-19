using Domain.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Security.Claims;
using WebApi.ViewModels;
using WebApi.ViewModels.Acconut;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserService userService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetUserNameList()
        {
            try
            {
                var result = await _userService.GetUserInfoNameList();
                List<UserFullNameViewModel> userFullNameList= new List<UserFullNameViewModel>();
                result.ForEach(u => { userFullNameList.Add(new UserFullNameViewModel(u)); });
                return StatusCode(200, CustomResult.Ok(userFullNameList));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel register)
        {
            try
            {
                return StatusCode(200, CustomResult.Ok(await _userService.Register(_mapper.Map<UserRegisterDto>(register))));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginInfoDto userInfo)
        {
            try
            {
                return StatusCode(200, CustomResult.Ok(await _userService.Login(userInfo)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            try
            {
                return StatusCode(200, CustomResult.Ok(_userService.SignOut()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault().Subject.Name;
                return StatusCode(200, CustomResult.Ok(userName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }

    }



}
