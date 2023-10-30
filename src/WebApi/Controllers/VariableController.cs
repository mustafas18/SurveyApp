using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;
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
        [HttpPost]
        public IActionResult Create([FromBody] VariableViewModel variableViewModel)
        {
            try
            {
                var result = _variableService.Create(_mapper.Map<Variable>(variableViewModel));
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
    }
}
