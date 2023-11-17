using Core.Entities;
using Core.Interfaces.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class VariableRepository : IVariableRepository
    {
        private readonly DapperContext _db;
        public VariableRepository(DapperContext db)
        {
            _db = db;
        }
        public async Task<Variable> GetByName(string sheetId,string name)
        {
            var query = @$"SELECT * 
FROM Variables 
WHERE Name=@name AND SheetVersion=(SELECT MAX(Version) FROM Sheets WHERE SheetId={sheetId}";
            using (var connection = _db.CreateConnection())
            {
                var variable = await connection.QueryFirstAsync<Variable>(query);
                return variable;
            }
        }

        public async Task<List<Variable>> GetBySheetId(string sheetId, int? sheetVersion)
        {
            string versionCondition = string.Empty;
            if (sheetVersion == null)
            {
                versionCondition = " AND SheetVersion=(SELECT MAX(Version)";
            }
            var query = @$"SELECT * 
FROM Variables 
WHERE Name=@name AND {versionCondition} FROM Sheets WHERE SheetId={sheetId}";
            using (var connection = _db.CreateConnection())
            {
                var variable = await connection.QueryFirstAsync<List<Variable>>(query);
                return variable;
            }
        }
    }
}
