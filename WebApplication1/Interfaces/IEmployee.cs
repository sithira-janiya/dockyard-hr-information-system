using WebApplication1.Models;
using WebApplication1.Models.RequestApiModels;

namespace WebApplication1.Interfaces
{
    public interface IEmployee
    {
        Response GetEmployeeDetails(EmployeeRequestAPI requestAPI);

        Response GetEmployeeById(EmployeeRequestAPI requestAPI);

        Response SaveEmployee(EmployeeRequestAPI requestAPI);

        Response UpdateEmployee(EmployeeRequestAPI requestAPI);
    }
}