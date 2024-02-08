using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class QuestionDto
    {
        public int? Id { get; set; }
        public string? SheetId { get; set; }
        public int? SheetVersion { get; set; }
        public QuestionTypeEnum Type { get; set; }
        public string? UserId { get; set; }
        public string Text { get; set; }
        public int? VariableId { get; set; }
        public bool Required { get; set; }
        public int Order { get; set; }
        public List<AnswerDto>? Answer { get; set; }
    }
    public class AnswerDto
    {
        public string text { get; set; }
        public int value { get; set; }
    }
}
