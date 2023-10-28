using Core.Dtos;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISheetService
    {
        Task<Sheet> CreateAsync(Sheet sheet);
        Task<SheetDto> GetByIdAsync(string sheetId);
        Task<SheetDto> GetByIdWithQuestionsAsync(string sheetId);
        Task<Sheet> AddQuestionToSheet(string sheetId, Question question);
    }
}
