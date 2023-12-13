using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserAnswerService
    {
        Task Create(List<UserAnswer> answers);
        Task<List<UserAnswer>> GetBySurveyId(int surveyId);
    }
}
