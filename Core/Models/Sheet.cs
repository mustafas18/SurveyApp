using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Sheet:BaseEntity,IAggregateRoot
    {
        public string SheetId { get; set; }
        public int Version { get; set; }
        public int TemplateId { get; set; }
        public string UserId { get; set; }
        public int LanguageId { get; set; }
        public int WelcomePageId { get; set; }
        public int EndPageId { get; set; }
        public string Link { get; set; }
        public int DurationTime { get; set; } // in seconds
        public DateTime DeadlineTime { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }
    }
}
