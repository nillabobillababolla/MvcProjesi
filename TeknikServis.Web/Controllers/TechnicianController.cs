using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Entities;
using TeknikServis.Models.Enums;
using TeknikServis.Models.Models;
using TeknikServis.Models.ViewModels;

namespace TeknikServis.Web.Controllers
{
    [Authorize(Roles = "Technician")]
    public class TechnicianController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var data = new IssueRepo().GetAll(x => x.TechnicianId == id && x.IssueState != Models.Enums.IssueStates.Tamamlandı).Select(x => Mapper.Map<IssueVM>(x)).ToList();
                if (data != null)
                {
                    return View(data);
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = new ErrorVM()
                {
                    Text = $"Bir hata oluştu. {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Technician",
                    ErrorCode = 500
                };
                return RedirectToAction("Error500", "Home");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            var issue = new IssueRepo().GetById(id);
            var data = Mapper.Map <Issue , IssueVM> (issue);
            return View(data);
        }

        [HttpPost]
        public JsonResult GetJob(string id)
        {
            try
            {
                var issue = new IssueRepo().GetById(id);
                if (issue == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Bulunamadi.",
                        success = false
                    });
                }
                issue.IssueState = Models.Enums.IssueStates.İşlemde;
                new IssueRepo().Update(issue);
                return Json(new ResponseData()
                {
                    message = "İş onayı başarılı",
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata oluştu: {ex.Message}",
                    success = false
                });
            }
        }

        [HttpPost]
        public ActionResult FinishJob(IssueVM model)
        {
            try
            {
                var repo = new IssueRepo();
                 var issue = repo.GetById(model.IssueId);
               if (issue == null)
               {
                   TempData["Message2"] = "Arıza kaydı bulunamadi.";
                   return RedirectToAction("Index", "Technician");
               }

               issue.TechReport = model.TechReport;
               issue.ServiceCharge += model.ServiceCharge;
               issue.IssueState = IssueStates.Tamamlandı;
               issue.ClosedDate=DateTime.Now;
               repo.Update(issue);
               TempData["Message"] = $"{issue.Description} adlı iş tamamlandı.";
               return RedirectToAction("Index", "Technician");
            }
            catch (Exception ex)
            {
                TempData["Message"] = new ErrorVM()
                {
                    Text = $"Bir hata oluştu. {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Technician",
                    ErrorCode = 500
                };
                return RedirectToAction("Error500", "Home");
            }
        }

    }
}