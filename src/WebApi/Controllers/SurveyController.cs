using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;

        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }
#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPost]
        public async Task<IActionResult> Create(string sheetId, string? userName)
        {
            try
            {
                var survey = await _surveyService.CreateSurveyAsync(sheetId, userName);
                return StatusCode(200, CustomResult.Ok(survey));
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
        public async Task<IActionResult> GetById(int surveyId)
        {
            try
            {
                var survey = await _surveyService.GetSurveyAsync(surveyId);
                return StatusCode(200, CustomResult.Ok(survey));
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
        public async Task<IActionResult> GetSurveyList(string sheetId)
        {
            try
            {
                var surveyList = await _surveyService.GetSurveyListAsync(sheetId);
                return StatusCode(200, CustomResult.Ok(surveyList));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
    }
}
