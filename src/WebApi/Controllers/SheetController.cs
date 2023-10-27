using Core.Dtos;
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

        public SheetController(ISheetRepository sheetRepository)
        {
            this.sheetRepository = sheetRepository;
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
    }
}
