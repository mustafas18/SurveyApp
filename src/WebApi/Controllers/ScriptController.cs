using DocumentFormat.OpenXml.Bibliography;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScriptController : ControllerBase
    {
        private readonly ICSharpCompiler _compiler;
        private readonly IRepository<Script> _scriptRepository;
        private readonly IRepository<Sheet> _sheetRepository;

        public ScriptController(ICSharpCompiler compiler,IRepository<Script> scriptRepository,
            IRepository<Sheet> sheetRepository)
        {
            _compiler= compiler;
            _scriptRepository = scriptRepository;
            _sheetRepository = sheetRepository;
        }
#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet]
        public async Task<IActionResult> GetSheetScript(string sheetId)
        {
            try
            {
                var latestVersion = _sheetRepository.Where(s2 => s2.SheetId == sheetId).Max(s2 => s2.Version);
                var sheet = _sheetRepository.FirstOrDefault(s => s.SheetId == sheetId && s.Version == latestVersion);
                if (sheet.Script == null)
                {
                    return StatusCode(200, CustomResult.Ok(null));
                }

                return StatusCode(200, CustomResult.Ok(sheet.Script));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet]
        public async Task<IActionResult> CompileScript(string surveyGuid)
        {
            try
            {
                await _compiler.CompileCode(surveyGuid);

                return StatusCode(200, CustomResult.Ok("OK"));
                ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPost]
        public async Task<IActionResult> AddUpdateScript([FromBody] ScriptDto script)
        {
            try
            {
                var latestVersion = _sheetRepository.Where(s2 => s2.SheetId == script.SheetId).Max(s2 => s2.Version);
                var sheet = _sheetRepository.FirstOrDefault(s => s.SheetId == script.SheetId && s.Version== latestVersion);
                sheet.Script = new Script(script.Code);
                sheet.Script.Code = script.Code;
                await _sheetRepository.SaveChangesAsync();

                return StatusCode(200, CustomResult.Ok(sheet.Script));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
    }
}
