using Dapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Data.Repositories
{
    public class SurveyRepository: ISurveyRepository
    {
        private readonly DapperContext _db;
        private readonly AppDbContext _efContext;

        public SurveyRepository(DapperContext db, AppDbContext efContext)
        {
            _db = db;
            _efContext = efContext;
        }

        public int GetSurveyId(string guid)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _guid = guid });
            var query = $@"SELECT Id FROM UserSurveys AS S
                WHERE S.Guid=@_Guid AND Version=(SELECT MAX(Version) FROM UserSurveys WHERE Guid=S.Guid)";
            using (var connection = _db.CreateConnection())
            {
                var result = connection.ExecuteScalar<int>(query, parameters);
                return result;
            }
        }

        public async Task<IEnumerable<UserSurvey>> GetSurveyListAsync(string sheetId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _sheetId = sheetId});
            var query = $@"SELECT * FROM UserSurveys AS S
                WHERE S.SheetId=@_sheetId AND S.Version=(SELECT MAX(Version) FROM UserSurveys WHERE Guid=S.Guid)";
            using (var connection = _db.CreateConnection())
            {
                var surveys = await connection.QueryAsync<UserSurvey>(query, parameters);
                return surveys;
            }
        }
        public async Task<IEnumerable<UserAnswer>> GetUserAnswersAsync(int surveyId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _previousSurveyId = surveyId -1 });
            var query = $@"SELECT * FROM dbo.UserAnswers AS U
                WHERE U.SurveyId = @_previousSurveyId";
            using (var connection = _db.CreateConnection())
            {
                var result = await connection.QueryAsync<UserAnswer>(query, parameters);
                return result;
            }
        }

        public async Task<UserSurvey> LatestSurvey(string guid)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _guid = guid });
            var query = $@"SELECT * FROM UserSurveys AS S WHERE S.Guid=@_guid AND S.Version=(SELECT MAX(Version) FROM dbo.UserSurveys
                WHERE Guid = @_guid)";
            using (var connection = _db.CreateConnection())
            {
                var result = await connection.QueryFirstAsync<UserSurvey>(query, parameters);
                return result;
            }
        }
    }
}
