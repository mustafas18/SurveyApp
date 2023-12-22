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
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DapperContext _db;
        private readonly AppDbContext _efContext;

        public QuestionRepository(DapperContext db, AppDbContext efContext)
        {
            _db = db;
            _efContext = efContext;
        }
        public async Task<(string SheetId, int Version)> DeleteAsync(int Id)
        {
            (string,int) sheetId = ("",1);
            var query = $@"
                UPDATE Questions SET Deleted=1 WHERE Id={Id}
                SELECT SheetId1 AS SheetId,SheetVersion FROM Questions WHERE Id={Id}";
            using (var connection = _db.CreateConnection())
            {
                sheetId = await connection.QueryFirstAsync<(string SheetId,int SheetVersion)>(query);
            }
            return sheetId;
        }
        public IEnumerable<QuestionWithAnswerDto> QuestionWithAnswers(string sheetId,int? sheetVersion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _sheetId = sheetId, _sheetVersion = sheetVersion ?? 1 });

            var query = $@"
               SELECT Q.Id AS [QuestionId]
	                  ,QA.Id AS AnswerId
	                  ,QA.Text AS [AnswerText]
                FROM [dbo].[Questions] AS Q
                LEFT JOIN dbo.QuestionAnswers AS QA
                ON Q.Id = QA.QuestionId
                WHERE Q.SheetId=@_sheetId AND Q.SheetVersion=@_sheetVersion AND Q.Deleted=0";

            using (var connection = _db.CreateConnection())
            {
                var answers = connection.Query<QuestionWithAnswerDto>(query, parameters);
                return answers;
            }
        }

    }

}
