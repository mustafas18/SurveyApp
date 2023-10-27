using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class VariableValueLabel : BaseEntity
    {
        public int Value { get; set; }
        public string Label { get; set; }
    }
}
