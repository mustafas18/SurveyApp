using Core.Dtos;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{
    public interface ISheetRepository
    {
        Task<IEnumerable<SheetDto>> GetSheetList();
        Task<SheetDto> GetSheetById(string sheetId);
        Task<Sheet> AddQuestion(string sheetId, Question question);

        /// <summary>
        /// Get the latest version of the sheet
        /// </summary>
        /// <param name="sheetId"></param>
        /// <returns></returns>
       int GetLatestVersion(string sheetId);
    }
}
