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

        // GET: Education
        public ActionResult Index()
        {
            return View(db.Educations.ToList());
        }

        // GET: Education/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        // GET: Education/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Education/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EduID,Title")] Education education)
        {
            if (ModelState.IsValid)
            {
                foreach(var item in db.Educations)
                {
                    if(item.Title.ToLower() == education.Title.ToLower())
                    {
                        return RedirectToAction("DuplicateErrorEdu");
                    }
                }
                db.Educations.Add(education);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(education);
        }

        public ActionResult CreateEduVM()
        {
            var st = db.School_Types.ToList();
            
            var CreateEduViewModel = new CreateEduViewModel
            {
                School_Types = st,
            };

            var stList = new SelectList(db.School_Types.ToList(), "School_TypeID", "Type");
            ViewData["DBMySchool_Types"] = stList;


            return View(CreateEduViewModel);
        }

        //public ActionResult UpdateEduVM(string actionType, CreateEduViewModel vm)
        //{
            
        //    if (actionType == "SaveSchool")
        //    {


        //        int stID = Int32.Parse(Request.Form["MySchool_Types"]); //ger id på skoltyp
                
        //        int stID = Int32.Parse(); //ger id på skola

        //        var stAdd = db.School_Types.Where(s => s.School_TypeID == stID).FirstOrDefault(); //hämtar objekt
                
        //        if (ModelState.IsValid) {
        //            string lolol = vm.SchoolName;
        //            foreach (var item in db.Schools)
        //            {
        //                if (item.Name.ToLower() == lolol.ToLower())
        //                {
        //                    return RedirectToAction("DuplicateErrorEdu");
        //                }
        //            }


                    
        //            db.Schools.Add(new Data.Models.School {Name = vm.SchoolName, Place = //lägg till denna i mvc, stID  });
        //            db.SaveChanges();
        //            return RedirectToAction("Index");

        //        }

                

        //        CV cv = db.CVs.Where(s => s.CVID == 2).FirstOrDefault();


        //        //cv.Skills.Add(skillAdd);
        //        //db.SaveChanges();


        //        return RedirectToAction("Index");

        //    }
        //    //else if (actionType == "Save and Close")
        //    //{
        //    //    // Save and quit action
        //    //}
        //    //else
        //    //{
        //    //    // Cancel action
        //    //}

        //    else
        //    {
        //        return View("Index");
        //    }


        //}



        public ActionResult DuplicateErrorEdu()
        {
            TempData["alertMessage"] = "Det finns redan en utbildning med denna titel i systemet!";
            return View();
        }

        // GET: Education/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        // POST: Education/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EduID,Title")] Education education)
        {
            if (ModelState.IsValid)
            {
                db.Entry(education).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(education);
        }

        // GET: Education/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        // POST: Education/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Education education = db.Educations.Find(id);
            db.Educations.Remove(education);
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
