using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserCategoryController : ControllerBase
    {

        private readonly IRepository<UserCategory> _repository;
        private readonly IUserCategoryRepository _categoryRepository;

        public UserCategoryController(IRepository<UserCategory> repository,
            IUserCategoryRepository categoryRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _categoryRepository.GetListAsync(1);
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [HttpGet]
#if DEBUG
        [AllowAnonymous]
#endif
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _repository.FirstOrDefaultAsync(s => s.Id == id);
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }

        [HttpGet]
#if DEBUG
        [AllowAnonymous]
#endif
        public async Task<IActionResult> GetCategoryUsers(int categoryId)
        {
            try
            {
                var result = await _categoryRepository.GetCategoryUsers(categoryId);
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [HttpPost]
#if DEBUG
        [AllowAnonymous]
#endif
        public async Task<IActionResult> Create(UserCategory userCategory)
        {
            try
            {
                await _repository.AddAsync(userCategory);
                return StatusCode(200, CustomResult.Ok(userCategory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [HttpPut]
#if DEBUG
        [AllowAnonymous]
#endif
        public async Task<IActionResult> Update(UserCategory userCategory)
        {
            try
            {
                await _repository.UpdateAsync(userCategory);
                return StatusCode(200, CustomResult.Ok(userCategory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }
        [HttpDelete]
#if DEBUG
        [AllowAnonymous]
#endif
        public async Task<IActionResult> Delete (int id)
        {
            try
            {
                var result = _repository.FirstOrDefault(s => s.Id == id);
                result.IsDelete = true;
                await _repository.UpdateAsync(result);
                return StatusCode(200, CustomResult.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }

        }

    }
}
