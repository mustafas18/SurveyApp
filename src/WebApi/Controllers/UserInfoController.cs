using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserInfoService _userInfoService;

        public UserInfoController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = _userInfoService.GetUserInfoList();
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetById(int userId)
        {
            try
            {
                var result = _userInfoService.GetUserInfo(userId);
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            try
            {
                var result = _userInfoService.GetUserInfo(userName);
                UserFullNameViewModel userFullNameViewModel = new UserFullNameViewModel(result.Id, result.FullName);
                return StatusCode(200, CustomResult.Ok(userFullNameViewModel));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [Authorize(Roles = "Admin,SurveyDesigner")]
#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserInfo userInfo)
        {
            try
            {
                UserInfoDetails userInfoDetails = new UserInfoDetails
                {
                    AtmCard = "5859831037000",
                    Address = "Mashad",
                    Birthday = "1989",
                    City = "Mashad",
                    Country = "Iran",
                    Gender = GenderEnum.Male,
                    Grade = "2",
                    Job = "Student",
                    Mobile = "09350000"
                };
                userInfo.UpdateUserInfo(userInfoDetails);
                _userInfoService.AddUserInfo(userInfo);
                return StatusCode(200, CustomResult.Ok(userInfo));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
    }
}
