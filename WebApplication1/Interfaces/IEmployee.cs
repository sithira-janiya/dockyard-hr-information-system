using WebApplication1.Models;
using WebApplication1.Models.RequestApiModels;

namespace WebApplication1.Interfaces
{
    public interface IEmployee
    {
        //#4 CRUD methods
        Response GetEmployeeDetails(EmployeeRequestAPI requestAPI);
        Response GetEmployeeById(EmployeeRequestAPI requestAPI);
        Response SaveEmployee(EmployeeRequestAPI requestAPI);
        Response UpdateEmployee(EmployeeRequestAPI requestAPI);

        //#4 Updated
        Response GetEmployeeDistanceToWorkplace(EmployeeRequestAPI requestAPI);
    }
}