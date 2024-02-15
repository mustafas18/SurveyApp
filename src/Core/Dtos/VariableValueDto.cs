using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class VariableValueDto
    {
        public string SurveyGuid { get; set; }
        public string VariableName { get;  set; }
        public string InputValue { get;  set; }
    }
}
