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

namespace Infrastructure.Services
{
    public class SheetService : ISheetService
    {
        private readonly IRepository<Sheet> _sheetRepository;
        private readonly ISheetRepository _sheetReadRepository;

        public SheetService(IRepository<Sheet> sheetRepository,
                ISheetRepository sheetReadRepository)
        {
            _sheetRepository = sheetRepository;
            _sheetReadRepository = sheetReadRepository;
        }
        public async Task<Sheet> CreateAsync(Sheet sheet)
        {
            Guard.Against.Null(sheet);
            sheet.CreateTime = DateTime.Now;
            sheet.Version = 1;
            sheet.SheetId = Guid.NewGuid().ToString();
            await _sheetRepository.AddAsync(sheet);
            return sheet;
        }
        public async Task<Sheet> AddQuestionToSheet(string sheetId, Question question)
        {
            var sheet = _sheetRepository.FirstOrDefault(q => q.SheetId == sheetId);
            if (sheet == null)
            {
                throw new Exception("The sheet does not exist.");
            }
            sheet.AddItem(sheetId, question);

            await _sheetRepository.UpdateAsync(sheet);
            return sheet;

        }

        public async Task<SheetDto> GetByIdAsync(string sheetId)
        {
          return await _sheetReadRepository.GetSheetById(sheetId);
        }

        public Task<SheetDto> GetByIdWithQuestionsAsync(string sheetId)
        {
            throw new NotImplementedException();
        }
    }
}
