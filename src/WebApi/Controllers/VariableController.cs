using AutoMapper;
using ClosedXML.Excel;
using Domain.Entities;
using Domain.Extension;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class VariableController : BaseApiController
    {
        private readonly IVariableService _variableService;
        private readonly IMapper _mapper;

        public VariableController(IVariableService variableService,
            IMapper mapper)
        {
            _variableService = variableService;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBySheetId(string sheetId, int? sheetVersion)
        {
            try
            {
                var result = await _variableService.GetBySheetId(sheetId, sheetVersion);
                if (result == null)
                {
                    return StatusCode(200, CustomResult.Ok(null));
                }
                var variableViewModel = _mapper.Map<IEnumerable<VariableViewModel>>(result);
                return StatusCode(200, CustomResult.Ok(variableViewModel));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VariableViewModel variableViewModel)
        {
            try
            {
                var result = await _variableService.Create(_mapper.Map<Variable>(variableViewModel));
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int variableId)
        {
            try
            {
                var result =await _variableService.DeleteAsync(variableId);
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ReportBySheetId(string sheetId, int? version)
        {
            try
            {
                var userAnswers =await _variableService.ReportBySurveyId(sheetId, version);
                return StatusCode(200, CustomResult.Ok(userAnswers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> IndividualVariableReport(int surveyId)
        {
            try
            {
                var result = await _variableService.SurveyReport(surveyId);
                return StatusCode(200, CustomResult.Ok(result));
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
        public async Task<IActionResult> GetSheetData(string sheetId)
        {
            try
            {
                var dataSet =await  _variableService.SheetData(sheetId,null);

                return StatusCode(200, CustomResult.Ok(dataSet.DataSetToBase64()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
    }
}
