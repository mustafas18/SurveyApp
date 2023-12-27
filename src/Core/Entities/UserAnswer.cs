using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserAnswer : BaseEntity
    {
        public int SheetId { get; set; }
        public int SurveyId { get; set; }
        public int SurveyVersion { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public QuestionTypeEnum QuestionType { get; set; }
        public int? VariableId { get; set; }
        public string UserName { get; set; }
        public string? InputLabel { get; set; }
        public string? InputValue { get; set; }
        public int UserResponseTime { get; set; }
    }
}
