using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TeknikServis.BLL.Helpers;
using TeknikServis.BLL.Repository;
using TeknikServis.BLL.Services.Senders;
using TeknikServis.Models.Entities;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MembershipTools;

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
           var user = NewUserManager().FindById(data.CustomerId);
            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> Create(IssueVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Hata Oluştu.");
                RedirectToAction("Create", "Issue", model);
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
                    PhotoPath = model.PhotoPath,
                    ProductType = model.ProductType,
                    CustomerId = model.CustomerId,
                    PurchasedDate = model.PurchasedDate,
                    ServiceCharge = model.ServiceCharge,
                    ClosedDate = model.ClosedDate,
                    CreatedDate = model.CreatedDate,
                    OperatorId = model.OperatorId,
                    Report = model.Report
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
                if (issue.WarrantyState == true)
                {
                    issue.ServiceCharge = 0;
                }

                if (model.PostedPhoto != null &&
                    model.PostedPhoto.ContentLength > 0)
                {
                    var file = model.PostedPhoto;
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extName = Path.GetExtension(file.FileName);
                    fileName = StringHelpers.UrlFormatConverter(fileName);
                    fileName += StringHelpers.GetCode();
                    var directorypath = Server.MapPath("~/Upload/");
                    var filepath = Server.MapPath("~/Upload/") + fileName + extName;

                    if (!Directory.Exists(directorypath))
                    {
                        Directory.CreateDirectory(directorypath);
                    }

                    file.SaveAs(filepath);

                    WebImage img = new WebImage(filepath);
                    img.Resize(250, 250, false);
                    img.AddTextWatermark("TeknikServis");
                    img.Save(filepath);
                    var oldPath = issue.PhotoPath;
                    issue.PhotoPath = "/Upload/" + fileName + extName;
                    System.IO.File.Delete(Server.MapPath(oldPath));
                }

                var repo = new IssueRepo();

                repo.InsertForMark(issue);
                repo.Save();
                TempData["Message"] = "Arıza kaydınız başarı ile oluşturuldu.";

                var emailService = new EmailService();

                var body = $"Merhaba <b>{user.Name} {user.Surname}</b><br>Arıza kaydınız başarıyla oluşturuldu.Birimlerimiz sorunu çözmek için en kısa zamanda olay yerine intikal edecektir.<br><br> Ayrıntılı bilgi için telefon numaramız:<i>0212 684 75 33</i>";

                await emailService.SendAsync(new IdentityMessage()
                {
                    Body = body,
                    Subject = "Arıza kaydı oluşturuldu."
                }, user.Email);
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
