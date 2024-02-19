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
        private readonly ISurveyRepository _surveyDataAccess;
        private readonly ISheetService _sheetService;

        public SurveyService(IRepository<UserSurvey> surveyRepository,
            ISurveyRepository surveyDataAccess,
            ISheetService sheetService)
        {
            _surveyRepository = surveyRepository;
            _surveyDataAccess = surveyDataAccess;
            _sheetService = sheetService;
        }

        public async Task<UserSurvey> CreateSurveyAsync(SurveyInvitationDto invitationDto)
        {
            int sheetVersion = _sheetService.GetLatestVersion(invitationDto.sheetId);
            SheetDto sheet = await _sheetService.GetSheetInfo(invitationDto.sheetId, sheetVersion);

            var survey = new UserSurvey
            {
                Guid = invitationDto.guid,
                SheetId = invitationDto.sheetId,
                SheetVersion = sheetVersion,
                SurveyTitle = sheet?.Title,
                Version = 0,
                CreatedTime = DateTime.Now,
                DeadLine = sheet?.DeadlineTime,
                UserName = invitationDto.userName,
                Status = SurveyStatusEnum.Pending
            };
            await _surveyRepository.AddAsync(survey);
            return survey;
        }
        public async Task<UserSurvey> ReviseSurveyAsync(SurveyInvitationDto invitationDto)
        {
            int sheetVersion = _sheetService.GetLatestVersion(invitationDto.sheetId);
            SheetDto sheet = _sheetService.GetSheetInfo(invitationDto.sheetId, sheetVersion).Result;
            var survey = _surveyDataAccess.LatestSurvey(invitationDto.guid).Result;

            var newSurvey = new UserSurvey
            {
                CreatedTime = DateTime.Now,
                DeadLine = sheet?.DeadlineTime,
                Guid = survey.Guid,
                Link = survey?.Link,
                ParticipateTime = DateTime.Now,
                SheetId = survey.SheetId,
                SheetVersion = survey.SheetVersion,
                Status = SurveyStatusEnum.Completed,
                SurveyTitle = survey.SurveyTitle,
                UserName = survey.UserName,
                Version = survey.Version+1,

            };
            await _surveyRepository.AddAsync(newSurvey);
            return survey;
        }
        public int GetLatestVersion(string surveyGuid)
        {
            int? result = _surveyRepository.AsNoTracking()
                .Where(s => s.Guid == surveyGuid)?
                .Max(s => s.Version);
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
            var latestVersion = GetLatestVersion(surveyGuid);
            return await _surveyRepository.FirstOrDefaultAsync(s => s.Guid == surveyGuid && s.Version == latestVersion);
        }

        public async Task<IEnumerable<UserSurvey>> GetSurveyListAsync(string sheetId)
        {
            return await _surveyDataAccess.GetSurveyListAsync(sheetId);
        }

        public async Task UpdateStatus(int surveyId, SurveyStatusEnum surveyStatus)
        {
            var survey = await _surveyRepository.FirstOrDefaultAsync(s => s.Id == surveyId && s.Version == GetLatestVersion(surveyId).Result);
            survey.Status = surveyStatus;
            survey.ParticipateTime = DateTime.Now;
            await _surveyRepository.UpdateAsync(survey);
        }

        public async Task<int> GetLatestVersion(int surveyId)
        {
            var lastVersion = _surveyRepository.AsNoTracking()
                    .Where(s2 => s2.Id == surveyId)
                    .Max(s => s.Version);
            var surveyVersion = await _surveyRepository
                     .AsNoTracking()
                     .Where(s => s.Id == surveyId && s.Version == lastVersion)
                     .Select(s => s.Version)
                     .FirstOrDefaultAsync();
            return surveyVersion;
        }

        public int GetSurveyId(string guid)
        {
            return _surveyDataAccess.GetSurveyId(guid);
 
        }

        public List<int> RevisionList(string guid)
        {
            var result = _surveyRepository.AsNoTracking()
                                .Where(s => s.Guid == guid)
                                .Select(s=> s.Id)
                                //.Skip(1)
                                .ToList();
            return result;
        }
    }
}
