# Dockyard HR Information System - Backend

ASP.NET MVC backend API for the Dockyard HR Information System.

This backend provides REST-style JSON APIs for employee management and master data retrieval. It is designed to work with the React frontend and connects to a SQL Server database using `System.Data.SqlClient`.

## Repository

**Backend Repository:**
`https://github.com/sithira-janiya/dockyard-hr-information-system.git`

**Frontend Repository:**
`https://github.com/sithira-janiya/dockyard-hr-frontend.git`

---

## Technology Stack

* ASP.NET MVC
* .NET Framework 4.8
* C#
* SQL Server
* System.Data.SqlClient
* Unity Dependency Injection
* IIS Express
* React frontend integration
* Environment-variable based database connection

---

## Main Features

### Employee Management

* Retrieve all employee details
* Retrieve employee details by Employee ID
* Save new employee records
* Update existing employee records
* Retrieve employee report data with related master information

### Master Data Management

* Retrieve divisions
* Retrieve departments
* Retrieve locations
* Retrieve towns
* Retrieve designations
* Retrieve education levels

### Integration Features

* JSON API responses for frontend usage
* CORS support for React frontend
* Environment-variable based database connection for better security
* Layered backend structure using controllers, interfaces, and data access classes
* Dependency injection using Unity

---

## Project Structure

```text
WebApplication1
├── Controllers
│   ├── EmployeeController.cs
│   ├── MasterDataController.cs
│   └── UserController.cs
│
├── DataAccess
│   ├── DAEmployee.cs
│   ├── DAEmployeeReport.cs
│   ├── DAMasterData.cs
│   ├── DATest.cs
│   └── DAUser.cs
│
├── DataBaseConnectivity
│   └── DBConnect.cs
│
├── Interfaces
│   ├── IEmployee.cs
│   ├── IEmployeeReport.cs
│   ├── IMasterData.cs
│   ├── ITest.cs
│   └── IUser.cs
│
├── Models
│
├── App_Start
│   └── UnityConfig.cs
│
├── Global.asax.cs
└── Web.config
```

---

## Backend API Overview

### Employee APIs

```text
GET /Employee/GetEmployeeDetails
GET /Employee/GetEmployeeById?id={employeeId}
POST /Employee/SaveEmployee
POST /Employee/UpdateEmployee
```

### Master Data APIs

```text
GET /MasterData/GetDivisions
GET /MasterData/GetDepartments
GET /MasterData/GetLocations
GET /MasterData/GetTowns
GET /MasterData/GetDesignations
GET /MasterData/GetEducationLevels
```

---

## Database Connection

The backend uses an environment variable to store the database connection string.

Environment variable name:

```text
DOCKYARD_HR_DB_CONNECTION
```

Example format:

```text
Server=SERVER_NAME;Database=DATABASE_NAME;User Id=USER_NAME;Password=PASSWORD;MultipleActiveResultSets=True;TrustServerCertificate=True;
```

For security reasons, real database credentials should not be hardcoded inside the source code.

---

## CORS Configuration

CORS is configured to allow requests from the React frontend.

Default frontend URL:

```text
http://localhost:3000
```

Default backend URL:

```text
http://localhost:60748
```

CORS handling is managed inside `Global.asax.cs` using `Application_BeginRequest`.

---

## How to Run the Backend

1. Clone the backend repository.

```bash
git clone https://github.com/sithira-janiya/dockyard-hr-information-system.git
```

2. Open the solution in Visual Studio.

3. Set the required database connection environment variable.

4. Restore NuGet packages if required.

5. Run the project using IIS Express.

6. Confirm that the backend is running on:

```text
http://localhost:60748
```

---

## How to Test

You can test the backend APIs using:

* Browser
* Postman
* React frontend
* IIS Express local server

Example test URL:

```text
http://localhost:60748/Employee/GetEmployeeDetails
```

---

## Development Notes

* Database operations are handled using direct SQL queries.
* SQL parameters are used to reduce SQL injection risk.
* Data access logic is separated from controllers.
* Interfaces are used to support dependency injection.
* The backend returns JSON responses for frontend consumption.
* Sensitive database credentials should be managed using environment variables.

---

## Current Status

The backend currently supports employee data retrieval, employee save/update operations, master data retrieval, frontend integration, and SQL Server database connectivity.

This backend is developed as part of the Dockyard HR Information System internship project.
