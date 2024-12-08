using Microsoft.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ReactMUIWebAPIApplication.DBConnection
{
    public class DBContext
    {
        //private readonly IConfigurationProvider _configuration;
        private readonly string _connectionString;

        public DBContext(IConfigurationProvider configuration)
        {
           // _configuration = configuration;
            //_connectionString = ConfigurationManager.ConnectionStrings["test"];//_configuration.GetConnectionString("dbconnection");
        }

        public DBContext()
        {
            _connectionString = "Data Source=localhost;Initial Catalog=ReactMUI;Integrated Security=True;User Id=user001;Password=password001;MultipleActiveResultSets=True;TrustServerCertificate=True";
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
