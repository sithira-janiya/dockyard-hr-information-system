\# Dockyard HR Information System - Backend



ASP.NET MVC backend API for the Dockyard HR Information System.



This backend provides employee management and master data APIs for the React frontend. It connects with SQL Server and returns JSON data for employee records, divisions, departments, locations, towns, designations, and education levels.



\## Technology Stack



\- ASP.NET MVC

\- .NET Framework 4.8

\- C#

\- SQL Server

\- System.Data.SqlClient

\- Unity Dependency Injection

\- IIS Express



\## Main Features



\- Retrieve all employee details

\- Retrieve employee by Employee ID

\- Save new employee details

\- Update employee details

\- Retrieve division master data

\- Retrieve department master data

\- Retrieve location master data

\- Retrieve town master data

\- Retrieve designation master data

\- Retrieve education level master data

\- CORS support for React frontend

\- Environment-variable based database connection



\## Project Structure



```text

WebApplication1

├── Controllers

│   ├── EmployeeController.cs

│   ├── MasterDataController.cs

│   └── UserController.cs

├── DataAccess

│   ├── DAEmployee.cs

│   ├── DAEmployeeReport.cs

│   ├── DAMasterData.cs

│   ├── DATest.cs

│   └── DAUser.cs

├── DataBaseConnectivity

│   └── DBConnect.cs

├── Interfaces

├── Models

├── Global.asax.cs

└── Web.config

