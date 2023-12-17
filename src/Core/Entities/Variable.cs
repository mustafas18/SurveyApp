using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Variable:BaseEntity,IAggregateRoot
    {
        public string Name { get; set; }
        public VariableTypeEnum Type { get; set; }
        public string Label { get; set; }
        public int MaxValue { get; set; }
        public string? ValuesAsString { get; set; }
        public List<VariableValueLabel>? Values { get; set; }
        public MessureEnum Messure { get; set; }
        public string SheetId { get; set; }
        public int SheetVersion { get; set; }
        public bool Deleted { get; set; }
    }
}
