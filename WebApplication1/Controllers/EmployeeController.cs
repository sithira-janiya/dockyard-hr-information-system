using System.Web.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models.RequestApiModels;

namespace WebApplication1.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployee _employee;

        public EmployeeController(IEmployee employee)
        {
            _employee = employee;
        }

        [HttpGet]
        public ActionResult GetEmployeeDetails(
            EmployeeRequestAPI requestAPI)
        {
            var result =
                _employee.GetEmployeeDetails(requestAPI);

            return Json(
                result,
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEmployeeById(
            EmployeeRequestAPI requestAPI)
        {
            var result =
                _employee.GetEmployeeById(requestAPI);

            return Json(
                result,
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveEmployee(
            EmployeeRequestAPI requestAPI)
        {
            var result =
                _employee.SaveEmployee(requestAPI);

            return Json(result);
        }

        [HttpPost]
        public ActionResult UpdateEmployee(
            EmployeeRequestAPI requestAPI)
        {
            var result =
                _employee.UpdateEmployee(requestAPI);

            return Json(result);
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}