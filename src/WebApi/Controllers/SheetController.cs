using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.IRepositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SheetController : BaseApiController
    {
        private readonly ISheetRepository sheetRepository;
        private readonly IEfRepository<Sheet> _efRepository;
        private readonly ISheetService _sheetService;
        private readonly IMapper _mapper;

        public SheetController(ISheetRepository sheetRepository,
            IEfRepository<Sheet> efRepository,
            ISheetService sheetService,
            IMapper mapper)
        {
            this.sheetRepository = sheetRepository;
            _efRepository = efRepository;
            _sheetService = sheetService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetSheetList()
        {
            try
            {
                var result = sheetRepository.GetSheetList();
                return StatusCode(200,CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));                
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateSheet([FromForm] SheetViewModel sheetViewModel)
        {
            try
            {
                var result = _sheetService.CreateSheetAsync(_mapper.Map<Sheet>(sheetViewModel));
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
    }
}
