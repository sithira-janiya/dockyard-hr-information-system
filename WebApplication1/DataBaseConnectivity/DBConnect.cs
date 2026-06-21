using System;
using System.Configuration;
using System.Data.SqlClient;

namespace WebApplication1.DataBaseConnectivity
{
    public class DBconnect
    {
        public SqlConnection GetOpenConnection()
        {
            string connectionString =
                Environment.GetEnvironmentVariable("DOCKYARD_HR_DB_CONNECTION");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = ConfigurationManager.AppSettings["DOCKYARD_HR_DB_CONNECTION"];
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Database connection string missing. Set DOCKYARD_HR_DB_CONNECTION.");
            }

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            return connection;
        }
    }
}