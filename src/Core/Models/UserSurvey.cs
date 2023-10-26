using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UserSurvey : BaseEntity
    {
        public string SheetId { get; set; }
        public int SheetVersion { get; set; }
        public string Link { get; set; }
        public DateTime ParticipateTime { get; set; }
    }
}
