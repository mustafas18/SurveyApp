using Domain.Entities;

namespace WebApi.ViewModels
{
    public class SheetViewModel
    {
        public string SheetId { get; set; }
        public int Version { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public int TemplateId { get; set; }
        public int LanguageId { get; set; }
        public int WelcomePageId { get; set; }
        public int EndPageId { get; set; }
        public string Link { get; set; }
        public List<Question>? Questions { get; set; }
        public int DurationTime { get; set; } // in seconds
        public DateTime DeadlineTime { get; set; }
        public string? CreatedByUserId { get; set; }
        public bool Deleted { get; set; }

        
    }
}
