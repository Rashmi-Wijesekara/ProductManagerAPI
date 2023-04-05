using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace ProductManagerAPI.Data
{
    public class DataContextDapper
    {
        private readonly IConfiguration _config;
        private IDbConnection _connection;

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
            _connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        public IEnumerable<T> LoadData<T>(string sql)
        {
            return _connection.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            return _connection.QuerySingle<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {
            return _connection.Execute(sql) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            return _connection.Execute(sql);
        }
    }
}
