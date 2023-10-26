using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Interfaces;

namespace Core.Models
{
    public class Question : BaseEntity, IAggregateRoot
    {
        public string SheetId { get; set; }
        public int SheetVersion { get; set; }
        public QuestionTypeEnum Type { get; set; }
        public string UserId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionFileUri { get; set; }
        public string QuestionFileContentType { get; set; }
        public int VariableId { get; set; }
        public List<QuestionAnswer> QuestionAnswers { get; set; }
        public int ResponseTime { get; set; }
        public bool Required { get; set; }
        public bool Deleted { get; set; }
    }
}
