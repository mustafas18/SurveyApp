using Domain.Dtos;
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
                var surveyGuid = _surveyService.GetSurveyGuid(surveyId);
                var survey = await _surveyService.GetSurveyAsync(surveyGuid);
                return StatusCode(200, CustomResult.Ok(survey));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetByGuid(string guid)
        {
            try
            {
                var survey = await _surveyService.GetSurveyAsync(guid);
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
 
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetId(string guid)
    {
        try
        {
            var surveyId =  _surveyService.GetSurveyId(guid);
            return StatusCode(200, CustomResult.Ok(surveyId));
        }
        catch (Exception ex)
        {
            return StatusCode(500, CustomResult.InternalError(ex));
        }
    }
}
}