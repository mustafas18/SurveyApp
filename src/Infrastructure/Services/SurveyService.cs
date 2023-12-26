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
                SheetId = invitationDto.sheetId,
                SheetVersion = sheetVersion,
                SurveyTitle = sheet?.Title,
                DeadLine = sheet?.DeadlineTime,
                Guid = invitationDto.guid,
                UserName = invitationDto.userName,
                Status = SurveyStatusEnum.Pending
            };
            await _surveyRepository.AddAsync(survey);
            return survey;
        }



        public async Task<UserSurvey> GetSurveyAsync(int surveyId)
        {
            return await _surveyRepository.FirstOrDefaultAsync(s => s.Id == surveyId);
        }
        public async Task<UserSurvey> GetSurveyAsync(string surveyGuid)
        {
            return await _surveyRepository.FirstOrDefaultAsync(s => s.Guid == surveyGuid);
        }

        public async Task<List<UserSurvey>> GetSurveyListAsync(string sheetId)
        {
            return await _surveyRepository.AsNoTracking().Where(s=>s.SheetId==sheetId).ToListAsync();
        }

        public async Task UpdateStatus(int surveyId, SurveyStatusEnum surveyStatus)
        {
            var survey = await _surveyRepository.FirstOrDefaultAsync(s => s.Id == surveyId);
            survey.Status = surveyStatus;
            survey.ParticipateTime= DateTime.Now;
            await _surveyRepository.UpdateAsync(survey);
        }
    }
}
