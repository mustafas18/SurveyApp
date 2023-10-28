using AutoMapper;
using Core.Entities;
using Core.Interfaces;
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
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] QuestionViewModel questionViewModel)
        {
            try
            {
                var result = _questionService.CreateAsync(questionViewModel.SheetId,_mapper.Map<Question>(questionViewModel));
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
    }
}
