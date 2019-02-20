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
    [Authorize(Roles = "Operator")]
    public class OperatorController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var data = new IssueRepo().GetAll(x => x.OperatorId == null).Select(x => Mapper.Map<IssueVM>(x)).ToList();
                if (data != null)
                {
                    return View(data);
                }
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorVM()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Operator",
                    ErrorCode = 500
                };
                return RedirectToAction("Error500", "Home");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            ViewBag.TechnicianList = GetTechnicianList();

            var issue = new IssueRepo().GetById(id);
            if (issue == null)
            {
                TempData["Message2"] = "Arıza kaydı bulunamadi.";
                return RedirectToAction("Index", "Operator");
            }
            var userid = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
            if (userid == null)
            {
                return RedirectToAction("Index", "Issue");
            }

            issue.OperatorId = userid;
            var data = Mapper.Map<Issue, IssueVM>(issue);
            if (new IssueRepo().Update(issue) > 0)
            {
                TempData["Message"] = "Üzerine alma işlemi başarılı.";
                return View(data);
            }
            else
            {
                TempData["Message2"] = "Üzerine alma işlemi başarısız.";
                return RedirectToAction("Index", "Operator");
            }

        }
    }
}