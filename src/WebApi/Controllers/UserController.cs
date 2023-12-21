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
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(IUserService userService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            SignInManager<AppUser> signInManager)
        {
            _userService = userService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
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
        /// <summary>
        /// Handle the External Login Request. 
        /// This method redirects the user to the respective provider's login page.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ExternalLogin(string provider="Google")
        {
            try
            {
                var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "User");
                var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
                return Challenge(properties, provider);
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
        /// <summary>
        /// Handle the External Login Callback
        /// Implement a method to handle the callback from Google after a successful external login.
        /// The access token can be obtained using HttpContext.GetTokenAsync("access_token") method
        /// within the ExternalLoginCallback method.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            try
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return StatusCode(400, CustomResult.InternalError(new Exception ("Bad Request")));
                }

                // Obtain the access token
                var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

                // Access other user information if needed
                var user = info.Principal;

                // Process the success login and token as per your requirements
                return StatusCode(200, CustomResult.Ok(accessToken));
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
