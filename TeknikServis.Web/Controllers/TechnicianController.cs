using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Entities;
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
        public ActionResult GetJob(IssueVM model)
        {
            var issue = new IssueRepo().GetById(model.IssueId);
            if (issue == null)
            {
                TempData["Message"] = new ErrorVM()
                {
                    Text = $"Bir hata oluştu.",
                    ActionName = "Index",
                    ControllerName = "Technician",
                    ErrorCode = 500
                };
                return RedirectToAction("Error500", "Home");
            }
            issue.IssueState = Models.Enums.IssueStates.İşlemde;
            issue.IsActive = true;
            new IssueRepo().Update(issue);
            TempData["Message"] = "İş Kabul Edildi.";
            return RedirectToAction("Index","Technician");
        }

        [HttpPost]
        public ActionResult FinishJob(IssueVM model)
        {
            var issue = new IssueRepo().GetById(model.IssueId);
            if (issue == null)
            {
                TempData["Message"] = new ErrorVM()
                {
                    Text = $"Bir hata oluştu.",
                    ActionName = "Index",
                    ControllerName = "Technician",
                    ErrorCode = 500
                };
                return RedirectToAction("Error500", "Home");
            }
            issue.IssueState = Models.Enums.IssueStates.Tamamlandı;
            issue.IsActive = false;
            new IssueRepo().Update(issue);
            TempData["Message"] = "İş Tamamlandı.";
            return RedirectToAction("Index", "Technician");
        }

    }
}