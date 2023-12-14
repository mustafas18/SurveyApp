using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.IRepositories;
using Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly IRepository<UserSurvey> _surveyRepository;
        private readonly ISheetRepository _sheetRepository;

        public SurveyService(IRepository<UserSurvey> surveyRepository,
            ISheetRepository sheetRepository)
        {
            _surveyRepository = surveyRepository;
            _sheetRepository = sheetRepository;
        }

        public async Task<UserSurvey> CreateSurveyAsync(string sheetId, string? userName)
        {
            var survey = new UserSurvey
            {
                SheetId = sheetId,
                SheetVersion = _sheetRepository.GetLatestVersion(sheetId),
                Link = Guid.NewGuid().ToString(),
                UserName = userName,
            };
            await _surveyRepository.AddAsync(survey);
            return survey;
        }



        public async Task<UserSurvey> GetSurveyAsync(int surveyId)
        {
            return await _surveyRepository.FirstOrDefaultAsync(s => s.Id == surveyId);
        }
    }
}
