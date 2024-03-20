using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class VariableSurveyResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Label { get; set; }
        public VariableTypeEnum? Type { get; set; }
        public int Sum { get; set; }
        public int? Value { get; set; }
    }
}
