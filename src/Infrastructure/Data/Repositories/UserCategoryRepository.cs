using Dapper;
using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class UserCategoryRepository : IUserCategoryRepository
    {
        private readonly DapperContext _db;
        private readonly AppDbContext _efContext;

        public UserCategoryRepository(DapperContext db, AppDbContext efContext)
        {
            _db = db;
            _efContext = efContext;
        }

        public async Task<IEnumerable<UserInfo>> GetCategoryUsers(int categoryId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _categoryId = categoryId });


            var query = @$"SELECT 
[Id]
,[UserName]
,FirstName
,LastName
,CONCAT(FirstName, ' ', LastName) Name
,[Mobile]
,[Email]
,[IsVerified]
FROM UserInfos
WHERE [CategoryId1]=@_categoryId";
            using (var connection = _db.CreateConnection())
            {
                var result = await connection.QueryAsync<UserInfo>(query,parameters);
                return result;
            }

        }

        public async Task<IEnumerable<UserCategory>> GetListAsync(int? page)
        {

        var query = @$"SELECT 
Id,
NameFa,
NameEn,
UserCount
FROM vw_UserCategories";
            using (var connection = _db.CreateConnection())
            {
                var result = await connection.QueryAsync<UserCategory>(query);
                return result;
            }
        }
    }
}
