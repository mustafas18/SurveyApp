using Ardalis.GuardClauses;
using Core.Interfaces;
using Core.Interfaces.IRepositories;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Core.Dtos;
using Core.IntegrationEvents.Events;
using MediatR;

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
            sheet.SheetId = Guid.NewGuid().ToString().Substring(1,7);
            await _sheetRepository.AddAsync(sheet);

            var domainEvent = new SheetAddedEvent(sheet.SheetId);
            await _mediator.Publish(domainEvent);

            return sheet;
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
          return await _sheetReadRepository.GetSheetById(sheetId);
        }


    }
}
