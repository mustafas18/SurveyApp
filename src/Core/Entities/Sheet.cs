﻿using Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Sheet : BaseEntity, IAggregateRoot
    {
        public Sheet()
        {
                
        }
          public string SheetId { get; set; }
        public int Version { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public int TemplateId { get; set; }
        public List<UserInfo>? Users { get; set; }
        public int LanguageId { get; set; }
        public int WelcomePageId { get; set; }
        public int EndPageId { get; set; }
        public string Link { get; set; }
        public int DurationTime { get; set; } // in seconds
        public DateTime DeadlineTime { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }

        private List<Question> _questions = new List<Question>();
        public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();
        public void AddQuestion(Question question)
        {
            if (!Questions.Any(i => i.Id == question.Id))
            {
                _questions.Add(question);
                return;
            }
        }
        public void ClearQuestions()
        {
            _questions.Clear();
            return;
        }
        public void SheetQuestions(List<Question> questionList)
        {
            if (questionList == null)
                return;
            _questions = questionList;
            return;
        }
        public void AddQuestions(List<Question> questions)
        {
            _questions.AddRange(questions);
            return;
        }
        public void ClearQuestions(string sheetId)
        {
            if (!Questions.Any(i => i.SheetId == sheetId))
            {
                _questions.Clear();
            }
        }
    }
}
