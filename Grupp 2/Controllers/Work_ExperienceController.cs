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

namespace Grupp_2.Controllers
{
    public class Work_ExperienceController : Controller
    {
        private Datacontext db = new Datacontext();
        // GET: Work_Experience/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Work_Experience/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkExpID,Titel")] Work_Experience work_Experience)
        {
            Work_Experience tempWe = db.Work_Experiences.Where(x => x.Titel == work_Experience.Titel).FirstOrDefault();

            if (tempWe == null)
            {
                if (ModelState.IsValid)
                {

                    db.Work_Experiences.Add(work_Experience);
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
            }

            else if (tempWe.Titel.ToLower() == work_Experience.Titel.ToLower())

            {
                TempData["alertMessage"] = "Denna arbetserfarenhet existerar redan i systemet!";
                return RedirectToAction("Create");
            }

            return View(work_Experience);
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
