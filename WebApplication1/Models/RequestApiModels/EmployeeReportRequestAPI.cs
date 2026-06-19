using WebApplication1.Models;

namespace WebApplication1.Models.RequestApiModels
{
    public class EmployeeReportRequestAPI : RequestAPI
    {
        public string EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string DivisionID { get; set; }

        public string DepartmentID { get; set; }

        public string LocationID { get; set; }

        public string TownID { get; set; }

        public string DesignationID { get; set; }

        public string EducationID { get; set; }
    }
}