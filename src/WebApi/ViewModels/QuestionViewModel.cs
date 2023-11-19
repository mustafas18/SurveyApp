﻿using Core.Entities;
using Core.Enums;

namespace WebApi.ViewModels
{
    public class QuestionViewModel
    {
        public string SheetId { get; set; }
        public int Type { get; set; }
        public string Text { get; set; }
        public int? VariableId { get; set; }
        public List<AnswerViewModel> Answers { get; set; }
        public bool Required { get; set; }
    }

    public class AnswerViewModel
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }
}
