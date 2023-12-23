using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class VariableWithValuesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MessureEnum Messure { get; set; }
        public string? VariableLabel { get; set; }
        public string? ValueLabel { get; set; }
        public string? Value { get; set; }
    }
}
