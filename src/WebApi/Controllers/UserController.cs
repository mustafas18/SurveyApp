﻿using Core.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.IRepositories;
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
        private readonly IUserInfoService _userInfoService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,
            IUserInfoService userInfoService,
            IMapper mapper)
        {
            _userService = userService;
            _userInfoService = userInfoService;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin,SurveyDesigner")]
        [HttpGet]
        public async Task<IActionResult> GetUserInfo(string userName)
        {
            try
            {
                return StatusCode(200, CustomResult.Ok(null));
            }
            catch (Exception ex)
            {
                return StatusCode(500,CustomResult.InternalError(ex));
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetUserInfoName(string userId)
        {
            try
            {
                var result= _userInfoService.GetUserInfo(userId);
                UserFullNameViewModel userFullNameViewModel= new UserFullNameViewModel(result.AppUserId,result.FullName);  
                return StatusCode(200, CustomResult.Ok(userFullNameViewModel));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetUserInfoNameList()
        {
            try
            {
                var result = _userInfoService.GetUserInfoList();
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
                return StatusCode(200, CustomResult.Ok(_userService.Register(_mapper.Map<UserRegisterDto>(register))));
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
                return StatusCode(200, CustomResult.Ok(null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }

    }



}
