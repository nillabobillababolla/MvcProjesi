using System.Collections.Generic;
using System.Web.Mvc;

namespace TeknikServis.Web.Controllers
{
    public class PartialController : Controller
    {
        public PartialViewResult DrawerPartial()
        {
            var data = new List<string>();
            return PartialView("Partial/_DrawerPartial", data);
        }
        public PartialViewResult HeaderPartial()
        {
            return PartialView("Partial/_HeaderPartial");
        }
        public PartialViewResult ModalPartial()
        {
            return PartialView("Partial/_ModalPartial");
        }
        public PartialViewResult FooterPartial()
        {
            return PartialView("Partial/_FooterPartial");
        }
    }
}