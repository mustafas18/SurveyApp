using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly IRepository<UserSurvey> _surveyRepository;
        private readonly IUserCategoryRepository _userCategoryRepository;
        private readonly ISurveyRepository _surveyDataAccess;
        private readonly ISheetService _sheetService;

        public SurveyService(IRepository<UserSurvey> surveyRepository,
            IUserCategoryRepository userCategoryRepository,
            ISurveyRepository surveyDataAccess,
            ISheetService sheetService)
        {
            _surveyRepository = surveyRepository;
            _userCategoryRepository = userCategoryRepository;
            _surveyDataAccess = surveyDataAccess;
            _sheetService = sheetService;
        }

        public async Task<UserSurvey> CreateSurveyAsync(SurveyInvitationDto invitationDto, bool isTemplate = false)
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
                CategoryId = invitationDto.categoryId,
                CreatedTime = DateTime.Now,
                DeadLine = sheet?.DeadlineTime,
                UserName = invitationDto.userName,
                IsTemplate = isTemplate,
                Status = SurveyStatusEnum.Pending,
                InviteType = invitationDto.inviteType == "Category" ? InviteTypeEnum.Category : InviteTypeEnum.Participant
            };
            if(survey.InviteType== InviteTypeEnum.Participant)
            {
                await _surveyRepository.AddAsync(survey);
            }
            else
            {
                List<UserSurvey> userSurveys = new();
                var users= await _userCategoryRepository.GetCategoryUsers(invitationDto.categoryId ?? 0);
               foreach(var user in users)
                {
                    userSurveys.Add(new UserSurvey
                    {
                        UserName= user.UserName,
                        Guid = Guid.NewGuid().ToString(),
                        SheetId = survey.SheetId,
                        SheetVersion = survey.SheetVersion,
                        SurveyTitle =survey.SurveyTitle,
                        Version = survey.Version,
                        CategoryId = survey.CategoryId,
                        CreatedTime = DateTime.Now,
                        DeadLine = survey.DeadLine,
                        IsTemplate = survey.IsTemplate,
                        Status = SurveyStatusEnum.Pending,
                        InviteType = InviteTypeEnum.Category
                    });
                }
                await _surveyRepository.AddRangeAsync(userSurveys);
            }

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
                Version = survey.Version + 1,

            };
            await _surveyDataAccess.SubmitSurvey(newSurvey);
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
                                .Where(s => s.Guid == guid && s.IsTemplate == false)
                                .Select(s => s.Id)
                                //.Skip(1)
                                .ToList();
            return result;
        }

        public async Task<IEnumerable<UserSurvey>> GetUserSurveysAsync(string userName)
        {
            var result = await _surveyDataAccess.GetUserSurveyListAsync(userName);
            return result;
        }

        public async Task<SurveysCount> CountSurveys()
        {
            var result = await _surveyDataAccess.CountSurveys();
            return result;
        }
    }
}
