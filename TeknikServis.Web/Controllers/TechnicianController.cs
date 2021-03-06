﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Helpers;
using TeknikServis.BLL.Identity;
using TeknikServis.BLL.Repository;
using TeknikServis.BLL.Services.Senders;
using TeknikServis.Models.Entities;
using TeknikServis.Models.Enums;
using TeknikServis.Models.Models;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MembershipTools;

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
                var data = new IssueRepo().GetAll(x => x.TechnicianId == id && x.IssueState != IssueStates.Tamamlandı).Select(x => Mapper.Map<IssueVM>(x)).ToList();
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
            var data = Mapper.Map<Issue, IssueVM>(issue);
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
                issue.IssueState = IssueStates.İşlemde;
                new IssueRepo().Update(issue);

                var issueLog = new IssueLog()
                {
                    IssueId = issue.Id,
                    Description = "Teknisyen işi aldı.",
                    FromWhom = "Teknisyen"
                };
                new IssueLogRepo().Insert(issueLog);

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
        public ActionResult UpdateJob(IssueVM model)
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
                issue.UpdatedDate = DateTime.Now;
                repo.Update(issue);

                var issueLog = new IssueLog()
                {
                    IssueId = issue.Id,
                    Description = $"Güncelleme: {issue.TechReport}",
                    FromWhom = "Teknisyen"
                };
                new IssueLogRepo().Insert(issueLog);

                return RedirectToAction("Index", "Technician");
            }
            catch (Exception ex)
            {
                TempData["Message"] = new ErrorVM()
                {
                    Text = $"Bir hata oluştu. {ex.Message}",
                    ActionName = "UpdateJob",
                    ControllerName = "Technician",
                    ErrorCode = 500
                };
                return RedirectToAction("Error500", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> FinishJob(IssueVM model)
        {
            try
            {
                var issueRepo = new IssueRepo();
                var issue = issueRepo.GetById(model.IssueId);
                if (issue == null)
                {
                    TempData["Message2"] = "Arıza kaydı bulunamadi.";
                    return RedirectToAction("Index", "Technician");
                }

                issue.IssueState = IssueStates.Tamamlandı;
                issue.ClosedDate = DateTime.Now;
                issueRepo.Update(issue);
                TempData["Message"] = $"{issue.Description} adlı iş tamamlandı.";

                var survey = new Survey();
                var surveyRepo=new SurveyRepo();
                surveyRepo.Insert(survey);
                issue.SurveyId = survey.Id;
                issueRepo.Update(issue);

                var user = await NewUserStore().FindByIdAsync(issue.CustomerId);
                var usernamesurname = GetNameSurname(issue.CustomerId);

                string siteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                 (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                var emailService = new EmailService();
                var body = $"Merhaba <b>{usernamesurname}</b><br>{issue.Description} adlı arıza kaydınız kapanmıştır.<br>Değerlendirmeniz için aşağıda linki bulunan anketi doldurmanızı rica ederiz.<br> <a href='{siteUrl}/issue/survey?code={issue.SurveyId}' >Anket Linki </a> ";
                await emailService.SendAsync(new IdentityMessage() { Body = body, Subject = "Değerlendirme Anketi" }, user.Email);

                var issueLog = new IssueLog()
                {
                    IssueId = issue.Id,
                    Description = "İş tamamlandı.",
                    FromWhom = "Teknisyen"
                };
                new IssueLogRepo().Insert(issueLog);

                return RedirectToAction("Index", "Technician");
            }
            catch (Exception ex)
            {
                TempData["Message"] = new ErrorVM()
                {
                    Text = $"Bir hata oluştu. {ex.Message}",
                    ActionName = "FinishJob",
                    ControllerName = "Technician",
                    ErrorCode = 500
                };
                return RedirectToAction("Error500", "Home");
            }
        }
    }
}