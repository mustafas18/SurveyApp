using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json.Linq;
using System.Collections.Immutable;
using WebApi.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private readonly IRepository<UserSurvey> _surveyRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Sheet> _sheetRepository;

        public AnswerController(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IUserAnswerService userAnswerService,
            ISurveyService surveyService,
            IRepository<UserSurvey> surveyRepository,
            IRepository<Question> questionRepository,
            IRepository<Sheet> sheetRepository
         )
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userAnswerService = userAnswerService;
            _surveyService = surveyService;
            _surveyRepository = surveyRepository;
            _questionRepository = questionRepository;
            _sheetRepository = sheetRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create(int surveyId, List<UserAnswerViewModel> answers)
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault()?.Subject.Name;
                List<UserAnswer> userAnswers = new();
                SurveyInvitationDto? survey = _surveyRepository.AsNoTracking()
                                       .Select(p => new SurveyInvitationDto
                                       {
                                           id = p.Id,
                                           guid = p.Guid,
                                           sheetId = p.SheetId,
                                           userName = p.UserName,
                                           version = p.Version
                                       })
                                       .FirstOrDefault(s => s.id == surveyId);
                var newVersion = (survey.version ?? 0) + 1;

                foreach (var ans in answers)
                {
                    var question = _questionRepository.Include("Answers")
                                        .FirstOrDefault(v => v.Id == ans.questionId && v.Deleted == false);
                    if (question == null)
                    {
                        continue;
                    }
                    var sheet = _sheetRepository.FirstOrDefault(s => s.SheetId == question.SheetId && s.Version == question.SheetVersion);
                    ans.answer.ForEach(s =>
                    {
                        int number = 0;
                        bool ConvertableToInt = Int32.TryParse(s, out number);
                        string? inputLabel = null;
                        int answerId = 0;
                        if (ConvertableToInt)
                        {
                            var answer = question.Answers.FirstOrDefault(a => a.Value == number);
                            inputLabel = answer?.Text;
                            answerId = answer?.Id ?? 0;
                        }

                        userAnswers.Add(new UserAnswer
                        {
                            SheetId = sheet.Id,
                            SurveyGuid = survey.guid,
                            AnswerId = answerId,
                            QuestionType = question.Type,
                            SurveyId = survey.id ?? 1,
                            SurveyVersion = newVersion,
                            QuestionId = ans.questionId,
                            VariableId = question.VariableId,
                            InputLabel = inputLabel,
                            InputValue = s,
                            UserName = userName ?? "guest"
                        });
                    });
                
                
                }
                await _userAnswerService.Create(userAnswers);
                _ = await  _surveyService.ReviseSurveyAsync(survey);
                //await _surveyService.UpdateStatus(surveyId, SurveyStatusEnum.Completed);
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
                var userAnswers = await _userAnswerService.GetBySurveyId(surveyId);
                return StatusCode(200, CustomResult.Ok(_mapper.Map<List<UserAnswerDto>>(userAnswers)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ReportBySheetId(string sheetId, int? version)
        {
            try
            {
                var userAnswers = _userAnswerService.ReportBySurveyId(sheetId, version);
                return StatusCode(200, CustomResult.Ok(userAnswers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
    }
}
