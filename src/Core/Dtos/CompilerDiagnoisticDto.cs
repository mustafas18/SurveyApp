using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class CompilerDiagnoisticDto
    {
        public CompilerDiagnoisticDto(Microsoft.CodeAnalysis.DiagnosticSeverity diagnossticSeverityEnum,
            string message) 
        {
            Message= message;
            DiagnossticSeverity = diagnossticSeverityEnum;
        }
        public string Message { get; protected set; }
        public Microsoft.CodeAnalysis.DiagnosticSeverity DiagnossticSeverity { get; protected set; }
    }
}
