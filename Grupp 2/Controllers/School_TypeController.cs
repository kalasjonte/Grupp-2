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
    public class School_TypeController : Controller
    {
        private Datacontext db = new Datacontext();

        // GET: School_Type
        public ActionResult Index()
        {
            return View(db.School_Types.ToList());
        }

        // GET: School_Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School_Type school_Type = db.School_Types.Find(id);
            if (school_Type == null)
            {
                return HttpNotFound();
            }
            return View(school_Type);
        }

        // GET: School_Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: School_Type/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "School_TypeID,Type")] School_Type school_Type)
        {
            if (ModelState.IsValid)
            {
                db.School_Types.Add(school_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(school_Type);
        }

        // GET: School_Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School_Type school_Type = db.School_Types.Find(id);
            if (school_Type == null)
            {
                return HttpNotFound();
            }
            return View(school_Type);
        }

        // POST: School_Type/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "School_TypeID,Type")] School_Type school_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(school_Type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(school_Type);
        }

        // GET: School_Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School_Type school_Type = db.School_Types.Find(id);
            if (school_Type == null)
            {
                return HttpNotFound();
            }
            return View(school_Type);
        }

        // POST: School_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            School_Type school_Type = db.School_Types.Find(id);
            db.School_Types.Remove(school_Type);
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
