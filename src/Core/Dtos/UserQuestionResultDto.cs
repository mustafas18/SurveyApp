using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    /// <summary>
    /// This class is an immutable  object. 
    /// </summary>
    public class UserQuestionResultDto
    {
        public UserQuestionResultDto(int questionId, string questionText, int totalAnswers, List<UserAnswerResultDto> answers)
        {
            QuestionId = questionId;
            QuestionText = questionText;
            TotalAnswers = totalAnswers;
            Answers = answers;
        }
        public int QuestionId { get; private set; }
        public string QuestionText { get; private set; }
        public int TotalAnswers { get; private set; }
        public List<UserAnswerResultDto> Answers { get; private set; }
    }
}
