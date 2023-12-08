using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.IRepositories;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;
using WebApi.ViewModels.Acconut;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SheetController : BaseApiController
    {
        private readonly ISheetRepository sheetRepository;
        private readonly IRepository<Sheet> _efRepository;
        private readonly ISheetService _sheetService;
        private readonly IUserInfoService _userInfoService;
        private readonly IUserService _userService;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IMapper _mapper;

        public SheetController(ISheetRepository sheetRepository,
            IRepository<Sheet> efRepository,
            ISheetService sheetService,
            IUserInfoService userInfoService,
            IUserService userService,
            IMapper mapper,
            IRedisCacheService redisCacheService)
        {
            this.sheetRepository = sheetRepository;
            _efRepository = efRepository;
            _sheetService = sheetService;
            _userInfoService = userInfoService;
            _userService = userService;
            _mapper = mapper;
            _redisCacheService = redisCacheService;
        }
        [HttpGet]
        public async Task<IActionResult> GetSheetList()
        {
            try
            {
                var result = sheetRepository.GetSheetList();

                return StatusCode(200,CustomResult.Ok(_mapper.Map<List<SheetDto>,List<SheetViewModel>>(result.Result.ToList())));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));                
            }

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string sheetId,bool cache=true)
        {
            try
            {
                if(cache)
                {
                    var cacheData = _redisCacheService.GetData<SheetDto>($"sheet_{sheetId}");
                    if (cacheData != null)
                    {
                        return StatusCode(200, CustomResult.Ok(cacheData));
                    }
                    cacheData = await _sheetService.GetByIdAsync(sheetId);
                    var expirationTime = TimeSpan.FromMinutes(1);
                    await _redisCacheService.SetDataAsync<SheetDto>($"sheet_{sheetId}", cacheData, expirationTime);
                    return StatusCode(200, CustomResult.Ok(cacheData));
                }
                var result = await _sheetService.GetByIdAsync(sheetId);
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SheetViewModel sheetViewModel)
        {
            try
            {
                sheetViewModel.CreatedByUserId = _userService.GetCurrentUserAsync().Result?.Id ?? "";
                var result = await _sheetService.CreateAsync(_mapper.Map<Sheet>(sheetViewModel));
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }

    }
}
