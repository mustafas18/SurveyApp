using Core.Enums;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UserSurvey : BaseEntity
    {
        public string SheetId { get; set; }
        public int SheetVersion { get; set; }
        public string? SurveyTitle { get; set; }
        public string? UserName { get; set; }
        public string Guid { get; set; }
        public string? Link { get; set; }
        public DateTime? ParticipateTime { get; set; }
        public DateTime? CreatedTime { get; set; }
        public SurveyStatusEnum? Status { get; set; }
    }

}

