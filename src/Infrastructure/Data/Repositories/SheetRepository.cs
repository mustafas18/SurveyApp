using Domain.Interfaces.IRepositories;
using Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using Sheet = Domain.Entities.Sheet;

namespace Infrastructure.Data.Repositories
{
    public class SheetRepository : ISheetRepository
    {
        private readonly DapperContext _db;
        private readonly AppDbContext _efContext;

        public SheetRepository(DapperContext db, AppDbContext efContext)
        {
            _db = db;
            _efContext = efContext;
        }
        public void SaveChanges(Domain.Entities.Sheet sheet)
        {
            _efContext.Sheets.Update(sheet);
            _efContext.SaveChanges();
        }
        public async Task<Domain.Entities.Sheet> AddQuestion(string sheetId, Question question)
        {
            var sheet = _efContext.Sheets.Where(s => s.SheetId == sheetId && s.Version == GetLatestVersion(sheetId))
                .Include(nameof(Sheet.Questions))
                .FirstOrDefault();
            sheet.AddQuestion(question);
            _efContext.Update(sheet);
            await _efContext.SaveChangesAsync();
            return sheet;
        }
        public async Task<Sheet> UpdateQuestion(string sheetId, Question question)
        {
            var sheet = _efContext.Sheets
                .AsNoTracking()
                .Where(s => s.SheetId == sheetId && s.Version == GetLatestVersion(sheetId))
                .Include(nameof(Sheet.Questions))
                .FirstOrDefault();

            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _questionId = question.Id});
            var query = $@"
DELETE FROM [QuestionAnswers] WHERE QuestionId=@_questionId
";
            using (var connection = _db.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);

            }

            //  _efContext.QuestionAnswers.Where(s=>s.)
            sheet.UpdateQuestion(question);
            _efContext.Update(sheet);
            await _efContext.SaveChangesAsync();
            return sheet;
        }
        public async Task<IEnumerable<SheetDto>> GetSheetList(string username)
        {
            /*
            var query = @"
            SELECT 
                S.SheetId,S.Title,S.LanguageId,STRING_AGG(CONCAT(UI.FirstName, ',', UI.LastName), ', ') AS UserFullName,S.Link,S.DurationTime,S.DeadlineTime,S.CreateTime,S.Deleted
            FROM Sheets AS S
            LEFT JOIN SheetUserInfo AS SU ON S.Id=SU.SheetsId
            LEFT JOIN UserInfos AS UI ON UI.Id=SU.UsersId
            GROUP BY S.SheetId,S.Title,S.LanguageId,S.Link,S.DurationTime,S.DeadlineTime,S.CreateTime,S.Deleted";
            */
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _userName = username });
            var query = @"
            SELECT 
                SheetId,Title,LanguageId,Link,CreateTime,DurationTime,DeadlineTime,CreateTime,Deleted
            FROM Sheets WHERE CreatedByUserId = @_userName";


            using (var connection = _db.CreateConnection())
            {
                var sheets = await connection.QueryAsync<SheetDto>(query, parameters);
                return sheets;
            }
        }
        public Sheet GetSheetById(string sheetId)
        {
            var sheet = _efContext.Sheets.Where(s => s.SheetId == sheetId && s.Version == _efContext.Sheets.Where(s2 => s2.SheetId == sheetId).Max(s2 => s2.Version))?
                  .Include(s => s.Questions)
                  .ThenInclude(q => q.Answers)
                   .FirstOrDefault();
            return sheet;
        }
        public async Task<Sheet?> GetSheetInfo(string sheetId, int? sheetVersion)
        {
            if (sheetVersion==null)
            {
                sheetVersion = GetLatestVersion(sheetId);
            }
            var sheet =await _efContext.Sheets
                            .Where(s => s.SheetId == sheetId && s.Version == sheetVersion)
                            .Include(s => s.Users)
                            .FirstOrDefaultAsync();
            return sheet;
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
                var result = connection.QueryFirstOrDefault<int?>(query, dynamicParameters);
                return result ?? 1;
            }
        }
        public int GetLatestSheet(string sheetId)
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
                var result = connection.QueryFirstOrDefault<int?>(query, dynamicParameters);
                return result ?? 1;
            }
        }
       

    }
}
