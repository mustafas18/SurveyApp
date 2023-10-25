using Core.Interfaces;
using Core.Interfaces.IRepositories;
using Core.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class Repository<T> where T : class, IRepository
    {
        private readonly DapperContext _db;
        public Repository(DapperContext db)
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
    }
}
