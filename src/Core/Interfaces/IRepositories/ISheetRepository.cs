using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IRepositories
{
    public interface ISheetRepository
    {
        Task<IEnumerable<SheetDto>> GetSheetList();
        Sheet GetSheetById(string sheetId);
        Task<Sheet?> GetSheetInfo(string sheetId, int? sheetVersion);
        Task<Sheet> AddQuestion(string sheetId, Question question);

        /// <summary>
        /// Get the latest version of the sheet
        /// </summary>
        /// <param name="sheetId"></param>
        /// <returns></returns>
        int GetLatestVersion(string sheetId);
        void SaveChanges(Sheet sheet);
    }
}
