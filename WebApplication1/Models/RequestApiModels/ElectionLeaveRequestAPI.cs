using WebApplication1.Models;

namespace WebApplication1.Models.RequestApiModels
{
    public class ElectionLeaveRequestAPI : RequestAPI
    {
        public string EmployeeID { get; set; }
        public string ElectionDate { get; set; }
    }
}