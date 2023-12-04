using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Migrations;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class QuestionController : BaseApiController
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _redisCacheService;

        public QuestionController(IQuestionService questionService,
            IMapper mapper,
            IRedisCacheService redisCacheService)
        {
            _questionService = questionService;
            _mapper = mapper;
            _redisCacheService = redisCacheService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBySheetId(string sheetId,int? sheetVersion=1)
        {
            try
            {
                var cacheData = _redisCacheService.GetData<IEnumerable<QuestionDto>>($"{sheetId}.{sheetVersion}.question");
                if (cacheData != null)
                {
                    return StatusCode(200, CustomResult.Ok(cacheData));
                }
                cacheData = await _questionService.GetBySheetId(sheetId, sheetVersion);
                var expirationTime = DateTimeOffset.Now.AddDays(1);
                await _redisCacheService.SetDataAsync<IEnumerable<QuestionDto>>($"{sheetId}.{sheetVersion}.question", cacheData, expirationTime);
                return StatusCode(200, CustomResult.Ok(cacheData));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuestionViewModel questionViewModel)
        {
            try
            {
                var question = new Question
                {
                    VariableId = questionViewModel.VariableId,
                    Required = questionViewModel.Required,
                    Text = questionViewModel.Text,
                    UserId = "",
                    SheetId = questionViewModel.SheetId,
                    Type = (QuestionTypeEnum)questionViewModel.Type,
                };
                question.Answers = new List<QuestionAnswer>();
                questionViewModel.Answers.ForEach(a => question.Answers.Add(new QuestionAnswer(a.Text,a.Value)));
                var result = _questionService.CreateAsync(questionViewModel.SheetId, question);
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuestionOrder([FromBody] QuestionOrderViewModel questionVewModel)
        {
            try
            {
                var result = _questionService.UpdateQuestionOrder(_mapper.Map<QuestionOrderDto>(questionVewModel));
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
    }
}
