using Core.Interfaces.IRepositories;
using Core.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class SheetRepository : ISheetRepository
    {
        private readonly DapperContext _db;
        private readonly AppDbContext _efContext;

        public SheetRepository(DapperContext db,, AppDbContext efContext)
        {
            _db = db;
            _efContext = efContext;
        }
        public async Task<Sheet> AddQuestion(string sheetId, Question question)
        {
            var sheet = _efContext.Sheets.Where(s => s.SheetId == sheetId && s.Version == GetLatestVersion(sheetId))
                .Include(nameof(Sheet.Questions))
                .FirstOrDefault();
            sheet.AddQuestion(question);
            _efContext.Update(sheet);
            await _efContext.SaveChangesAsync();
            return sheet;
        }
        public async Task<IEnumerable<SheetDto>> GetSheetList()
        {
            var query = @"
            SELECT 
                S.SheetId,S.Title,S.LanguageId,STRING_AGG(CONCAT(UI.FirstName, ',', UI.LastName), ', ') AS UserFullName,S.Link,S.DurationTime,S.DeadlineTime,S.CreateTime,S.Deleted
            FROM Sheets AS S
            LEFT JOIN SheetUserInfo AS SU ON S.Id=SU.SheetsId
            LEFT JOIN UserInfos AS UI ON UI.Id=SU.UsersId
            GROUP BY S.SheetId,S.Title,S.LanguageId,S.Link,S.DurationTime,S.DeadlineTime,S.CreateTime,S.Deleted";
            //            var query = @"
            //SELECT 
            //    SheetId,LanguageId,Link,DurationTime,DeadlineTime,CreateTime,Deleted
            //FROM Sheets";


            using (var connection = _db.CreateConnection())
            {
                var sheets = await connection.QueryAsync<SheetDto>(query);
                return sheets.ToList();
            }
        }
        public async Task<SheetDto> GetSheetById(string sheetId)
        {
            var query = @"
SELECT 
    SheetId,Title,Icon,TemplateId,UserId,LanguageId,WelcomePageId,EndPageId,Link,DurationTime,DeadlineTime,CreateTime
FROM Sheets
WHERE SheetId=@SheetId AND Version=(SELECT MAX(Version) FROM Sheets WHERE SheetId=@SheetId AND Deleted=0)";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("SheetId", sheetId);
            using (var connection = _db.CreateConnection())
            {
                var sheet = await connection.QueryFirstOrDefaultAsync<SheetDto>(query, dynamicParameters);
                return sheet;
            }
        }
        public int GetLatestVersion(string sheetId)
        {
            var query = @"
SELECT 
    MAX(Version)
FROM Sheets
WHERE SheetId=@SheetId AND Deleted=0";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("SheetId", sheetId);
            using (var connection = _db.CreateConnection())
            {
                var result = connection.QueryFirstOrDefault<int>(query, dynamicParameters);
                if (result == 0 || result == null)
                {
                    return 1;
                }
                return result;
            }
        }

    }
}
