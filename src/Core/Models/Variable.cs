using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Variable:BaseEntity
    {
        public string Name { get; set; }
        public VariableTypeEnum Type { get; set; }
        public string SheetId { get; set; }
        public int SheetVersion { get; set; }
        public bool Deleted { get; set; }
    }
}
