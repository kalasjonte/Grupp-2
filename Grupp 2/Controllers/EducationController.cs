using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Models;
using Grupp_2.Models;

namespace Grupp_2.Controllers
{
    public class EducationController : Controller
    {
        private Datacontext db = new Datacontext();

        public ActionResult Index()
        {
            return View(db.Educations.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EduID,Title")] Education education)
        {
            Education tempEdu = db.Educations.Where(x => x.Title == education.Title).FirstOrDefault();

            if (tempEdu == null)
            {
                if (ModelState.IsValid)
                {
                    db.Educations.Add(education);
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
            }

            else if (tempEdu.Title.ToLower() == education.Title.ToLower())
            {
                TempData["alertMessage"] = "Denna utbildning existerar redan i systemet!";
                return RedirectToAction("Create");
            }
            return View(education);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
