using System.Web.Mvc;
using TeknikServis.Models.ViewModels;

namespace TeknikServis.Web.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public ActionResult Create(IssueVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Hata Oluştu.");
                RedirectToAction("Create", "Issue", model);
            }

            TempData["Message"]= "Arıza kaydınız başarı ile oluşturuldu.";
            return View("Create");
        }

        // GET: Issue/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Issue/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Issue/List/5
        public ActionResult List(string id)
        {
            return View();
        }

        // POST: Issue/List/5
        [HttpPost]
        public ActionResult List(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add list logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
