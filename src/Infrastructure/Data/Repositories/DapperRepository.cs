using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class DapperRepository<T> where T:class, IDapperRepository<T>
    {
        private readonly DapperContext _db;
        public DapperRepository(DapperContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            var query = @$"SELECT * FROM {nameof(T)}";

            using (var connection = _db.CreateConnection())
            {
                var sheets = await connection.QueryAsync<T>(query);
                return sheets.ToList();
            }
        }
        public async Task<bool> DeleteAsync(int Id)
        {
            var query = $@"UPDATE {nameof(T)} SET Deleted=1 WHERE Id={Id}";
            using (var connection = _db.CreateConnection())
            {
                var sheets = await connection.ExecuteAsync(query);
            }
            return true;
        }
    }
}
