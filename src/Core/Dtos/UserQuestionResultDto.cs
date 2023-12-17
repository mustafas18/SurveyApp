using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class UserQuestionResultDto
    {
        public UserQuestionResultDto(int questionId, string questionText,int totalAnswers, List<UserAnswerResultDto> answers)
        {
            QuestionId = questionId;
            QuestionText = questionText;
            TotalAnswers = totalAnswers;
            Answers = answers;
        }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int TotalAnswers { get; set; }
        public List<UserAnswerResultDto> Answers { get; set; }
    }
}
