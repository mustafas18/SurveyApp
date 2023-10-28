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

namespace Infrastructure.Services
{
    public class SheetService : ISheetService
    {
        private readonly IEfRepository<Sheet> _sheetRepository;

        public SheetService(IEfRepository<Sheet> sheetRepository)
        {
            _sheetRepository = sheetRepository;
        }
        public async Task<Sheet> CreateSheetAsync(Sheet sheet)
        {
            Guard.Against.Null(sheet);
            sheet.CreateTime = DateTime.Now;
            sheet.Version = 1;
            sheet.SheetId = new Guid().ToString();
            await _sheetRepository.AddAsync(sheet);
            return sheet;
        }
        public async Task<Sheet> AddQuestionToSheet(string sheetId, Question question)
        {
            var sheet = await _sheetRepository.FirstOrDefaultAsync(q => q.SheetId == sheetId);
            if (sheet == null)
            {
                throw new Exception("The sheet does not exist.");
            }
            sheet.AddItem(sheetId, question);

            await _sheetRepository.UpdateAsync(sheet);
            return sheet;

        }
    }
}
