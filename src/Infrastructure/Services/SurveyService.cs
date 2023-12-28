using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
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

        public async Task<UserSurvey> CreateSurveyAsync(SurveyInvitationDto invitationDto)
        {
            int sheetVersion = _sheetService.GetLatestVersion(invitationDto.sheetId);
            SheetDto sheet = await _sheetService.GetSheetInfo(invitationDto.sheetId, sheetVersion);

            var survey = new UserSurvey
            {
                Guid = Guid.NewGuid().ToString(),
                SheetId = invitationDto.sheetId,
                SheetVersion = sheetVersion,
                SurveyTitle = sheet?.Title,
                Version = 1,
                DeadLine = sheet?.DeadlineTime,
                UserName = invitationDto.userName,
                Status = SurveyStatusEnum.Pending
            };
            await _surveyRepository.AddAsync(survey);
            return survey;
        }

        public int GetLatestVersion(string surveyGuid)
        {
            int? result = _surveyRepository.AsNoTracking().Where(s => s.Guid == surveyGuid)?.Max(s => s.Version);
            return result ?? 0;
        }
        public string GetSurveyGuid(int surveyId)
        {
            var surveyGuid = _surveyRepository
                                .AsNoTracking()
                                .Where(s => s.Id == surveyId)
                                .Select(s => s.Guid)
                                .FirstOrDefault();
            return surveyGuid;
        }

        public async Task<UserSurvey> GetSurveyAsync(string surveyGuid)
        {
            var latestVersion=GetLatestVersion(surveyGuid);
            return await _surveyRepository.FirstOrDefaultAsync(s => s.Guid == surveyGuid && s.Version == latestVersion);
        }

        public async Task<List<UserSurvey>> GetSurveyListAsync(string sheetId)
        {
            return await _surveyRepository.AsNoTracking().Where(s=>s.SheetId==sheetId).ToListAsync();
        }

        public async Task UpdateStatus(int surveyId, SurveyStatusEnum surveyStatus)
        {
            var survey = await _surveyRepository.FirstOrDefaultAsync(s => s.Id == surveyId && s.Version == GetLatestVersion(surveyId));
            survey.Status = surveyStatus;
            survey.ParticipateTime= DateTime.Now;
            await _surveyRepository.UpdateAsync(survey);
        }

        public int GetLatestVersion(int surveyId)
        {
            var surveyVersion = _surveyRepository
                     .AsNoTracking()
                     .Where(s => s.Id == surveyId && s.Version==_surveyRepository.AsNoTracking().Where(s2=>s2.Id==surveyId).Max(s=>s.Version))
                     .Select(s => s.Version)
                     .FirstOrDefault();
            return surveyVersion;
        }
    }
}
