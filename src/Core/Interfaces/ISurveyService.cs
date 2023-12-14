using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISurveyService
    {
        Task<UserSurvey> CreateSurveyAsync(string sheetId, string? userName);
        Task<UserSurvey> GetSurveyAsync(int surveyId);
    }
}
