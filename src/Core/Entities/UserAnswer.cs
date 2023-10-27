using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UserAnswer:BaseEntity
    {

        public int UserSurveyId { get; set; }
        public int QuestionId { get; set; }
        public int QuestionAnswerId { get; set; }
        public string UserId { get; set; }
        public string InputValue { get; set; }
        public int UserResponseTime { get; set; }
    }
}
