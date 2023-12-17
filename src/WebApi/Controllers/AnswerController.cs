using AutoMapper;
using Core.Entities;
using Core.Enums;
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
        private readonly ISurveyService _surveyService;
        private readonly IRepository<Question> _questionRepository;

        public AnswerController(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IUserAnswerService userAnswerService,
            ISurveyService surveyService,
            IRepository<Question> questionRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userAnswerService = userAnswerService;
            _surveyService = surveyService;
            _questionRepository = questionRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create(int surveyId, List<UserAnswerViewModel> answers)
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault().Subject.Name;
                List<UserAnswer> userAnswers = new();
                answers.ForEach(ans =>
                {
                    var question = _questionRepository.AsNoTracking()
                                        .FirstOrDefault(v => v.Id == ans.questionId);
                    ans.answer.ForEach(s =>
                    {
                        userAnswers.Add(new UserAnswer
                        {
                            QuestionId = ans.questionId,
                            VariableId = question.VariableId,
                            InputValue = s,
                            UserName = userName,
                            SurveyId = surveyId
                        });
                    });
                });
                await _userAnswerService.Create(userAnswers);
                await _surveyService.UpdateStatus(surveyId,SurveyStatusEnum.Completed);
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
