using Domain.Dtos;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICSharpCompiler
    {
        Task<List<CompilerDiagnoisticDto>> CompileCode(string guidId, string script,CompileEnvironment compileEnvironment);
        Task CompileCode(string guidId);
        Task<List<CompilerDiagnoisticDto>> TestCode(string sheetId,string script);
    }
}
