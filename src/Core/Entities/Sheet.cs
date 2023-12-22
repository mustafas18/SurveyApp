using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// This class is a non-anemic model, which business concerns are now handled in the domain entites. 
    /// Like other models, it does not follow S principle in SOLID, since it does not do its responsibility well.
    /// </summary>
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
        public string? Link { get; set; }
        public int DurationTime { get; set; } // in seconds
        public DateTime DeadlineTime { get; set; }
        [NotMapped]
        public string DeadlineString { get; set; }
        public string? CreatedByUserId { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }

        public List<Question>? Questions { get; set; }
        public void AddQuestion(Question question)
        {
            if (!Questions.Any(i => i.Id == question.Id))
            {
                Questions.Add(question);
                return;
            }
        }
        public void ClearQuestions()
        {
            Questions.Clear();
            return;
        }
        public void SheetQuestions(List<Question> questionList)
        {
            if (questionList == null)
                return;
            Questions = questionList;
            return;
        }
        public void AddQuestions(List<Question> questions)
        {
            Questions.AddRange(questions);
            return;
        }
        public void ClearQuestions(string sheetId)
        {
            if (!Questions.Any(i => i.SheetId == sheetId))
            {
                Questions.Clear();
            }
        }
    }
}
