using System.Web.Mvc;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }

        [HttpGet]
        public ActionResult GetUser()
        {
            var result = _user.User();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}