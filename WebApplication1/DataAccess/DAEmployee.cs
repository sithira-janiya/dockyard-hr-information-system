using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.BusinessLayer;  // ← NEW: For DistanceCalculator
using WebApplication1.Database_Layer;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.RequestApiModels;

namespace WebApplication1.DataAccess
{
    public class DAEmployee : IEmployee
    {

        //Version 1.0
        public Response GetEmployeeDetails(EmployeeRequestAPI requestAPI)
        {
            Response response = new Response();
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            string query = @"
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
                    ON E.Education_ID = EL.Education_ID
                ORDER BY E.Employee_ID;";

            try
            {
                using (DBconnect dbConnect = new DBconnect())
                using (SqlConnection connection = dbConnect.GetOpenConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employeeList.Add(MapEmployee(reader));
                    }
                }

                response.StatusCode = 200;
                response.Result = "Employee details retrieved successfully";
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

        public Response GetEmployeeById(EmployeeRequestAPI requestAPI)
        {
            Response response = new Response();
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            int employeeId;

            if (requestAPI == null ||
                !int.TryParse(requestAPI.EmployeeID, out employeeId))
            {
                response.StatusCode = 400;
                response.Result = "A valid EmployeeID is required";
                response.ResultSet = null;

                return response;
            }

            string query = @"
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
                    ON E.Education_ID = EL.Education_ID
                WHERE E.Employee_ID = @EmployeeID;";

            try
            {
                using (DBconnect dbConnect = new DBconnect())
                using (SqlConnection connection = dbConnect.GetOpenConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = employeeId;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeeList.Add(MapEmployee(reader));
                        }
                    }
                }

                if (employeeList.Count == 0)
                {
                    response.StatusCode = 404;
                    response.Result = "Employee not found";
                    response.ResultSet = employeeList;

                    return response;
                }

                response.StatusCode = 200;
                response.Result = "Employee retrieved successfully";
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

        public Response SaveEmployee(EmployeeRequestAPI requestAPI)
        {
            Response response = new Response();

            int locationId;
            int townId;
            int designationId;
            int educationId;

            if (!ValidateEmployeeRequest(
                    requestAPI,
                    out locationId,
                    out townId,
                    out designationId,
                    out educationId))
            {
                response.StatusCode = 400;
                response.Result = "Valid employee details are required";
                response.ResultSet = null;

                return response;
            }

            string query = @"
                INSERT INTO dbo.EmployeeDetails
                (
                    Employee_Name,
                    Employee_Address,
                    Location_ID,
                    Town_ID,
                    Designation_ID,
                    Education_ID
                )
                OUTPUT INSERTED.Employee_ID
                VALUES
                (
                    @EmployeeName,
                    @EmployeeAddress,
                    @LocationID,
                    @TownID,
                    @DesignationID,
                    @EducationID
                );";

            try
            {
                int newEmployeeId;

                using (DBconnect dbConnect = new DBconnect())
                using (SqlConnection connection = dbConnect.GetOpenConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddEmployeeParameters(
                        command,
                        requestAPI,
                        locationId,
                        townId,
                        designationId,
                        educationId);

                    newEmployeeId = Convert.ToInt32(command.ExecuteScalar());
                }

                response.StatusCode = 200;
                response.Result = "Employee saved successfully";
                response.ResultSet = new
                {
                    EmployeeID = newEmployeeId
                };
            }
            catch (Exception exception)
            {
                response.StatusCode = 500;
                response.Result = exception.Message;
                response.ResultSet = null;
            }

            return response;
        }

        public Response UpdateEmployee(EmployeeRequestAPI requestAPI)
        {
            Response response = new Response();

            int employeeId;
            int locationId;
            int townId;
            int designationId;
            int educationId;

            if (requestAPI == null ||
                !int.TryParse(requestAPI.EmployeeID, out employeeId) ||
                !ValidateEmployeeRequest(
                    requestAPI,
                    out locationId,
                    out townId,
                    out designationId,
                    out educationId))
            {
                response.StatusCode = 400;
                response.Result = "Valid employee details are required";
                response.ResultSet = null;

                return response;
            }

            string query = @"
                UPDATE dbo.EmployeeDetails
                SET
                    Employee_Name = @EmployeeName,
                    Employee_Address = @EmployeeAddress,
                    Location_ID = @LocationID,
                    Town_ID = @TownID,
                    Designation_ID = @DesignationID,
                    Education_ID = @EducationID
                WHERE Employee_ID = @EmployeeID;";

            try
            {
                int affectedRows;

                using (DBconnect dbConnect = new DBconnect())
                using (SqlConnection connection = dbConnect.GetOpenConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = employeeId;

                    AddEmployeeParameters(
                        command,
                        requestAPI,
                        locationId,
                        townId,
                        designationId,
                        educationId);

                    affectedRows = command.ExecuteNonQuery();
                }

                if (affectedRows == 0)
                {
                    response.StatusCode = 404;
                    response.Result = "Employee not found";
                    response.ResultSet = null;

                    return response;
                }

                response.StatusCode = 200;
                response.Result = "Employee updated successfully";
                response.ResultSet = null;
            }
            catch (Exception exception)
            {
                response.StatusCode = 500;
                response.Result = exception.Message;
                response.ResultSet = null;
            }

            return response;
        }

        //Version 2.0 - New method to calculate distance to workplace
        /// Retrieves employee details along with calculated distance to workplace
        /// <param name="requestAPI">Contains EmployeeID and IncludeDistance flag</param>
        /// <returns>Response with EmployeeDistanceModel containing employee + distance data</returns>
        /// (///) This 3 Slashes Means Here is a XML Documentation Comment which can be used to generate API documentation and provide IntelliSense descriptions in IDEs.   
        public Response GetEmployeeDistanceToWorkplace(EmployeeRequestAPI requestAPI)
        {
            Response response = new Response();
            List<EmployeeDistanceModel> resultList = new List<EmployeeDistanceModel>();

            // Step 1: Validate EmployeeID
            if (requestAPI == null || string.IsNullOrWhiteSpace(requestAPI.EmployeeID))
            {
                response.StatusCode = 400;
                response.Result = "A valid EmployeeID is required";
                response.ResultSet = null;
                return response;
            }

            int employeeId;
            if (!int.TryParse(requestAPI.EmployeeID, out employeeId))
            {
                response.StatusCode = 400;
                response.Result = "Invalid EmployeeID format";
                response.ResultSet = null;
                return response;
            }

            // Step 2: Query to get employee details
            string query = @"
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
                    ON E.Education_ID = EL.Education_ID
                WHERE E.Employee_ID = @EmployeeID;";

            try
            {
                EmployeeModel employee = null;

                // Step 3: Execute query and get employee data
                using (DBconnect dbConnect = new DBconnect())
                using (SqlConnection connection = dbConnect.GetOpenConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = employeeId;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = MapEmployee(reader);
                        }
                    }
                }

                // Step 4: Check if employee exists
                if (employee == null)
                {
                    response.StatusCode = 404;
                    response.Result = "Employee not found";
                    response.ResultSet = null;
                    return response;
                }

                // Step 5: Calculate distance to workplace using DistanceCalculator
                var distanceToWorkplace = DistanceCalculator.GetDistanceToWorkplace(employee.LocationID);
                var employeeCoords = DistanceCalculator.GetLocationCoordinates(employee.LocationID);
                var workplaceCoords = DistanceCalculator.GetWorkplaceCoordinates();

                // Step 6: Build distance model
                var distanceModel = new EmployeeDistanceModel
                {
                    Employee = employee,
                    DistanceToWorkplace = distanceToWorkplace,
                    WorkplaceLocation = DistanceCalculator.GetWorkplaceName(),
                    EmployeeLatitude = employeeCoords.Lat,
                    EmployeeLongitude = employeeCoords.Lon,
                    WorkplaceLatitude = workplaceCoords.Lat,
                    WorkplaceLongitude = workplaceCoords.Lon,
                    FormattedDistance = DistanceCalculator.FormatDistance(distanceToWorkplace)
                };

                resultList.Add(distanceModel);

                // Step 7: Return response
                response.StatusCode = 200;
                response.Result = "Employee distance to workplace calculated successfully";
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

        // Model to hold employee details along with distance information
        public class EmployeeDistanceModel
        {
            public EmployeeModel Employee { get; set; }
            public decimal DistanceToWorkplace { get; set; } // We collect Distance in Kilometers
            public string WorkplaceLocation { get; set; }
            public decimal EmployeeLatitude { get; set; }
            public decimal EmployeeLongitude { get; set; }
            public decimal WorkplaceLatitude { get; set; }
            public decimal WorkplaceLongitude { get; set; }
            public string FormattedDistance { get; set; }
        }

        //Helper method to validate employee request and parse IDs
        private bool ValidateEmployeeRequest(
            EmployeeRequestAPI requestAPI,
            out int locationId,
            out int townId,
            out int designationId,
            out int educationId)
        {
            locationId = 0;
            townId = 0;
            designationId = 0;
            educationId = 0;

            return requestAPI != null &&
                   !string.IsNullOrWhiteSpace(requestAPI.EmployeeName) &&
                   int.TryParse(requestAPI.LocationID, out locationId) &&
                   int.TryParse(requestAPI.TownID, out townId) &&
                   int.TryParse(requestAPI.DesignationID, out designationId) &&
                   int.TryParse(requestAPI.EducationID, out educationId);
        }

        private void AddEmployeeParameters(
            SqlCommand command,
            EmployeeRequestAPI requestAPI,
            int locationId,
            int townId,
            int designationId,
            int educationId)
        {
            command.Parameters.Add(
                "@EmployeeName",
                SqlDbType.NVarChar,
                150).Value = requestAPI.EmployeeName.Trim();

            command.Parameters.Add(
                "@EmployeeAddress",
                SqlDbType.NVarChar,
                250).Value =
                string.IsNullOrWhiteSpace(requestAPI.EmployeeAddress)
                    ? (object)DBNull.Value
                    : requestAPI.EmployeeAddress.Trim();

            command.Parameters.Add("@LocationID", SqlDbType.Int).Value = locationId;
            command.Parameters.Add("@TownID", SqlDbType.Int).Value = townId;
            command.Parameters.Add("@DesignationID", SqlDbType.Int).Value = designationId;
            command.Parameters.Add("@EducationID", SqlDbType.Int).Value = educationId;
        }


        /// Maps a SqlDataReader row to EmployeeModel
        private EmployeeModel MapEmployee(SqlDataReader reader)
        {
            return new EmployeeModel
            {
                EmployeeID = reader["Employee_ID"].ToString(),
                EmployeeName = reader["Employee_Name"].ToString(),
                EmployeeAddress = reader["Employee_Address"].ToString(),
                LocationID = reader["Location_ID"].ToString(),
                LocationName = reader["Location_Name"].ToString(),
                DepartmentID = reader["Department_ID"].ToString(),
                DepartmentName = reader["Department_Name"].ToString(),
                DivisionID = reader["Division_ID"].ToString(),
                DivisionName = reader["Division_Name"].ToString(),
                TownID = reader["Town_ID"].ToString(),
                TownName = reader["Town_Name"].ToString(),
                DesignationID = reader["Designation_ID"].ToString(),
                DesignationName = reader["Designation_Name"].ToString(),
                EducationID = reader["Education_ID"].ToString(),
                EducationLevel = reader["Education_Level"].ToString(),

                // NEW: PoliceStation and ElectionDivision (currently NULL in DB)
                PoliceStationID = reader["PoliceStation_ID"] != DBNull.Value
                    ? reader["PoliceStation_ID"].ToString()
                    : null,
                PoliceStationName = reader["PoliceStation_Name"] != DBNull.Value
                    ? reader["PoliceStation_Name"].ToString()
                    : null,
                ElectionDivisionID = reader["ElectionDivision_ID"] != DBNull.Value
                    ? reader["ElectionDivision_ID"].ToString()
                    : null,
                ElectionDivisionName = reader["ElectionDivision_Name"] != DBNull.Value
                    ? reader["ElectionDivision_Name"].ToString()
                    : null,

                // Distance properties (will be populated separately)
                Latitude = null,
                Longitude = null,
                DistanceToWorkplace = null,
                WorkplaceLocation = null
            };
        }
    }
}