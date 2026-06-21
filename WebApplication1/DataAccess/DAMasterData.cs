using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.DataBaseConnectivity;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.RequestApiModels;

namespace WebApplication1.DataAccess
{
    public class DAMasterData : IMasterData
    {
        public Response GetDivisions(MasterDataRequestAPI requestAPI)
        {
            string query = @"
                SELECT
                    Division_ID,
                    Division_Name
                FROM dbo.Division
                ORDER BY Division_Name;";

            return ExecuteMasterDataQuery(query, null);
        }

        public Response GetDepartments(MasterDataRequestAPI requestAPI)
        {
            int divisionId;

            if (requestAPI != null &&
                int.TryParse(requestAPI.DivisionID, out divisionId))
            {
                string filteredQuery = @"
                    SELECT
                        Department_ID,
                        Department_Name,
                        Division_ID
                    FROM dbo.Department
                    WHERE Division_ID = @DivisionID
                    ORDER BY Department_Name;";

                SqlParameter parameter = new SqlParameter(
                    "@DivisionID",
                    SqlDbType.Int)
                {
                    Value = divisionId
                };

                return ExecuteMasterDataQuery(
                    filteredQuery,
                    new[] { parameter });
            }

            string query = @"
                SELECT
                    Department_ID,
                    Department_Name,
                    Division_ID
                FROM dbo.Department
                ORDER BY Department_Name;";

            return ExecuteMasterDataQuery(query, null);
        }

        public Response GetLocations(MasterDataRequestAPI requestAPI)
        {
            int departmentId;

            if (requestAPI != null &&
                int.TryParse(requestAPI.DepartmentID, out departmentId))
            {
                string filteredQuery = @"
                    SELECT
                        Location_ID,
                        Location_Name,
                        Department_ID
                    FROM dbo.Location
                    WHERE Department_ID = @DepartmentID
                    ORDER BY Location_Name;";

                SqlParameter parameter = new SqlParameter(
                    "@DepartmentID",
                    SqlDbType.Int)
                {
                    Value = departmentId
                };

                return ExecuteMasterDataQuery(
                    filteredQuery,
                    new[] { parameter });
            }

            string query = @"
                SELECT
                    Location_ID,
                    Location_Name,
                    Department_ID
                FROM dbo.Location
                ORDER BY Location_Name;";

            return ExecuteMasterDataQuery(query, null);
        }

        public Response GetTowns(MasterDataRequestAPI requestAPI)
        {
            string query = @"
                SELECT
                    Town_ID,
                    Town_Name
                FROM dbo.Town
                ORDER BY Town_Name;";

            return ExecuteMasterDataQuery(query, null);
        }

        public Response GetDesignations(MasterDataRequestAPI requestAPI)
        {
            string query = @"
                SELECT
                    Designation_ID,
                    Designation_Name
                FROM dbo.Designation
                ORDER BY Designation_Name;";

            return ExecuteMasterDataQuery(query, null);
        }

        public Response GetEducationLevels(
            MasterDataRequestAPI requestAPI)
        {
            string query = @"
                SELECT
                    Education_ID,
                    Education_Level
                FROM dbo.EducationLevel
                ORDER BY Education_Level;";

            return ExecuteMasterDataQuery(query, null);
        }

        private Response ExecuteMasterDataQuery(
            string query,
            SqlParameter[] parameters)
        {
            Response response = new Response();
            List<Dictionary<string, object>> dataList =
                new List<Dictionary<string, object>>();

            try
            {
                using (DBconnect dbConnect = new DBconnect())
                using (SqlConnection connection =
                       dbConnect.GetOpenConnection())
                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataReader reader =
                           command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> item =
                                new Dictionary<string, object>();

                            for (int columnIndex = 0;
                                 columnIndex < reader.FieldCount;
                                 columnIndex++)
                            {
                                string columnName =
                                    reader.GetName(columnIndex);

                                object value =
                                    reader.IsDBNull(columnIndex)
                                        ? null
                                        : reader.GetValue(columnIndex);

                                item[columnName] = value;
                            }

                            dataList.Add(item);
                        }
                    }
                }

                response.StatusCode = 200;
                response.Result =
                    "Master data retrieved successfully";
                response.ResultSet = dataList;
            }
            catch (Exception exception)
            {
                response.StatusCode = 500;
                response.Result = exception.Message;
                response.ResultSet = null;
            }

            return response;
        }
    }
}