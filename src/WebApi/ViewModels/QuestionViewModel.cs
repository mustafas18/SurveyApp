using Core.Entities;
using Core.Enums;

namespace WebApi.ViewModels
{
    public class QuestionViewModel
    {
        public string SheetId { get; set; }
        public QuestionTypeEnum Type { get; set; }
        public string UserId { get; set; }
        public string QuestionText { get; set; }
        public int VariableId { get; set; }
        public List<QuestionAnswer> QuestionAnswers { get; set; }
        public int ResponseTime { get; set; }
        public bool Required { get; set; }
    }
}
