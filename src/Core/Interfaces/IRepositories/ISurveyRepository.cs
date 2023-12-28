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
        Task<IEnumerable<UserAnswer>> GetUserAnswersAsync(int surveyId);
        int GetSurveyId(string guid);
        int LatestVersion(string guid);
    }
}
