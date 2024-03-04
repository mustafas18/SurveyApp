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
        Task<UserSurvey> CreateSurveyAsync(SurveyInvitationDto invitationDto, bool isTemplate);
        Task<UserSurvey> ReviseSurveyAsync(SurveyInvitationDto invitationDto);
        Task<UserSurvey> GetSurveyAsync(string surveyGuid);
        Task<SurveysCount> CountSurveys();
        Task<IEnumerable<UserSurvey>> GetUserSurveysAsync(string userName);
        
        int GetLatestVersion(string surveyGuid);
        Task<int> GetLatestVersion(int surveyId);
        string GetSurveyGuid(int surveyId);
        int GetSurveyId(string guid);
        List<int> RevisionList(string guid);
        Task<IEnumerable<UserSurvey>> GetSurveyListAsync(string sheetId);
        Task UpdateStatus(int surveyId, SurveyStatusEnum surveyStatus);
    }
}
