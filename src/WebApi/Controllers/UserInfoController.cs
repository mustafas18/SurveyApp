using ClosedXML.Excel;
using Domain.Dtos;
using Domain.Entities;
using Domain.Extension;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq.Expressions;
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
                UserFullNameViewModel userFullNameViewModel = new UserFullNameViewModel(result.Id, userName, result.FullName);
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
                var majors = _majorRepository.Include("Degree")
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
                var degree = _degreeRepository.FirstOrDefault(s => s.Id == degreeMajor.DegreeId);
                UserDegreeMajor userDegreeMajor = new UserDegreeMajor
                {
                    Degree = degree,
                    MajorTitle = degreeMajor.MajorTitle,
                    UserName = degreeMajor.UserName

                };
                await _majorRepository.AddAsync(userDegreeMajor);
                return StatusCode(200, CustomResult.Ok(degreeMajor));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteDegree(int majorId)
        {
            try
            {
                var major = _majorRepository.FirstOrDefault(s => s.Id == majorId);
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

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDto uploadViewModel)
        {
            try
            {
                if (uploadViewModel.Data.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        uploadViewModel.Data.CopyTo(ms);
                        uploadViewModel.DataBytes = ms.ToArray();
                        //string s = Convert.ToBase64String(fileBytes);
                    }
                }
                var result = await _userInfoService.UploadCV(uploadViewModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
        [HttpGet]
        public async Task<IActionResult> DownloadFile([FromQuery] int userInfoId)
        {
            try
            {
                var result= _userInfoService.DownloadCV(userInfoId);
                return File(result.DataBytes,result.FileContent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserInfo userInfo)
        {
            try
            {
                var category = _categoryRepository.FirstOrDefault(s => s.Id == userInfo.CategoryId);
                UserInfoDetails userInfoDetails = new UserInfoDetails
                {
                    Category = category,
                    AtmCard = userInfo.AtmCard,
                    Address = userInfo.Address,
                    Birthday = userInfo.Birthday,
                    Email = userInfo.Email,
                    City = userInfo.City,
                    Country = userInfo.Country,
                    Gender = userInfo.Gender,
                    Grade = userInfo.Grade,
                    Job = userInfo.Job,
                    Mobile = userInfo.Mobile,
                    ResearchInterest = userInfo.ResearchInterests
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
                UserCategory userCategory = _categoryRepository.FirstOrDefault(s => s.Id == userInfo.CategoryId);
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
                    Email = userInfo.Email,
                    ResearchInterest = userInfo.ResearchInterests

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
#if DEBUG
        [AllowAnonymous]
#endif  
        [HttpGet]
        public async Task<IActionResult> ExportExcel()
        {
            try
            {
                var userList = _userInfoService.GetUserInfoList();

                var userDataSet = userList.ToDataSet();

                string base64String;

                using (var wb = new XLWorkbook())
                {
                    var sheet = wb.AddWorksheet(userDataSet.Tables[0]);

                    // Apply font color to columns 1 to 5
                    sheet.Columns(1, 5).Style.Font.FontColor = XLColor.Black;

                    using (var ms = new MemoryStream())
                    {
                        wb.SaveAs(ms);

                        // Convert the Excel workbook to a base64-encoded string
                        base64String = Convert.ToBase64String(ms.ToArray());
                    }
                }

                // Return a CreatedResult with the base64-encoded Excel data
                return new CreatedResult(string.Empty, new
                {
                    Code = 200,
                    Status = true,
                    Message = "",
                    Data = base64String
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
    }
}
