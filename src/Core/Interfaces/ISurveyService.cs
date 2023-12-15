using Core.Entities;
using Core.Enums;
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
        Task<List<UserSurvey>> GetSurveyListAsync(string sheetId);
        Task UpdateStatus(int surveyId, SurveyStatusEnum surveyStatus);
    }
}
