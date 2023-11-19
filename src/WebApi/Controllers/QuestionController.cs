using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
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

        public QuestionController(IQuestionService questionService,
            IMapper mapper)
        {
            _questionService = questionService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetBySheetId(string sheetId,int? sheetVersion)
        {
            try
            {
                var result =await _questionService.GetBySheetId(sheetId, sheetVersion);
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
                questionViewModel.Answers.ForEach(a => question.Answers.Add(new QuestionAnswer(a.Text,a.Value)));
                var result = _questionService.CreateAsync(questionViewModel.SheetId, question);
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
    }
}
