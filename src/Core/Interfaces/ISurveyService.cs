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
        Task<UserSurvey> ReviseSurveyAsync(SurveyInvitationDto invitationDto);
        Task<UserSurvey> GetSurveyAsync(string surveyGuid);
        int GetLatestVersion(string surveyGuid);
        Task<int> GetLatestVersion(int surveyId);
        string GetSurveyGuid(int surveyId);
        int GetSurveyId(string guid);
        List<int> RevisionList(string guid);
        Task<IEnumerable<UserSurvey>> GetSurveyListAsync(string sheetId);
        Task UpdateStatus(int surveyId, SurveyStatusEnum surveyStatus);
    }
}
