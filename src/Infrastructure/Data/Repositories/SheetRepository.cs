using Core.Interfaces.IRepositories;
using Core.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infrastructure.Data.Repositories
{
    public class SheetRepository: ISheetRepository
    {
        private readonly DapperContext _db;
        public SheetRepository(DapperContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Sheet>> GetSheetList()
        {
            var query = @"
SELECT 
    SheetId,UserId,Link,DurationTime,DeadlineTime,CreateTime,Deleted
FROM Sheets";

            using (var connection = _db.CreateConnection())
            {
                var sheets = await connection.QueryAsync<Sheet>(query);
                return sheets.ToList();
            }
        }
        public async Task<Sheet> GetSheetById(int sheetId)
        {
            var query = @"
SELECT 
    SheetId,Name,Icon,UserId,Link,DurationTime,DeadlineTime,CreateTime,Deleted
FROM Sheets
WHERE SheetId=@SheetId AND Version=(SELECT MAX(Version) FROM Sheets WHERE SheetId=@SheetId AND Deleted=0)";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("SheetId", sheetId);
            using (var connection = _db.CreateConnection())
            {
                var sheet = await connection.QueryFirstOrDefaultAsync<Sheet>(query,dynamicParameters);
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
                return result;
            }
        }
        }
}
