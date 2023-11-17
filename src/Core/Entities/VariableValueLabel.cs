using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class VariableValueLabel : BaseEntity
    {
        public VariableValueLabel()
        {
                
        }
        public VariableValueLabel(int value,string label)
        {
            Value = value;
            Label = label;
        }
        public int Value { get; set; }
        public string Label { get; set; }
    }
}
