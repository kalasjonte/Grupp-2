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
    public class SkillController : Controller
    {
        private Datacontext db = new Datacontext();

        // GET: Skill
        public ActionResult Index()
        {
            return View(db.Skills.ToList());
        }

        // GET: Skill/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = db.Skills.Find(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            return View(skill);
        }

        // GET: Skill/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Skill/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SkillID,Title")] Skill skill)
        {
            if (ModelState.IsValid)
            {
                db.Skills.Add(skill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(skill);
        }

        // GET: Skill/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = db.Skills.Find(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            return View(skill);
        }

        // POST: Skill/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SkillID,Title")] Skill skill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(skill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(skill);
        }

        // GET: Skill/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = db.Skills.Find(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            return View(skill);
        }

        // POST: Skill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Skill skill = db.Skills.Find(id);
            db.Skills.Remove(skill);
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
