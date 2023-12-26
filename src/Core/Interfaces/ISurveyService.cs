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
        Task<UserSurvey> GetSurveyAsync(int surveyId);
        Task<UserSurvey> GetSurveyAsync(string surveyGuid);
        Task<List<UserSurvey>> GetSurveyListAsync(string sheetId);
        Task UpdateStatus(int surveyId, SurveyStatusEnum surveyStatus);
    }
}
