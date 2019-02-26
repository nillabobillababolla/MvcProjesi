using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TeknikServis.Models.IdentityModels;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MembershipTools;

namespace TeknikServis.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        [HttpGet]
        [Route("kullanici_detay")]
        public ActionResult Index()
        {
            var userManager = NewUserManager();
            var userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
            var user = userManager.FindById(userId);
            if (user == null)
                RedirectToAction("Error500", "Home");
            var data = Mapper.Map<User, UserProfileVM>(user);
            return View(data);
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