using Dapper;
using Domain.Entities;
using Domain.Interfaces.IRepositories;
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

        public UserCategoryRepository(DapperContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<UserCategory>> GetListAsync(int? page)
        {

        var query = @$"SELECT 
UC.Id,
UC.NameFa,
UC.NameEn,
(SELECT COUNT(1) FROM UserInfos WHERE CategoryId = UC.Id) ParticipantCount
FROM UserCategories UC WHERE UC.IsDelete = 0";
            using (var connection = _db.CreateConnection())
            {
                var result = await connection.QueryAsync<UserCategory>(query);
                return result;
            }
        }
    }
}
