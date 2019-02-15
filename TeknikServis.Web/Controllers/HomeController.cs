using System.Web.Mvc;

namespace TeknikServis.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Error404()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Error500()
        {
            return View();
        }
    }
}