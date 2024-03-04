using Dapper;
using DocumentFormat.OpenXml.Presentation;
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
                WHERE S.SheetId=@_sheetId AND S.Version=(SELECT MAX(Version) FROM UserSurveys WHERE Guid=S.Guid)
                    AND IsDelete=0";
            using (var connection = _db.CreateConnection())
            {
                var surveys = await connection.QueryAsync<UserSurvey>(query, parameters);
                return surveys;
            }
        }
        public async Task<IEnumerable<UserSurvey>> GetUserSurveyListAsync(string userName)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _userName = userName });
            var query = $@"SELECT 
u.Id, u.CategoryId,(SELECT TOP(1) CreatedTime FROM dbo.UserSurveys WHERE [Guid] = u.Guid AND Version=0) AS CreatedTime, u.DeadLine, u.Guid, u.IsDelete, u.IsTemplate, u.Link, u.ParticipateTime, u.SheetId, u.SheetVersion, u.Status, u.SurveyTitle, u.UserName, u.Version
FROM UserSurveys AS u
WHERE u.UserName = @_userName AND u.IsDelete = 0 AND u.Version = (SELECT MAX(Version) FROM dbo.UserSurveys WHERE [Guid] = u.Guid) 
ORDER BY u.CreatedTime DESC";
            using (var connection = _db.CreateConnection())
            {
                var surveys = await connection.QueryAsync<UserSurvey>(query, parameters);
                return surveys;
            }
        }
        public async Task<IEnumerable<UserAnswer>> GetUserAnswersAsync(int surveyId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _previousSurveyId = surveyId  });
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

        public async Task<UserSurvey> SubmitSurvey(UserSurvey survey)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _guid = survey.Guid,

                _SurveyTitle=survey.SurveyTitle,
                _UserName = survey.UserName,
                _Link = survey.Link,
                _DeadLine = survey.DeadLine,
                _ParticipateTime = survey.CreatedTime,
                _Status = survey.Status,
                _CategoryId = survey.CategoryId,
                _CreatedTime = survey.CreatedTime
            });
            var query = $@"BEGIN TRANSACTION;
DECLARE @InsertedId TABLE(Id INT);
DECLARE @SurveyVersion INT = (SELECT MAX(Version) FROM UserSurveys WHERE Guid=@_guid);
DECLARE @PreSurveyId INT = (SELECT Id FROM [UserSurveys] WHERE Guid=@_guid AND Version=@SurveyVersion);
UPDATE [UserSurveys] SET IsDelete=1 WHERE IsTemplate=1 AND Guid=@_guid;
INSERT INTO UserSurveys (
       [Guid]
      ,[Version]
      ,[SheetId]
      ,[SheetVersion]
      ,[SurveyTitle]
      ,[UserName]
      ,[Link]
      ,[DeadLine]
      ,[ParticipateTime]
      ,[CreatedTime]
      ,[Status]
      ,[CategoryId]
)
OUTPUT INSERTED.[Id] INTO @InsertedId(Id)
VALUES (
       '{survey.Guid}'
      ,'{survey.Version}'
      ,'{survey.SheetId}'
      ,'{survey.SheetVersion}'
      ,@_SurveyTitle
      ,@_UserName
      ,@_Link
      ,@_DeadLine
      ,@_ParticipateTime
      ,@_CreatedTime
      ,@_Status
      ,@_CategoryId);
DECLARE @SurveyId INT = (SELECT TOP(1) Id FROM @InsertedId);
SET @SurveyVersion = (SELECT MAX(SurveyVersion) FROM UserAnswers WHERE SurveyGuid=@_guid);
UPDATE [UserAnswers] SET SurveyId=@SurveyId 
    WHERE SurveyGuid=@_guid AND SurveyId = @PreSurveyId AND SurveyVersion=@SurveyVersion;
COMMIT TRANSACTION
";
            using (var connection = _db.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return survey;
            }
        }

        public async Task<SurveysCount> CountSurveys()
        {
            var query = $@"DECLARE @ActiveSurveyCount INT = ( SELECT COUNT(1) FROM  (SELECT DISTINCT [Guid] FROM [dbo].[UserSurveys]  WHERE DeadLine < GETDATE()) AS tbl); 
                    DECLARE @TotalSurveyCount INT = (SELECT COUNT(1) FROM (SELECT DISTINCT [Guid] FROM [dbo].[UserSurveys]) AS tbl); 
                    DECLARE @RevisedCount INT = (SELECT COUNT(1) FROM [dbo].[UserSurveys]  WHERE Version > 0);
                    SELECT @ActiveSurveyCount AS ActiveSurveyCount,@TotalSurveyCount TotalSurveyCount,@RevisedCount RevisedCount";
            using (var connection = _db.CreateConnection())
            {
                var result = await connection.QueryFirstAsync<SurveysCount>(query);
                return result;
            }
        }
    }
}
