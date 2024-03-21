using DocumentFormat.OpenXml.Bibliography;
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

        public ScriptController(ICSharpCompiler compiler,IRepository<Script> scriptRepository)
        {
            _compiler= compiler;
            _scriptRepository = scriptRepository;
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet]
        public async Task<IActionResult> CompileScript(string surveyGuid, string script)
        {
            try
            {
                await _compiler.CompileCode(surveyGuid, script);

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
        public async Task<IActionResult> AddUpdateScript(string sheetId, string script)
        {
            try
            {
                var sheetScript = _scriptRepository.FirstOrDefault(s => s.SheetId == sheetId);
                if(sheetScript == null)
                {
                    await _scriptRepository.AddAsync(new Script
                    {
                        SheetId = sheetId,
                        Code = script
                    });
                }
                else
                {
                    sheetScript.Code = script;
                    await _scriptRepository.UpdateAsync(sheetScript);
                }

                return StatusCode(200, CustomResult.Ok(sheetScript));
                ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, CustomResult.InternalError(ex));
            }
        }
    }
}
