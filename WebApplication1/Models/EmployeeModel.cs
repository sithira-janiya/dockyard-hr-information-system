using System;

namespace WebApplication1.Models
{
    public class EmployeeModel
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeAddress { get; set; }

        public string LocationID { get; set; }
        public string LocationName { get; set; }

        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }

        public string DivisionID { get; set; }
        public string DivisionName { get; set; }

        public string TownID { get; set; }
        public string TownName { get; set; }

        public string DesignationID { get; set; }
        public string DesignationName { get; set; }

        public string EducationID { get; set; }
        public string EducationLevel { get; set; }

        //#1 Updated
        public string PoliceStationID { get; set; }
        public string PoliceStationName { get; set; }

        //#1 Updated
        public string ElectionDivisionID { get; set; }
        public string ElectionDivisionName { get; set; }

        //#1 Updated
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        //#1 Updated
        public decimal? DistanceToWorkplace { get; set; }
        public string WorkplaceLocation { get; set; }
    }
}