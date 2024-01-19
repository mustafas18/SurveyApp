using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;
using WebApi.ViewModels.Acconut;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserInfoService _userInfoService;
        private readonly IRepository<UserCategory> _categoryRepository;
        private readonly IRepository<UserDegree> _degreeRepository;
        private readonly IRepository<UserDegreeMajor> _majorRepository;

        public UserInfoController(IUserInfoService userInfoService,
            IRepository<UserCategory> categoryRepository,
            IRepository<UserDegree> degreeRepository,
             IRepository<UserDegreeMajor> majorRepository)
        {
            _userInfoService = userInfoService;
            _categoryRepository = categoryRepository;
            _degreeRepository = degreeRepository;
            _majorRepository = majorRepository;
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
        [HttpGet]
        public async Task<IActionResult> GetDegree()
        {
            try
            {
                var result = _degreeRepository.AsNoTracking().ToList();
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetMajors(string userName)
        {
            try
            {
                var majors = _majorRepository.AsNoTracking()
                    .Where(s => s.UserName == userName)
                    .ToList();
                return StatusCode(200, CustomResult.Ok(majors));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddDegree([FromBody] UserDegreeViewModel degreeMajor)
        {
            try
            {
                var degree = _degreeRepository.AsNoTracking().FirstOrDefault(s => s.Id == degreeMajor.DegreeId);
                UserDegreeMajor userDegreeMajor = new UserDegreeMajor
                {
                    Degree = degree,
                    MajorTitle = degreeMajor.MajorTitle,
                    UserName = degreeMajor.UserName

                };
                _majorRepository.AddAsync(userDegreeMajor);
                return StatusCode(200, CustomResult.Ok(degreeMajor));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteDegree(int degreeMajorId)
        {
            try
            {
                var major = _majorRepository.FirstOrDefault(s => s.Id == degreeMajorId);
                if (major != null)
                {
                    _majorRepository.DeleteAsync(major);
                    return StatusCode(200, CustomResult.Ok("OK"));
                }
                return StatusCode(400, CustomResult.NotFound());
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserInfo userInfo)
        {
            try
            {
                UserInfoDetails userInfoDetails = new UserInfoDetails
                {
                    AtmCard = userInfo.AtmCard,
                    Address = userInfo.Address,
                    Birthday = userInfo.Birthday,
                    City = userInfo.City,
                    Country = userInfo.Country,
                    Gender = userInfo.Gender,
                    Grade = userInfo.Grade,
                    Job = userInfo.Job,
                    Mobile = userInfo.Mobile
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
        [Authorize(Roles = "Admin,SurveyDesigner")]
#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserInfo userInfo)
        {
            try
            {
                UserCategory userCategory = _categoryRepository.FirstOrDefault(s => s.Id == userInfo.UserCategoryId);
                UserInfoDetails userInfoDetails = new UserInfoDetails
                {
                    Category = userCategory,
                    AtmCard = userInfo.AtmCard,
                    Address = userInfo.Address,
                    Birthday = userInfo.Birthday,
                    City = userInfo.City,
                    Country = userInfo.Country,
                    Gender = userInfo.Gender,
                    Grade = userInfo.Grade,
                    Job = userInfo.Job,
                    Mobile = userInfo.Mobile,
                    Email = userInfo.Email
                };
                userInfo.UpdateUserInfo(userInfoDetails);
                _userInfoService.UpdateUserInfo(userInfo);
                return StatusCode(200, CustomResult.Ok(userInfo));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
    }
}
