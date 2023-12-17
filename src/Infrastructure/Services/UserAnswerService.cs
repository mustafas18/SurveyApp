using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly IRepository<UserAnswer> _userAnswerRepository;

        public UserAnswerService(IRepository<UserAnswer> userAnswerRepository)
        {
            _userAnswerRepository = userAnswerRepository;
        }
        public async Task Create(List<UserAnswer> answers)
        {
           await _userAnswerRepository.AddRangeAsync(answers);
        }

        public async Task<List<UserAnswer>> GetBySurveyId(int surveyId)
        {
            return await _userAnswerRepository
                            .AsNoTracking()
                            .Where(s => s.SurveyId == surveyId)
                            .ToListAsync();
        }
    }
}
