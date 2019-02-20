using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Entities;
using TeknikServis.Models.ViewModels;
using System.Threading.Tasks;
using static TeknikServis.BLL.Identity.MembershipTools;
using TeknikServis.BLL.Services.Senders;

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
                data.OperatorName = issue.Operator.Name + " " + issue.Operator.Surname;
                issue.IssueState = Models.Enums.IssueStates.KabulEdildi;
                data.IssueState = issue.IssueState;
                new IssueRepo().Update(issue);
                TempData["Message"] = "Üzerine alma işlemi başarılı.";
                return View(data);
            }
            else
            {
                TempData["Message2"] = "Üzerine alma işlemi başarısız.";
                return RedirectToAction("Index", "Operator");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> AssignTechAsync(IssueVM model)
        {
            try
            {
                var issue = new IssueRepo().GetById(model.IssueId);
                issue.TechnicianId = model.TechnicianId;
                issue.IssueState = Models.Enums.IssueStates.Atandı;
                new IssueRepo().Update(issue);
                var technician = await NewUserStore().FindByIdAsync(issue.TechnicianId);
                TempData["Message"] =
                    $"{issue.Description} adlı arızaya {technician.Name}  {technician.Surname} teknisyeni atandı.";

                var emailService = new EmailService();
                var body = $"Merhaba <b>{issue.Customer.Name} {issue.Customer.Surname}</b><br>{issue.Description} adlı arızanız onaylanmıştır ve görevli teknisyen en kısa sürede yola çıkacaktır.";
                await emailService.SendAsync(new IdentityMessage()
                {
                    Body = body,
                    Subject = $"{issue.Description} adlı arıza hk."
                }, issue.Customer.Email);

                return RedirectToAction("Index", "Operator");
            }

            catch (Exception ex)
            {
                TempData["Model"] = new ErrorVM()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "AssignTechAsync",
                    ControllerName = "Operator",
                    ErrorCode = 500
                };
                return RedirectToAction("Error500", "Home");
            }
        }

        [HttpGet]
        public ActionResult AssignedIssues()
        {
            return View();
        }
    }
}