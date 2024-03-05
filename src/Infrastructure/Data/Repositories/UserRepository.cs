using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _db;
        private readonly AppDbContext _efContext;

        public UserRepository(DapperContext db, AppDbContext efContext)
        {
            _db = db;
            _efContext = efContext;
        }

        public async Task UpdateInfo(Domain.Entities.UserInfo userInfo)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { _userName = userInfo.UserName });


            var query = @$"UPDATE UserInfos
    SET
      [FirstName] = '{userInfo.FirstName}'
      ,[LastName] = '{userInfo.LastName}'
      ,[Gender] = {(int)userInfo.Gender}
      ,[Birthday] = '{userInfo.Birthday}'
      ,[PictureBase64] = '{userInfo.PictureBase64}'
      ,[Country] ='{userInfo.Country}'
      ,[City] = '{userInfo.City}'
      ,[ResearchInterests] = '{userInfo.ResearchInterests}'
      ,[Grade] = '{userInfo.Grade}'
      ,[Job] = '{userInfo.Job}'
      ,[Mobile] = '{userInfo.Mobile}'
      ,[Email] = '{userInfo.Email}'
      ,[Address] = '{userInfo.Address}'
      ,[AtmCard] = '{userInfo.AtmCard}'
WHERE [UserName]=@_userName";
            using (var connection = _db.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
