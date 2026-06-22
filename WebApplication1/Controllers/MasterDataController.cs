using System.Web.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models.RequestApiModels;

namespace WebApplication1.Controllers
{
    public class MasterDataController : Controller
    {
        private readonly IMasterData _masterData;

        public MasterDataController(IMasterData masterData)
        {
            _masterData = masterData;
        }

        [HttpGet]
        public ActionResult GetDivisions(MasterDataRequestAPI requestAPI)
        {
            var result = _masterData.GetDivisions(requestAPI);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDepartments(MasterDataRequestAPI requestAPI)
        {
            var result = _masterData.GetDepartments(requestAPI);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetLocations(MasterDataRequestAPI requestAPI)
        {
            var result = _masterData.GetLocations(requestAPI);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTowns(MasterDataRequestAPI requestAPI)
        {
            var result = _masterData.GetTowns(requestAPI);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDesignations(MasterDataRequestAPI requestAPI)
        {
            var result = _masterData.GetDesignations(requestAPI);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEducationLevels(MasterDataRequestAPI requestAPI)
        {
            var result = _masterData.GetEducationLevels(requestAPI);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPoliceStations(MasterDataRequestAPI requestAPI)
        {
            var result = _masterData.GetPoliceStations(requestAPI);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetElectionDivisions(MasterDataRequestAPI requestAPI)
        {
            var result = _masterData.GetElectionDivisions(requestAPI);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}