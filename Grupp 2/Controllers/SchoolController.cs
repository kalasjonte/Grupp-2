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
    public class SchoolController : Controller
    {
        private Datacontext db = new Datacontext();

        // GET: School
        public ActionResult Index()
        {
            var schools = db.Schools.Include(s => s.School_Type);
            return View(schools.ToList());
        }

        // GET: School/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = db.Schools.Find(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // GET: School/Create
        public ActionResult Create()
        {
            ViewBag.Type = new SelectList(db.School_Types, "School_TypeID", "Type");
            return View();
        }

        // POST: School/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SchoolID,Name,Place,Type")] School school)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in db.Schools) {
                    if (school.Name.ToLower() == item.Name.ToLower()) {
                        return RedirectToAction("DuplicateErrorSchool");
                    }
                }
                db.Schools.Add(school);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Type = new SelectList(db.School_Types, "School_TypeID", "Type", school.Type);
            return View(school);
        }

        // GET: School/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = db.Schools.Find(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            ViewBag.Type = new SelectList(db.School_Types, "School_TypeID", "Type", school.Type);
            return View(school);
        }

        // POST: School/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SchoolID,Name,Place,Type")] School school)
        {
            if (ModelState.IsValid)
            {
                db.Entry(school).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Type = new SelectList(db.School_Types, "School_TypeID", "Type", school.Type);
            return View(school);
        }

        // GET: School/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = db.Schools.Find(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // POST: School/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            School school = db.Schools.Find(id);
            db.Schools.Remove(school);
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

        public ActionResult DuplicateErrorSchool()
        {
            TempData["alertMessage"] = "Det finns redan en skola med denna titel i systemet!";
            return View();
        }

        public ActionResult CreateSchoolVM()
        {
            var st = db.School_Types.ToList();
            var CreateSchoolViewModel = new CreateSchoolViewModel
            {
                School_Types = st,
            };
            var stList = new SelectList(db.School_Types.ToList(), "School_TypeID", "Type");
            ViewData["DBMySchool_Types"] = stList;
            return View(CreateSchoolViewModel);
        }

        public ActionResult UpdateSchoolVM(string actionType, CreateSchoolViewModel vm)
        {
            if (actionType == "SaveSchool")
            {
                int schoolTypeID = Int32.Parse(Request.Form["MySchool_Types"]);
                if (ModelState.IsValid)
                {
                    foreach (var item in db.Schools)
                    {
                        if (item.Name.ToLower() == vm.SchoolName.ToLower())
                        {
                            return RedirectToAction("DuplicateErrorSchool");
                        }
                    }
                    db.Schools.Add(new Data.Models.School
                    {
                        Name = vm.SchoolName,
                        Place = vm.SchoolPlace,
                        Type = schoolTypeID
                    });
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("CreateSchoolVM");
                }
            }
            else
            {
                return RedirectToAction("CreateSchoolVM");
            }
        }

    }
}
