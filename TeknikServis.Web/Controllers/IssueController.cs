using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Helpers;
using TeknikServis.BLL.Repository;
using TeknikServis.BLL.Services.Senders;
using TeknikServis.Models.Entities;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MembershipTools;
using WebImage = System.Web.Helpers.WebImage;

namespace TeknikServis.Web.Controllers
{
    [Authorize]
    public class IssueController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var data = new IssueRepo().GetAll(x => x.CustomerId == id).Select(x => Mapper.Map<IssueVM>(x)).ToList();
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
                    ActionName = "Details",
                    ControllerName = "Issue",
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
            if (issue == null)
            {
                TempData["Message2"] = "Arıza kaydı bulunamadi.";
                return RedirectToAction("Index", "Issue");
            }
            var data = Mapper.Map<Issue, IssueVM>(issue);
            data.PhotoPath = new PhotographRepo().GetAll(x => x.IssueId == id).Select(y => y.Path).ToList();
            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "Customer, Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<ActionResult> Create(IssueVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Hata Oluştu.");
                return RedirectToAction("Create", "Issue", model);
            }
            try
            {
                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var user = NewUserManager().FindById(id);
                var issue = new Issue()
                {
                    Description = model.Description,
                    IssueState = model.IssueState,
                    Location = model.Location == Models.Enums.Locations.KonumYok ? user.Location : model.Location,
                    ProductType = model.ProductType,
                    CustomerId = model.CustomerId,
                    PurchasedDate = model.PurchasedDate,
                    PhotoPath = model.PhotoPath,
                    ServiceCharge = model.ServiceCharge,
                    ClosedDate = model.ClosedDate,
                    CreatedDate = model.CreatedDate,
                    OperatorId = model.OperatorId,
                    TechReport = model.TechReport
                };
                switch (issue.ProductType)
                {
                    case Models.Enums.ProductTypes.Buzdolabı:
                        if (issue.PurchasedDate.AddYears(1) > DateTime.Now)
                        {
                            issue.WarrantyState = true;
                        }
                        break;
                    case Models.Enums.ProductTypes.BulaşıkMakinesi:
                        if (issue.PurchasedDate.AddYears(2) > DateTime.Now)
                        {
                            issue.WarrantyState = true;
                        }
                        break;
                    case Models.Enums.ProductTypes.Fırın:
                        if (issue.PurchasedDate.AddYears(3) > DateTime.Now)
                        {
                            issue.WarrantyState = true;
                        }
                        break;
                    case Models.Enums.ProductTypes.ÇamaşırMakinesi:
                        if (issue.PurchasedDate.AddYears(4) > DateTime.Now)
                        {
                            issue.WarrantyState = true;
                        }
                        break;
                    case Models.Enums.ProductTypes.Mikrodalga:
                        if (issue.PurchasedDate.AddYears(5) > DateTime.Now)
                        {
                            issue.WarrantyState = true;
                        }
                        break;
                    default:
                        if (issue.PurchasedDate.AddYears(2) > DateTime.Now)
                        {
                            issue.WarrantyState = true;
                        }
                        break;
                }
                if (issue.WarrantyState)
                {
                    issue.ServiceCharge = 0;
                }

                var repo = new IssueRepo();
                repo.Insert(issue);
                var fotorepo = new PhotographRepo();
                if (model.PostedPhoto.Count > 0)
                {
                    model.PostedPhoto.ForEach(file =>
                    {
                        if (file == null || file.ContentLength <= 0)
                        {
                            var filepath2 = Server.MapPath("~/assets/images/image-not-available.png");

                            var img2 = new WebImage(filepath2);
                            img2.Resize(250, 250, false);
                            img2.Save(filepath2);

                            fotorepo.Insert(new Photograph()
                            {
                                IssueId = issue.Id,
                                Path = "/assets/images/image-not-available.png"
                            });

                            return;
                        }

                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        var extName = Path.GetExtension(file.FileName);
                        fileName = StringHelpers.UrlFormatConverter(fileName);
                        fileName += StringHelpers.GetCode();
                        var directorypath = Server.MapPath("~/Upload/");
                        var filepath = Server.MapPath("~/Upload/") + fileName + extName;

                        if (!Directory.Exists(directorypath))
                        {
                            Directory.CreateDirectory(directorypath);
                        }

                        file.SaveAs(filepath);

                        var img = new WebImage(filepath);
                        img.Resize(250, 250, false);
                        img.Save(filepath);

                        fotorepo.Insert(new Photograph()
                        {
                            IssueId = issue.Id,
                            Path = "/Upload/" + fileName + extName
                        });
                    });
                }

                var fotograflar = fotorepo.GetAll(x => x.IssueId == issue.Id).ToList();
                var foto = fotograflar.Select(x => x.Path).ToList();
                issue.PhotoPath = foto;
                repo.Update(issue);

                TempData["Message"] = "Arıza kaydınız başarı ile oluşturuldu.";

                var emailService = new EmailService();

                var body = $"Merhaba <b>{user.Name} {user.Surname}</b><br>Arıza kaydınız başarıyla oluşturuldu.Birimlerimiz sorunu çözmek için en kısa zamanda olay yerine intikal edecektir.<br><br> Ayrıntılı bilgi için telefon numaramız:<i>0212 684 75 33</i>";

                await emailService.SendAsync(new IdentityMessage()
                {
                    Body = body,
                    Subject = "Arıza kaydı oluşturuldu."
                }, user.Email);

                var issueLog = new IssueLog()
                {
                    IssueId = issue.Id,
                    Description = "Arıza Kaydı Oluşturuldu.",
                    FromWhom = "Müşteri"
                };
                new IssueLogRepo().Insert(issueLog);

                return RedirectToAction("Index", "Issue");
            }
            catch (DbEntityValidationException ex)
            {
                TempData["Message3"] = new ErrorVM()
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
                TempData["Message2"] = new ErrorVM()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Create",
                    ControllerName = "Issue",
                    ErrorCode = 500
                };
                return RedirectToAction("Error500", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult Survey(string code)
        {
            try
            {
                var surveyRepo = new SurveyRepo();
                var survey = surveyRepo.GetById(code);
                if (survey == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var data = Mapper.Map<Survey, SurveyVM>(survey);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Message2"] = new ErrorVM()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Survey",
                    ControllerName = "Issue",
                    ErrorCode = 500
                };
                return RedirectToAction("Error500", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult Survey(SurveyVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Hata Oluştu.");
                return RedirectToAction("Survey", "Issue", model);
            }
            try
            {
                var surveyRepo = new SurveyRepo();
                var survey = surveyRepo.GetById(model.SurveyId);
                if (survey == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                survey.Pricing = model.Pricing;
                survey.Satisfaction = model.Satisfaction;
                survey.Solving = model.Solving;
                survey.Speed = model.Speed;
                survey.TechPoint = model.TechPoint;
                survey.Suggestions = model.Suggestions;
                survey.IsDone = true;
                surveyRepo.Update(survey);
                TempData["Message2"] = "Anket tamamlandı.";
                return RedirectToAction("UserProfile", "Account");
            }
            catch (Exception ex)
            {
                TempData["Message2"] = new ErrorVM()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Survey",
                    ControllerName = "Issue",
                    ErrorCode = 500
                };
                return RedirectToAction("Error500", "Home");
            }
        }

        [HttpGet]
        [Route("IssueTimeline/{id}")]
        public ActionResult Timeline(string id)
        {
            var data = new IssueLogRepo().GetAll(x => x.IssueId == id).OrderBy(x => x.CreatedDate).ToList();
            if (data == null)
                return RedirectToAction("Details", "Issue", id);
            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult ListAll()
        {
            var data = new IssueRepo().GetAll(x=>x.ClosedDate!=null);
            if (data == null)
                return RedirectToAction("Index", "Home");
            return View(data);
        }
    }
}
