using Ardalis.GuardClauses;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Domain.Dtos;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class SheetService : ISheetService
    {
        private readonly IRepository<Sheet> _sheetRepository;
        private readonly ISheetRepository _sheetReadRepository;
        private readonly IMediator _mediator;

        public SheetService(IRepository<Sheet> sheetRepository,
                ISheetRepository sheetReadRepository,
                IMediator mediator)
        {
            _sheetRepository = sheetRepository;
            _sheetReadRepository = sheetReadRepository;
            _mediator = mediator;
        }
        public async Task<Sheet> CreateAsync(Sheet sheet)
        {
            Guard.Against.Null(sheet);
            sheet.CreateTime = DateTime.Now;
            sheet.Version = 1;
            sheet.SheetId = Guid.NewGuid().ToString().Substring(1, 7);
            await _sheetRepository.AddAsync(sheet);

            var domainEvent = new SheetAddedEvent(sheet.SheetId);
            await _mediator.Publish(domainEvent);

            return sheet;
        }
        public async Task<Sheet> UpdateAsync(Sheet sheet)
        {
            Guard.Against.Null(sheet);
            if (sheet.Version == 0)
            {
                sheet.Version = _sheetReadRepository.GetLatestVersion(sheet.SheetId);
            }
            var result = _sheetRepository.Where(s => s.SheetId == sheet.SheetId && s.Version == sheet.Version)
                                .FirstOrDefault();
            result.UpdateSheetDetail(new SheetDetail
            {
                SheetId = sheet.SheetId,
                Version = sheet.Version,
                Link = sheet.Link,
                EndPageId = sheet.EndPageId,
                WelcomePageId = sheet.WelcomePageId,
                LanguageId = sheet.LanguageId,
                TemplateId = sheet.TemplateId,
                Icon = sheet.Icon,
                Title = sheet.Title,
                CreatedByUserId = sheet.CreatedByUserId,
                DeadlineString = sheet.DeadlineString,
                DeadlineTime = sheet.DeadlineTime,
                DurationTime = sheet.DurationTime,
            });
            await _sheetRepository.UpdateAsync(result);

            var domainEvent = new SheetAddedEvent(sheet.SheetId);
            await _mediator.Publish(domainEvent);

            return result;
        }
        public async Task<Sheet> AddQuestionToSheet(string sheetId, Question question)
        {
            var sheet = _sheetRepository.FirstOrDefault(q => q.SheetId == sheetId);
            if (sheet == null)
            {
                throw new Exception("The sheet does not exist.");
            }
            sheet.AddQuestion(question);

            await _sheetRepository.UpdateAsync(sheet);
            return sheet;

        }

        public async Task<SheetDto> GetByIdAsync(string sheetId)
        {
            var sheet = _sheetReadRepository.GetSheetById(sheetId);
            var sheetDto = new SheetDto
            {
                LanguageId = sheet.LanguageId,
                Icon = sheet.Icon,
                EndPageId = sheet.EndPageId,
                DurationTime = sheet.DurationTime,
                DeadlineTime = sheet.DeadlineTime,
                CreateTime = sheet.CreateTime,
                Link = sheet.Link,
                Questions = sheet.Questions?.Where(q => q.Deleted == false).OrderBy(s => s.Order).ToList(),
                SheetId = sheet.SheetId,
                TemplateId = sheet.TemplateId,
                Title = sheet.Title,
                WelcomePageId = sheet.WelcomePageId

            };
            return sheetDto;

        }
        public async Task<SheetDto> GetSheetInfo(string sheetId, int? sheetVersion)
        {
            var sheet = await _sheetReadRepository.GetSheetInfo(sheetId, sheetVersion);
            var users = sheet.Users.FirstOrDefault();
            var sheetDto = new SheetDto
            {
                LanguageId = sheet.LanguageId,
                Icon = sheet.Icon,
                EndPageId = sheet.EndPageId,
                DurationTime = sheet.DurationTime,
                DeadlineTime = sheet.DeadlineTime,
                CreateTime = sheet.CreateTime,
                Link = sheet.Link,
                UserName = users?.UserName,
                Questions = null,
                SheetId = sheet.SheetId,
                TemplateId = sheet.TemplateId,
                Title = sheet.Title,
                WelcomePageId = sheet.WelcomePageId

            };
            return sheetDto;
        }

        public int GetLatestVersion(string sheetId)
        {
            return _sheetReadRepository.GetLatestVersion(sheetId);
        }
    }
}
