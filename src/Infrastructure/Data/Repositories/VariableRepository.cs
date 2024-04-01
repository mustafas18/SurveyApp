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
using System.Text.Json;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace Infrastructure.Data.Repositories
{
    public class VariableRepository : IVariableRepository
    {
        private readonly DapperContext _db;
        private readonly IConfiguration configuration;

        public VariableRepository(DapperContext db, IConfiguration configuration)
        {
            _db = db;
            this.configuration = configuration;
        }
        public async Task<Variable> GetByName(string sheetId, string name)
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
        public async Task<DataTable> GetSurveyData(string sheetId)
        {
            var _connectionString = string.Empty;
#if DEBUG
            _connectionString = this.configuration.GetConnectionString("DefaultConnectionString");
#else
            _connectionString = this.configuration.GetConnectionString("ReleaseConnectionString");
#endif
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand($"sp_GetSurveyData @SheetId='{sheetId}'", connection);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            connection.Close();
            da.Dispose();
            return dt;
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
        public async Task<IEnumerable<VariableSurveyResultDto>> SurveyVariableReportAsync(int surveyId)
        {
            var result = new List<VariableSurveyResultDto>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _surveyId = surveyId });

            var query = $@"SELECT V.Id,V.Name,V.Label,SUM(CAST(UA.InputValue AS INT)) AS [Sum] FROM [iSurveyApp].[dbo].[UserAnswers] UA
INNER JOIN dbo.Variables V
	ON V.Id=UA.VariableId
WHERE UA.SurveyId=@_surveyId AND v.Messure=0
GROUP BY V.Id,V.Name,V.Label";

            using (var connection = _db.CreateConnection())
            {
                return await connection.QueryAsync<VariableSurveyResultDto>(query, parameters);
            }
        }
        public IEnumerable<VariableAnswerDto> VariableAnswers(string sheetId, int? sheetVersion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { SheetId = sheetId, SheetVersion = sheetVersion ?? 1 });

            var query = $@"sp_GetVariableAnswers";

            using (var connection = _db.CreateConnection())
            {
                var variable = connection.Query<VariableAnswerDto>(query, parameters, commandType: CommandType.StoredProcedure);
                return variable;
            }

        }

        public async Task<IEnumerable<VariableSurveyResultDto>> GetSurveyVariableData(string surveyGuid)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _surveyGuid = surveyGuid });

            var query = $@"sp_SurveyVariableData";

            using (var connection = _db.CreateConnection())
            {
                var variable = await connection.QueryAsync<VariableSurveyResultDto>(query, parameters, commandType: CommandType.StoredProcedure);
                return variable;
            }

        }

        public async Task<IEnumerable<VariableSurveyResultDto>> UpdateVariables(List<VariableSurveyResultDto> variables, string guidId)
        {
            var result = new List<VariableSurveyResultDto>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _guidId = guidId });
            var query = new StringBuilder("ALTER TABLE [UserAnswers] NOCHECK CONSTRAINT [FK_UserAnswers_Questions_QuestionId];");
            variables.ForEach(v =>
            {
                query.AppendLine(@$"exec sp_UpdateVariable @GuidId=@_guidId, @VariableId={v.Id},@InputValue={v.Value};");
            });
            query.AppendLine(@$"ALTER TABLE [UserAnswers] CHECK CONSTRAINT [FK_UserAnswers_Questions_QuestionId];
");
            using (var connection = _db.CreateConnection())
            {
                return await connection.QueryAsync<VariableSurveyResultDto>(query.ToString(), parameters);
            }
        }

        public async Task<IEnumerable<VariableSurveyResultDto>> GetSheetVariableData(string sheetId)
        {
            List<VariableSurveyResultDto> variables = new List<VariableSurveyResultDto>();
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { SheetId = sheetId ,SheetVersion=""});

            using (var connection = _db.CreateConnection())
            {
                var variable = await connection.QueryAsync<Variable>("sp_GetSheetVariables", parameters, commandType: CommandType.StoredProcedure);
                foreach (var item in variable)
                {
                    variables.Add(new VariableSurveyResultDto
                    {
                        Id = item.Id,
                        Label = item.Label,
                        Name = item.Name,
                        Type = item.Type,
                        Sum = 0
                    });
                }
            }
            return variables;

        }
    }
}
