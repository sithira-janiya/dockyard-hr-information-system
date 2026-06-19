using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using WebApplication1.Models;

namespace WebApplication1.Database_Layer
{
    internal class DBconnect : IDisposable
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        public DBconnect()
        {
            _connectionString = Environment.GetEnvironmentVariable("DOCKYARD_HR_DB_CONNECTION");

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException(
                    "Database connection string is missing. Please set the DOCKYARD_HR_DB_CONNECTION environment variable.");
            }
        }

        public SqlConnection GetOpenConnection()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();

            return _connection;
        }

        public SqlDataReader ReadTable(string readStr)
        {
            SqlConnection connection = GetOpenConnection();
            SqlCommand command = new SqlCommand(readStr, connection);

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public ProcedureDBModel ProcedureRead(
            RequestAPI requestAPI,
            string procedureName)
        {
            ProcedureDBModel result = new ProcedureDBModel();

            using (SqlConnection connection = GetOpenConnection())
            using (SqlCommand command = new SqlCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter statusCodeParameter =
                    new SqlParameter("@ResultStatusCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                SqlParameter resultParameter =
                    new SqlParameter("@Result", SqlDbType.VarChar, -1)
                    {
                        Direction = ParameterDirection.Output
                    };

                SqlParameter exceptionParameter =
                    new SqlParameter("@ExceptionMessage", SqlDbType.VarChar, -1)
                    {
                        Direction = ParameterDirection.Output
                    };

                command.Parameters.Add(statusCodeParameter);
                command.Parameters.Add(resultParameter);
                command.Parameters.Add(exceptionParameter);

                if (requestAPI != null)
                {
                    Type requestType = requestAPI.GetType();

                    PropertyInfo[] properties =
                        requestType.GetProperties(
                            BindingFlags.Public |
                            BindingFlags.Instance);

                    foreach (PropertyInfo property in properties)
                    {
                        string parameterName = "@" + property.Name;

                        object parameterValue =
                            property.GetValue(requestAPI) ?? DBNull.Value;

                        SqlParameter inputParameter =
                            new SqlParameter(parameterName, SqlDbType.VarChar)
                            {
                                Direction = ParameterDirection.Input,
                                Value = parameterValue
                            };

                        command.Parameters.Add(inputParameter);
                    }
                }

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        result.ResultDataTable = dataTable;
                    }

                    result.ResultStatusCode =
                        statusCodeParameter.Value != DBNull.Value
                            ? statusCodeParameter.Value.ToString()
                            : "1";

                    result.Result =
                        resultParameter.Value != DBNull.Value
                            ? resultParameter.Value.ToString()
                            : "Success";

                    result.ExceptionMessage =
                        exceptionParameter.Value != DBNull.Value
                            ? exceptionParameter.Value.ToString()
                            : null;
                }
                catch (Exception exception)
                {
                    result.ResultStatusCode = "-1";
                    result.Result = "Failed";
                    result.ExceptionMessage = exception.Message;
                }
            }

            return result;
        }

        public bool AddEditDel(string AddEditDelStr)
        {
            using (SqlConnection connection = GetOpenConnection())
            using (SqlCommand command = new SqlCommand(AddEditDelStr, connection))
            {
                int affectedRows = command.ExecuteNonQuery();

                return affectedRows > 0;
            }
        }

        internal void ExecuteQuery(string query)
        {
            using (SqlConnection connection = GetOpenConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            if (_connection == null)
            {
                return;
            }

            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }

            _connection.Dispose();
            _connection = null;
        }
    }
}