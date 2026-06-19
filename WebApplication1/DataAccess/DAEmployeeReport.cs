using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Database_Layer;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.RequestApiModels;

namespace WebApplication1.DataAccess
{
    public class DAEmployeeReport : IEmployeeReport
    {
        public Response GetEmployeeReport(
            EmployeeReportRequestAPI requestAPI)
        {
            string query = GetEmployeeReportQuery();

            return ExecuteEmployeeReport(
                query,
                null);
        }

        public Response GetEmployeeReportById(
            EmployeeReportRequestAPI requestAPI)
        {
            Response response = new Response();

            int employeeId;

            if (requestAPI == null ||
                !int.TryParse(
                    requestAPI.EmployeeID,
                    out employeeId))
            {
                response.StatusCode = 400;
                response.Result =
                    "A valid EmployeeID is required";
                response.ResultSet = null;

                return response;
            }

            string query =
                GetEmployeeReportQuery() +
                " WHERE E.Employee_ID = @EmployeeID;";

            SqlParameter parameter = new SqlParameter(
                "@EmployeeID",
                SqlDbType.Int)
            {
                Value = employeeId
            };

            return ExecuteEmployeeReport(
                query,
                new[] { parameter });
        }

        private string GetEmployeeReportQuery()
        {
            return @"
                SELECT
                    E.Employee_ID,
                    E.Employee_Name,
                    E.Employee_Address,
                    L.Location_ID,
                    L.Location_Name,
                    D.Department_ID,
                    D.Department_Name,
                    V.Division_ID,
                    V.Division_Name,
                    T.Town_ID,
                    T.Town_Name,
                    G.Designation_ID,
                    G.Designation_Name,
                    EL.Education_ID,
                    EL.Education_Level
                FROM dbo.EmployeeDetails AS E
                INNER JOIN dbo.Location AS L
                    ON E.Location_ID = L.Location_ID
                INNER JOIN dbo.Department AS D
                    ON L.Department_ID = D.Department_ID
                INNER JOIN dbo.Division AS V
                    ON D.Division_ID = V.Division_ID
                INNER JOIN dbo.Town AS T
                    ON E.Town_ID = T.Town_ID
                INNER JOIN dbo.Designation AS G
                    ON E.Designation_ID = G.Designation_ID
                INNER JOIN dbo.EducationLevel AS EL
                    ON E.Education_ID = EL.Education_ID";
        }

        private Response ExecuteEmployeeReport(
            string query,
            SqlParameter[] parameters)
        {
            Response response = new Response();
            List<EmployeeModel> employeeList =
                new List<EmployeeModel>();

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
                            employeeList.Add(
                                MapEmployee(reader));
                        }
                    }
                }

                if (employeeList.Count == 0)
                {
                    response.StatusCode = 404;
                    response.Result =
                        "Employee report data not found";
                    response.ResultSet = employeeList;

                    return response;
                }

                response.StatusCode = 200;
                response.Result =
                    "Employee report retrieved successfully";
                response.ResultSet = employeeList;
            }
            catch (Exception exception)
            {
                response.StatusCode = 500;
                response.Result = exception.Message;
                response.ResultSet = null;
            }

            return response;
        }

        private EmployeeModel MapEmployee(
            SqlDataReader reader)
        {
            return new EmployeeModel
            {
                EmployeeID =
                    reader["Employee_ID"].ToString(),

                EmployeeName =
                    reader["Employee_Name"].ToString(),

                EmployeeAddress =
                    reader["Employee_Address"].ToString(),

                LocationID =
                    reader["Location_ID"].ToString(),

                LocationName =
                    reader["Location_Name"].ToString(),

                DepartmentID =
                    reader["Department_ID"].ToString(),

                DepartmentName =
                    reader["Department_Name"].ToString(),

                DivisionID =
                    reader["Division_ID"].ToString(),

                DivisionName =
                    reader["Division_Name"].ToString(),

                TownID =
                    reader["Town_ID"].ToString(),

                TownName =
                    reader["Town_Name"].ToString(),

                DesignationID =
                    reader["Designation_ID"].ToString(),

                DesignationName =
                    reader["Designation_Name"].ToString(),

                EducationID =
                    reader["Education_ID"].ToString(),

                EducationLevel =
                    reader["Education_Level"].ToString()
            };
        }
    }
}