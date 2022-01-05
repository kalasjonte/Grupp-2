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

        // GET: Work_Experience
        public ActionResult Index()
        {
            return View(db.Work_Experiences.ToList());
        }

        // GET: Work_Experience/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work_Experience work_Experience = db.Work_Experiences.Find(id);
            if (work_Experience == null)
            {
                return HttpNotFound();
            }
            return View(work_Experience);
        }

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

        // GET: Work_Experience/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work_Experience work_Experience = db.Work_Experiences.Find(id);
            if (work_Experience == null)
            {
                return HttpNotFound();
            }
            return View(work_Experience);
        }

        // POST: Work_Experience/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkExpID,Titel")] Work_Experience work_Experience)
        {
            if (ModelState.IsValid)
            {
                db.Entry(work_Experience).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(work_Experience);
        }

        // GET: Work_Experience/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work_Experience work_Experience = db.Work_Experiences.Find(id);
            if (work_Experience == null)
            {
                return HttpNotFound();
            }
            return View(work_Experience);
        }

        // POST: Work_Experience/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Work_Experience work_Experience = db.Work_Experiences.Find(id);
            db.Work_Experiences.Remove(work_Experience);
            db.SaveChanges();
            return RedirectToAction("Index");
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
