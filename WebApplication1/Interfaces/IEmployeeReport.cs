using WebApplication1.Models;
using WebApplication1.Models.RequestApiModels;

namespace WebApplication1.Interfaces
{
    public interface IEmployeeReport
    {
        Response GetEmployeeReport(
            EmployeeReportRequestAPI requestAPI);

        Response GetEmployeeReportById(
            EmployeeReportRequestAPI requestAPI);
    }
}