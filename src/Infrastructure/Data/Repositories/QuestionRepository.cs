using Core.Entities;
using Core.Interfaces.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<string> DeleteAsync(int Id)
        {
            string sheetId = "";
            var query = $@"
                UPDATE Questions SET Deleted=1 WHERE Id={Id}
                SELECT SheetId1 FROM Questions WHERE Id={Id}";
            using (var connection = _db.CreateConnection())
            {
                sheetId = await connection.QueryFirstAsync<string>(query);
            }
            return sheetId;
        }

    }
}
