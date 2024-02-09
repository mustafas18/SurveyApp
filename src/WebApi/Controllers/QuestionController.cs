using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class QuestionController : BaseApiController
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IRedisCacheService _redisCacheService;

        public QuestionController(IQuestionService questionService,
            IMapper mapper,
            IMediator mediator,
            IRedisCacheService redisCacheService)
        {
            _questionService = questionService;
            _mapper = mapper;
            _mediator = mediator;
            _redisCacheService = redisCacheService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBySheetId(string sheetId, int? sheetVersion = 1)
        {
            try
            {
                var result = await _questionService.GetBySheetId(sheetId, sheetVersion);
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetQuestionWithAnswers(int questionId)
        {
            try
            {
                var result = await _questionService.GetQuestionAnswers(questionId);
                return StatusCode(200, CustomResult.Ok(result));
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
                questionViewModel.Answers.ForEach(a => question.Answers.Add(new QuestionAnswer(a.Text, a.Value)));
                if (questionViewModel.Id == 0)
                {
                    var result = await _questionService.CreateAsync(questionViewModel.SheetId, question);
                    return StatusCode(200, CustomResult.Ok(result));
                }
                else
                {
                    question.Id = questionViewModel.Id ?? 0;
                    var result = await _questionService.UpdateAsync(questionViewModel.SheetId, question);
                    return StatusCode(200, CustomResult.Ok(result));
                }

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

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int questionId)
        {
            try
            {
                var result=await _questionService.DeleteQuestionAsync(questionId);
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
           }
}
