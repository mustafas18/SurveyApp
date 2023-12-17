using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class QuestionAnswer : BaseEntity
    {
        public QuestionAnswer()
        {
            
        }
        public QuestionAnswer(string text, int value)
        {
            Text = text;
            Value = value;
        }
        public string Text { get; set; }
        public string? File { get; set; }
        public string? FileType { get; set; }
        public int Value { get; set; }

    }
}
