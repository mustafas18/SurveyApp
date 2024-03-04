using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class SurveysCount
    {
        public int ActiveSurveyCount { get; set; }
        public int TotalSurveyCount { get; set; }
        public int RevisedCount { get; set; }
    }
}
