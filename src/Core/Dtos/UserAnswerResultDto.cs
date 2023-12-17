using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class UserAnswerResultDto
    {
        public UserAnswerResultDto(string input, int count,string inputLabel)
        {
            InputLabel = inputLabel;
            InputValue = input;
            Count = count;
        }
        public string InputLabel { get; set; }
        public string InputValue { get; set; }
        public int Count { get; set; }
    }
}
