using Core.Entities;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class QuestionDto
    {
        public string? SheetId { get; set; }
        public int? SheetVersion { get; set; }
        public QuestionTypeEnum Type { get; set; }
        public string? UserId { get; set; }
        public string Text { get; set; }
        public int? VariableId { get; set; }
        public bool Required { get; set; }
        public int Order { get; set; }
    }
}
