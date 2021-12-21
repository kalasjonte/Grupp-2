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
    public class CVController : Controller
    {
        private Datacontext db = new Datacontext();

        // GET: CV
        public ActionResult Index()
        {
            var cVs = db.CVs.Include(c => c.User);
            return View(cVs.ToList());
        }

        // GET: CV/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CV cV = db.CVs.Find(id);
            if (cV == null)
            {
                return HttpNotFound();
            }
            return View(cV);
        }

        // GET: CV/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Firstname");
            return View();
        }

        public ActionResult CreateCVVM()
        {
            int cvId = GetLoggedInCvID();
            var workExp = db.Work_Experiences.Where(we => we.CVs.Any(cv => cv.CVID == cvId)).ToList();

            var education = db.Educations.Where(ed => ed.CVs.Any(cv => cv.CVID == cvId)).ToList();

            var skills = db.Skills.Where(s => s.CVs.Any(cv => cv.CVID == cvId)).ToList();

            var CreateCVViewModel = new CreateCVViewModel
            {
                Educations = education,
                Skills = skills,
                Work_Experiences = workExp
            };
            var skillsList = new SelectList(db.Skills.ToList(), "SkillID", "Title");
            ViewData["DBMySkills"] = skillsList;

            var workExperience = new SelectList(db.Work_Experiences.ToList(), "WorkExpID", "Titel");
            ViewData["DBMyWorkExp"] = workExperience;

            var educations = new SelectList(db.Educations.ToList(), "EduID", "Title");
            ViewData["DBMyEducations"] = educations;

            return View(CreateCVViewModel);
    }

        
        public ActionResult UpdateCvVm(string actionType)
        {
            int cvId = GetLoggedInCvID();
            CV cv = db.CVs.Where(s => s.CVID == cvId).FirstOrDefault();


            if (actionType == "Lägg till färdighet på ditt cv")
            {

                int skill = Int32.Parse(Request.Form["MySkills"]);
                var skillAdd = db.Skills.Where(s => s.SkillID == skill).FirstOrDefault();

                System.Diagnostics.Debug.WriteLine(skill);

                cv.Skills.Add(skillAdd);
                db.SaveChanges();

                
                return RedirectToAction("CreateCVVM");

            }
            else if (actionType == "Lägg till arbete i cvet")
            {

                int workxp = Int32.Parse(Request.Form["WorkExp"]);
                var workadd = db.Work_Experiences.Where(s => s.WorkExpID == workxp).FirstOrDefault();
                System.Diagnostics.Debug.WriteLine(workxp);

                cv.Work_Experiences.Add(workadd);
                db.SaveChanges();
                return RedirectToAction("CreateCVVM");
            }
            else if (actionType == "Lägg till Utbildning i cvet")
            {
                int educ = Int32.Parse(Request.Form["MyEducations"]);
                System.Diagnostics.Debug.WriteLine(educ);
                var eduadd = db.Educations.Where(s => s.EduID == educ).FirstOrDefault();
                

                cv.Educations.Add(eduadd);
                db.SaveChanges();

                return RedirectToAction("CreateCVVM");
            }

            else
            {
                return View("Index");
            }

           
        }

        // POST: CV/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CVID,UserID,ImgPath")] CV cV)
        {
            if (ModelState.IsValid)
            {
                db.CVs.Add(cV);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "Firstname", cV.UserID);
            return View(cV);
        }

        // GET: CV/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CV cV = db.CVs.Find(id);
            if (cV == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Firstname", cV.UserID);
            return View(cV);
        }

        // POST: CV/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CVID,UserID,ImgPath")] CV cV)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cV).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Firstname", cV.UserID);
            return View(cV);
        }

        // GET: CV/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CV cV = db.CVs.Find(id);
            if (cV == null)
            {
                return HttpNotFound();
            }
            return View(cV);
        }

        // POST: CV/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CV cV = db.CVs.Find(id);
            db.CVs.Remove(cV);
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

        public int GetLoggedInCvID()
        {
            Datacontext ct = new Datacontext();

            string loggedInUserMail = User.Identity.Name.ToString();
            User user = ct.Users.Where(u => u.Email == loggedInUserMail).FirstOrDefault();
            int userid = user.UserID;

            CV cv = db.CVs.Where(u => u.UserID == userid).FirstOrDefault();
            int id = cv.CVID;
            return id;
        }
    }
}
