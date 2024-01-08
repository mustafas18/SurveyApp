using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserCategoryController : ControllerBase
    {
        private readonly IReadRepository<UserCategory> _readRepository;
        private readonly IRepository<UserCategory> _repository;

        public UserCategoryController(IReadRepository<UserCategory> readRepository,
            IRepository<UserCategory> repository)
        {
            _readRepository = readRepository;
            _repository = repository;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _readRepository.ListAsync();
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
                var result = await _readRepository.GetByIdAsync(id);
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
        [HttpPost]
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
