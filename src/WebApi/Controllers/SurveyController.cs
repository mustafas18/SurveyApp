using Core.Dtos;
using Core.Entities;
using Core.Enums;
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
        public async Task<IActionResult> Create(SurveyInvitationDto invitaiton)
        {
            try
            {
                var survey = await _surveyService.CreateSurveyAsync(invitaiton);
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
#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPut]
        public async Task<IActionResult> UpdateStatus(int surveyId,int status= (int)SurveyStatusEnum.Completed)
        {
            try
            {
                await _surveyService.UpdateStatus(surveyId, (SurveyStatusEnum)status);
                return StatusCode(200, CustomResult.Ok(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
    }
}
