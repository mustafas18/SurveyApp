﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UserAnswer:BaseEntity
    {

        public int SurveyId { get; set; }
        public int QuestionId { get; set; }
        public string UserName { get; set; }
        public string? InputValue { get; set; }
        public int UserResponseTime { get; set; }
    }
}
