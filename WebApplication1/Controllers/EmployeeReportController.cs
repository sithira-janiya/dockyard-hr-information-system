using System.Web.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models.RequestApiModels;

namespace WebApplication1.Controllers
{
    public class EmployeeReportController : Controller
    {
        private readonly IEmployeeReport _employeeReport;

        public EmployeeReportController(
            IEmployeeReport employeeReport)
        {
            _employeeReport = employeeReport;
        }

        [HttpGet]
        public ActionResult GetEmployeeReport(
            EmployeeReportRequestAPI requestAPI)
        {
            var result =
                _employeeReport.GetEmployeeReport(requestAPI);

            return Json(
                result,
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEmployeeReportById(
            EmployeeReportRequestAPI requestAPI)
        {
            var result =
                _employeeReport.GetEmployeeReportById(
                    requestAPI);

            return Json(
                result,
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}