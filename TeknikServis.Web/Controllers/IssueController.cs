using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MembershipTools;

namespace TeknikServis.Web.Controllers
{
    public class IssueController : Controller
    {
        // GET: Issue
        public ActionResult Index()
        {
            return View();
        }

        // GET: Issue/Details/5
        public ActionResult Details(string id)
        {
            return View();
        }

        // GET: Issue/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Issue/Create
        [HttpPost]
        public ActionResult Create(IssueVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Hata Oluştu.");
                RedirectToAction("Create", "Issue",model);
            }

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
