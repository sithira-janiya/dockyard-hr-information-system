using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApplication1.Database_Layer;
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
                SELECT Division_ID, Division_Name
                FROM dbo.Division
                ORDER BY Division_Name";

            return ExecuteMasterDataQuery(query, null);
        }

        public Response GetDepartments(MasterDataRequestAPI requestAPI)
        {
            string query = @"
                SELECT Department_ID, Department_Name, Division_ID
                FROM dbo.Department";

            List<SqlParameter> parameters = null;

            if (requestAPI != null && int.TryParse(requestAPI.DivisionID, out int divisionId))
            {
                query += " WHERE Division_ID = @DivisionID";
                parameters = new List<SqlParameter>
                {
                    new SqlParameter("@DivisionID", divisionId)
                };
            }

            query += " ORDER BY Department_Name";

            return ExecuteMasterDataQuery(query, parameters);
        }

        public Response GetLocations(MasterDataRequestAPI requestAPI)
        {
            string query = @"
                SELECT Location_ID, Location_Name, Department_ID
                FROM dbo.Location";

            List<SqlParameter> parameters = null;

            if (requestAPI != null && int.TryParse(requestAPI.DepartmentID, out int departmentId))
            {
                query += " WHERE Department_ID = @DepartmentID";
                parameters = new List<SqlParameter>
                {
                    new SqlParameter("@DepartmentID", departmentId)
                };
            }

            query += " ORDER BY Location_Name";

            return ExecuteMasterDataQuery(query, parameters);
        }

        public Response GetTowns(MasterDataRequestAPI requestAPI)
        {
            string query = @"
                SELECT Town_ID, Town_Name
                FROM dbo.Town
                ORDER BY Town_Name";

            return ExecuteMasterDataQuery(query, null);
        }

        public Response GetDesignations(MasterDataRequestAPI requestAPI)
        {
            string query = @"
                SELECT Designation_ID, Designation_Name
                FROM dbo.Designation
                ORDER BY Designation_Name";

            return ExecuteMasterDataQuery(query, null);
        }

        public Response GetEducationLevels(MasterDataRequestAPI requestAPI)
        {
            string query = @"
                SELECT Education_ID, Education_Level
                FROM dbo.EducationLevel
                ORDER BY Education_Level";

            return ExecuteMasterDataQuery(query, null);
        }

        public Response GetPoliceStations(MasterDataRequestAPI requestAPI)
        {
            string query = @"
                SELECT PoliceStation_ID, PoliceStation_Name
                FROM dbo.PoliceStation
                ORDER BY PoliceStation_Name";

            return ExecuteMasterDataQuery(query, null);
        }

        public Response GetElectionDivisions(MasterDataRequestAPI requestAPI)
        {
            string query = @"
        SELECT ElectionDivision_ID, ElectionDivision_Name
        FROM dbo.ElectionDivision
        ORDER BY ElectionDivision_Name";

            return ExecuteMasterDataQuery(query, null);
        }



        private Response ExecuteMasterDataQuery(string query, List<SqlParameter> parameters)
        {
            Response response = new Response();
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            try
            {
                using (DBconnect dbConnect = new DBconnect())
                using (SqlConnection connection = dbConnect.GetOpenConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();

                            for (int index = 0; index < reader.FieldCount; index++)
                            {
                                row.Add(
                                    reader.GetName(index),
                                    reader[index] == DBNull.Value ? null : reader[index]
                                );
                            }

                            resultList.Add(row);
                        }
                    }
                }

                response.StatusCode = 200;
                response.Result = "Master data loaded successfully";
                response.ResultSet = resultList;
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