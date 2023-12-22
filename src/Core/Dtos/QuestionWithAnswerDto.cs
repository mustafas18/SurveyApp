using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class QuestionWithAnswerDto
    {
        public int QuestionId { get;private set; }
        public int AnswerId { get; private set; }
        public string? AnswerText { get; private set; }

    }
}
