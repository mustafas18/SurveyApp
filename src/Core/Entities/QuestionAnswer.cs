using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class QuestionAnswer : BaseEntity
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public string? File { get; set; }
        public string? FileType { get; set; }
        public int Value { get; set; }

    }
}
