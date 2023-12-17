using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class SurveyInvitationDto
    {
        public string sheetId { get; set; }
        public string userName { get; set; }
        public string? guid { get; set; }
    }
}
