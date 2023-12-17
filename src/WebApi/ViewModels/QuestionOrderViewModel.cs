using Domain.Dtos;

namespace WebApi.ViewModels
{
    public class QuestionOrderViewModel
    {
        public string SheetId { get; set; }
        public List<QuestionOrderQDto> Questions { get; set; }
    }
}
