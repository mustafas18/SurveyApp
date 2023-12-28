using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISurveyService
    {
        Task<UserSurvey> CreateSurveyAsync(SurveyInvitationDto invitationDto);
        Task<UserSurvey> GetSurveyAsync(string surveyGuid);
        int GetLatestVersion(string surveyGuid);
        int GetLatestVersion(int surveyId);
        string GetSurveyGuid(int surveyId);
        Task<List<UserSurvey>> GetSurveyListAsync(string sheetId);
        Task UpdateStatus(int surveyId, SurveyStatusEnum surveyStatus);
    }
}
