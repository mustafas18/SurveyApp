using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISheetService
    {
        Task<Sheet> CreateAsync(Sheet sheet);
        Task<SheetDto> GetByIdAsync(string sheetId);
        Task<Sheet> AddQuestionToSheet(string sheetId, Question question);
        Task<SheetDto> GetSheetInfo(string sheetId, int? sheetVersion);
        int GetLatestVersion(string sheetId);
    }
}
