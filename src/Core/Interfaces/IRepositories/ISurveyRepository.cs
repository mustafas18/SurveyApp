using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IRepositories
{
    public interface  ISurveyRepository
    {
        Task<IEnumerable<UserSurvey>> GetSurveyListAsync(string sheetId);
        Task<IEnumerable<UserSurvey>> GetUserSurveyListAsync(string userName);
        Task<IEnumerable<UserAnswer>> GetUserAnswersAsync(int surveyId);
        Task<SurveysCount> CountSurveys();
        int GetSurveyId(string guid);
        Task<UserSurvey> LatestSurvey(string guid);
        Task<UserSurvey> SubmitSurvey(UserSurvey survey);
    }
}
