﻿using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SheetController(ISheetRepository sheetRepository,
            IRepository<Sheet> efRepository,
            ISheetService sheetService,
            IUserInfoService userInfoService,
            IUserService userService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IRedisCacheService redisCacheService)
        {
            this.sheetRepository = sheetRepository;
            _efRepository = efRepository;
            _sheetService = sheetService;
            _userInfoService = userInfoService;
            _userService = userService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _redisCacheService = redisCacheService;
        }
#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet]
        public async Task<IActionResult> GetSheetList()
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault()?.Subject.Name;
                var result = sheetRepository.GetSheetList(userName);

                return StatusCode(200, CustomResult.Ok(_mapper.Map<List<SheetDto>, List<SheetViewModel>>(result.Result.ToList())));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string sheetId, int version = 1, bool cache = true)
        {
            try
            {
                if (cache)
                {
                    //var cacheData = _redisCacheService.GetData<SheetDto>($"sheet_{sheetId}_{version}");
                    //if (cacheData != null)
                    //{
                    //    return StatusCode(200, CustomResult.Ok(cacheData));
                    //}
                   var cacheData = await _sheetService.GetByIdAsync(sheetId);
                    var expirationTime = TimeSpan.FromMinutes(1);
                   // await _redisCacheService.SetDataAsync<SheetDto>($"sheet_{sheetId}_{version}", cacheData, expirationTime);
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
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSheetInfo(string sheetId, int? version)
        {
            try
            {
                var result = await _sheetService.GetSheetInfo(sheetId, version);
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
                var userName = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault().Subject.Name;
                if (sheetViewModel.DeadlineString != null && sheetViewModel.DeadlineString != "")
                {
                    sheetViewModel.DeadlineTime = DateTime.ParseExact(sheetViewModel.DeadlineString, "yyyy-MM-dd HH:mm",
                                            System.Globalization.CultureInfo.InvariantCulture);
                }

                sheetViewModel.CreatedByUserId = userName ?? "";
                var result = await _sheetService.CreateAsync(_mapper.Map<Sheet>(sheetViewModel));
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SheetViewModel sheetViewModel)
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault().Subject.Name;
                if (sheetViewModel.DeadlineString != null && sheetViewModel.DeadlineString != "")
                {
                    sheetViewModel.DeadlineTime = DateTime.ParseExact(sheetViewModel.DeadlineString, "yyyy-MM-dd HH:mm",
                                            System.Globalization.CultureInfo.InvariantCulture);
                }

                sheetViewModel.CreatedByUserId = userName ?? "";
                var result = await _sheetService.UpdateAsync(_mapper.Map<Sheet>(sheetViewModel));
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }

    }
}
