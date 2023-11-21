﻿using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.IRepositories;
using Infrastructure.Services;
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
        private readonly IMapper _mapper;

        public SheetController(ISheetRepository sheetRepository,
            IRepository<Sheet> efRepository,
            ISheetService sheetService,
            IUserInfoService userInfoService,
            IUserService userService,
            IMapper mapper)
        {
            this.sheetRepository = sheetRepository;
            _efRepository = efRepository;
            _sheetService = sheetService;
            _userInfoService = userInfoService;
            _userService = userService;
            _mapper = mapper;
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
        public async Task<IActionResult> GetSheetListWithQuestions()
        {
            try
            {
                var result = sheetRepository.GetSheetList();
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
        [HttpGet]
        public async Task<IActionResult> GetById(string sheetId)
        {
            try
            {
                var result = _sheetService.GetByIdAsync(sheetId);
                SheetViewModel sheetInfoViewModel = _mapper.Map<SheetViewModel>(result);
                var userInfo = _userInfoService.GetUserInfo(sheetInfoViewModel.UserId);
                sheetInfoViewModel.UserFullName = userInfo.FullName;
                return StatusCode(200, CustomResult.Ok(sheetInfoViewModel));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
    }
}
