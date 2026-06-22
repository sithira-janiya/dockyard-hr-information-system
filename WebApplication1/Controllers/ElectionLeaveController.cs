using System.Web.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models.RequestApiModels;

namespace WebApplication1.Controllers
{
    public class ElectionLeaveController : Controller
    {
        private readonly IElectionLeave _electionLeave;

        public ElectionLeaveController(IElectionLeave electionLeave)
        {
            _electionLeave = electionLeave;
        }

        [HttpGet]
        public ActionResult GetElectionLeavePlan(ElectionLeaveRequestAPI requestAPI)
        {
            var result = _electionLeave.GetElectionLeavePlan(requestAPI);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}