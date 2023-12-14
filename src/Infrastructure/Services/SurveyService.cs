using Core.Dtos;
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
        private readonly ISheetService _sheetService;

        public SurveyService(IRepository<UserSurvey> surveyRepository,
            ISheetService sheetService)
        {
            _surveyRepository = surveyRepository;
            _sheetService = sheetService;
        }

        public async Task<UserSurvey> CreateSurveyAsync(string sheetId, string? userName)
        {
           int sheetVersion = _sheetService.GetLatestVersion(sheetId);
           SheetDto sheet= await _sheetService.GetSheetInfo(sheetId, sheetVersion);

            var survey = new UserSurvey
            {
                SheetId = sheetId,
                SheetVersion = _sheetService.GetLatestVersion(sheetId),
                SurveyTitle= sheet?.Title,
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
