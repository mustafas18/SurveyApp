using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class SurveyInvitationDto
    {
        [JsonIgnore]
        public int? id { get; set; }
        public int? categoryId { get; set; }
        public string sheetId { get; set; }
        public string? userName { get; set; }
        public string? guid { get; set; }
        [JsonIgnore]
        public int? version { get; set; }
    }
}
