using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICSharpCompiler
    {
        Task CompileCode(string guidId, string script);
        Task CompileCode(string guidId);
    }
}
