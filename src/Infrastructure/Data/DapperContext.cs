using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
#if DEBUG
            _connectionString = _configuration.GetConnectionString("DefaultConnectionString");
#else
            _connectionString = _configuration.GetConnectionString("ReleaseConnectionString");
#endif
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
