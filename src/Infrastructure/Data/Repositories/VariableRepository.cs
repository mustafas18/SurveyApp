using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;
using System.Data;

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
        public async Task<IEnumerable<VariableWithValuesDto>> GetVariableWithValues(string sheetId, int? sheetVersion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _sheetId = sheetId, _sheetVersion = sheetVersion });

            string versionCondition = " V.SheetVersion=1";


            if (sheetVersion == null)
            {
                versionCondition = $" V.SheetVersion=(SELECT MAX(Version) FROM Sheets WHERE SheetId=@_sheetId) AND ";
            }

            var query = @$"SELECT 
                        V.Id
                        ,V.Name,V.Messure,V.Label AS VariableLabel
                        ,VL.Label AS ValueLabel
                        ,VL.Value
                    FROM dbo.Variables AS V
	                LEFT JOIN dbo.VariableValueLabel AS VL ON VL.VariableId=V.Id
	                WHERE {versionCondition} V.Deleted=0 AND V.SheetId=@_sheetId
	                GROUP BY VL.Label,V.Id,VL.Value,V.Name,V.Messure,V.Label";
            using (var connection = _db.CreateConnection())
            {
                var variable = await connection.QueryAsync<VariableWithValuesDto>(query, parameters);
                return variable;
            }
        }
        public async Task<IEnumerable<VariableValueLabel>> GetVariableValues(int variableId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _variableId = variableId });

            var query = @$"SELECT * FROM dbo.VariableValueLabel 
                            WHERE VariableId=@_variableId";
            using (var connection = _db.CreateConnection())
            {
                var variableValues = await connection.QueryAsync<VariableValueLabel>(query, parameters);
                return variableValues;
            }
        }
        public async Task<IEnumerable<Variable>> GetBySheetId(string sheetId, int? sheetVersion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { SheetId = sheetId, SheetVersion = sheetVersion });

            using (var connection = _db.CreateConnection())
            {
                var variable = await connection.QueryAsync<Variable>("sp_GetSheetVariables", parameters, commandType: CommandType.StoredProcedure);
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
        public IEnumerable<VariableAnswerDto> VariableAnswers(string sheetId, int? sheetVersion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { SheetId = sheetId,SheetVersion=sheetVersion ?? 1});

            var query = $@"sp_GetVariableAnswers";

            using (var connection = _db.CreateConnection())
            {
                var variable = connection.Query<VariableAnswerDto>(query, parameters,commandType: CommandType.StoredProcedure);
                return variable;
            }

        }
        
        }
}
