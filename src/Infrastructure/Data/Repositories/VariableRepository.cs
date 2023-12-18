using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;

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

        public async Task<IEnumerable<Variable>> GetBySheetId(string sheetId, int? sheetVersion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _sheetId = sheetId, _sheetVersion = sheetVersion });

            string versionCondition = string.Empty;

  
            if (sheetVersion == null)
            {
                versionCondition = $" SheetVersion=(SELECT MAX(Version) FROM Sheets WHERE SheetId=@_sheetId) AND ";
            }

            var query = @$"SELECT [Id]
                      ,[Name]
                      ,[Type]
                      ,[Label]
                      ,[MaxValue]
                      ,[Messure]
                      ,[SheetId]
                      ,[SheetVersion]
                      ,[Deleted]
                FROM Variables 
                WHERE {versionCondition} Deleted=0 AND SheetId=@_sheetId";
            using (var connection = _db.CreateConnection())
            {
                var variable = await connection.QueryAsync<Variable>(query,parameters);
                return variable;
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
        public IEnumerable<VariableViewDto> VariableAnswers(string sheetId, int? sheetVersion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _sheetId = sheetId,_sheetVersion=sheetVersion ?? 1});

            var query = $@"
                SELECT A.[VariableId]
                    ,A.[InputValue]
                    ,(SELECT TOP(1) Label FROM VariableValueLabel WHERE VariableId=A.VariableId AND [Value]=A.InputValue) AS AnswerLabel
                    ,COUNT(1) AS [AnswerCount]
                FROM [UserAnswers]  AS A
                WHERE A.SheetId=(SELECT TOP (1) Id FROM dbo.Sheets WHERE SheetId=@_sheetId AND Version=@_sheetVersion)
                GROUP BY A.[VariableId],A.[InputValue]";

            using (var connection = _db.CreateConnection())
            {
                var variable = connection.Query<VariableViewDto>(query, parameters);
                return variable;
            }

        }
        
        }
}
