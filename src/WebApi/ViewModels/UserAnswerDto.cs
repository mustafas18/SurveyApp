namespace WebApi.ViewModels
{
    public class UserAnswerDto
    {
        public int QuestionId { get; set; }
        public int? VariableId { get; set; }
        public string? InputValue { get; set; }
       // public int? UserResponseTime { get; set; }
    }
}
