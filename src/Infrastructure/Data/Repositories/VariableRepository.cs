using Domain.Entities;
using Domain.Interfaces.IRepositories;
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
                versionCondition = $" SheetVersion=(SELECT MAX(Version) FROM Sheets WHERE SheetId='{sheetId}') AND ";
            }

            var query = @$"SELECT * 
                FROM Variables 
                WHERE {versionCondition} Deleted=0 AND SheetId='{sheetId}'";
            using (var connection = _db.CreateConnection())
            {
                var variable = await connection.QueryAsync<Variable>(query);
                return variable.ToList();
            }
        }
        public async Task<bool> DeleteAsync(int variableId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { varId = variableId });

            var query = $@"UPDATE Variables SET Deleted=1 WHERE Id=@varId";

            using (var connection = _db.CreateConnection())
            {
                var variable = await connection.ExecuteAsync(query, parameters);
            }
            return true;
        }
    }
}
