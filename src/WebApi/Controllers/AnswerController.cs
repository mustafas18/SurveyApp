using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IUserAnswerService _userAnswerService;

        public AnswerController(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IUserAnswerService userAnswerService)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userAnswerService = userAnswerService;
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create(int surveyId, List<UserAnswerViewModel> answers)
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault().Subject.Name;
                List<UserAnswer> userAnswers = new();
                answers.ForEach(ans =>
                {
                    ans.answer.ForEach(s =>
                    {
                        userAnswers.Add(new UserAnswer
                        {
                            QuestionId = ans.questionId,
                            InputValue = s,
                            UserName = userName,
                            SurveyId = surveyId
                        });
                    });
                });
                _userAnswerService.Create(userAnswers);

                return StatusCode(200, CustomResult.Ok(userAnswers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBySurveyId(int surveyId)
        {
            try
            {
                var userAnswers =await _userAnswerService.GetBySurveyId(surveyId);
                return StatusCode(200, CustomResult.Ok(_mapper.Map<List<UserAnswerDto>>(userAnswers)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
    }
}
