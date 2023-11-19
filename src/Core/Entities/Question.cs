using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Interfaces;

namespace Core.Entities
{
    public class Question : BaseEntity, IAggregateRoot
    {
        public Question()
        {
            
        }
    
        public string SheetId { get; set; }
        public int SheetVersion { get; set; }
        public QuestionTypeEnum Type { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public int Value { get; }
        public int? VariableId { get; set; }
        public string? FileUri { get; set; }
        public string? FileContentType { get; set; }
        public ICollection<QuestionAnswer>? Answers { get; set; }
        public int? ResponseTime { get; set; }
        public int Order { get; set; }
        public bool Required { get; set; }
        public bool Deleted { get; set; }
    }
}
