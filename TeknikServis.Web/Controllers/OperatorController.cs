using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Entities;
using TeknikServis.Models.ViewModels;
using System.Threading.Tasks;
using TeknikServis.BLL.Helpers;
using static TeknikServis.BLL.Identity.MembershipTools;
using TeknikServis.BLL.Services.Senders;
using TeknikServis.BLL.Identity;

namespace TeknikServis.Web.Controllers
{
    [Authorize]
    public class OperatorController : BaseController
    {
        [HttpGet]
        [Authorize(Roles = "Operator")]
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
                TempData["Message"] = new ErrorVM()
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
        [Authorize(Roles = "Operator")]
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


            var data = Mapper.Map<Issue, IssueVM>(issue);

            if (issue.OperatorId == null)
            {
                issue.OperatorId = userid;
                if (new IssueRepo().Update(issue) > 0)
                {
                    issue.IssueState = Models.Enums.IssueStates.KabulEdildi;
                    data.IssueState = issue.IssueState;
                    new IssueRepo().Update(issue);

                    var issueLog = new IssueLog()
                    {
                        IssueId = issue.Id,
                        Description = "Operatör tarafından kabul edildi.",
                        FromWhom = "Operatör"
                    };
                    new IssueLogRepo().Insert(issueLog);

                    return View(data);
                }
            }

            return View(data);
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
                issue.OptReport = model.OptReport;
                new IssueRepo().Update(issue);
                var technician = await NewUserStore().FindByIdAsync(issue.TechnicianId);
                TempData["Message"] =
                    $"{issue.Description} adlı arızaya {technician.Name}  {technician.Surname} teknisyeni atandı.";

                var customer = NewUserManager().FindById(issue.CustomerId);
                var emailService = new EmailService();
                var body = $"Merhaba <b>{GetNameSurname(issue.CustomerId)}</b><br>{issue.Description} adlı arızanız onaylanmıştır ve görevli teknisyen en kısa sürede yola çıkacaktır.";
                await emailService.SendAsync(new IdentityMessage()
                {
                    Body = body,
                    Subject = $"{issue.Description} adlı arıza hk."
                }, customer.Email);

                var issueLog = new IssueLog()
                {
                    IssueId = issue.Id,
                    Description = "Teknisyene atandı.",
                    FromWhom = "Operatör"
                };
                new IssueLogRepo().Insert(issueLog);

                return RedirectToAction("AllIssues", "Operator");
            }
            catch (DbEntityValidationException ex)
            {
                TempData["Message"] = new ErrorVM()
                {
                    Text = $"Bir hata oluştu: {EntityHelpers.ValidationMessage(ex)}",
                    ActionName = "Create",
                    ControllerName = "Issue",
                    ErrorCode = 500
                };
                return RedirectToAction("Error500", "Home");
            }
            catch (Exception ex)
            {
                TempData["Message"] = new ErrorVM()
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
        [Authorize(Roles = "Operator")]
        public ActionResult AssignedIssues()
        {
            try
            {
                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var data = new IssueRepo().GetAll(x => x.OperatorId == id && x.TechnicianId == null).Select(x => Mapper.Map<IssueVM>(x)).ToList();
                if (data != null)
                {
                    return View(data);
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = new ErrorVM()
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
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult AllIssues()
        {
            try
            {
                var data = new IssueRepo().GetAll().Select(x => Mapper.Map<IssueVM>(x)).ToList();
                if (data != null)
                {
                    return View(data);
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = new ErrorVM()
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
    }
}