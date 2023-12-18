using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class VariableViewDto
    {
        public VariableViewDto(int variableId, string inputValue, string answerLabel,int answerCount)
        {
            VariableId=variableId;
            AnswerLabel = answerLabel;
            InputValue= Convert.ToInt32(inputValue);
            AnswerCount = answerCount;
        }
        public int VariableId { get; private set; }
        public string AnswerLabel { get;private set; }      
        public int InputValue { get; private set; }
        public int AnswerCount { get; private set; }

    }
}
