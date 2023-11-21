namespace WebApi.ViewModels
{
    public class QuestionOrderViewModel
    {
        public string SheetId;
        public QuestionOrderQViewModel Questions;
    }
    public class QuestionOrderQViewModel
    {
        public int id { get; set; }
        public int order { get; set; }
    }
}
