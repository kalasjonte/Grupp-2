using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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

            var uploadedFiles = new List<Image>();

            var files = Directory.GetFiles(Server.MapPath("~/UploadedFiles"));
            int cvId = GetLoggedInCvID();

            foreach (var file in files)
            {

                var picture = new Image() { Name = Path.GetFileName(file) };
                var cvs = from c in db.CVs
                           where c.CVID == cvId
                           select c;

                foreach (CV c in cvs)
                {
                    picture.Path = ("~/UploadedFiles/") + Path.GetFileName(file);
                    uploadedFiles.Add(picture);



                    //var test = from i in db.Images
                    //           where c.CVID == cvId
                    //           select c;


                    ViewBag.Path = picture.Path;
                }
            }


            //.------------------------
            
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

                

                cv.Skills.Add(skillAdd);
                db.SaveChanges();

                
                return RedirectToAction("CreateCVVM");

            }
            else if (actionType == "Lägg till arbete i cvet")
            {

                int workxp = Int32.Parse(Request.Form["WorkExp"]);
                var workadd = db.Work_Experiences.Where(s => s.WorkExpID == workxp).FirstOrDefault();
                

                cv.Work_Experiences.Add(workadd);
                db.SaveChanges();
                return RedirectToAction("CreateCVVM");
            }
            else if (actionType == "Lägg till Utbildning i cvet")
            {
                int educ = Int32.Parse(Request.Form["MyEducations"]);
                
                var eduadd = db.Educations.Where(s => s.EduID == educ).FirstOrDefault();
                

                cv.Educations.Add(eduadd);
                db.SaveChanges();

                return RedirectToAction("CreateCVVM");
            }

            else if (actionType.Contains("Ta bort utbildning"))
            {
                
                string utbildning = actionType.Substring(19);
                var edudel = db.Educations.Where(s => s.Title.Equals(utbildning)).FirstOrDefault();

                cv.Educations.Remove(edudel);
                db.SaveChanges();

                return RedirectToAction("CreateCVVM");
            }

            else if (actionType.Contains("Ta bort erfarenhet"))
            {
                

                string erfarenhet = actionType.Substring(19);
                var skillDel = db.Skills.Where(s => s.Title.Equals(erfarenhet)).FirstOrDefault();

                cv.Skills.Remove(skillDel);
                db.SaveChanges();

                return RedirectToAction("CreateCVVM");
            }

            else if (actionType.Contains("Ta bort arbete"))
            {
                string arbete = actionType.Substring(15);

                var workDel = db.Work_Experiences.Where(s => s.Titel.Equals(arbete)).FirstOrDefault();

                cv.Work_Experiences.Remove(workDel);
                db.SaveChanges();

                return RedirectToAction("CreateCVVM");
            }

            else
            {
                return View("Index");
            }

           
        }

        public ActionResult ShowCVVM() //bryt ut till service, ha denna här men contenten i services
        {
            string loggedInUserMail = User.Identity.Name.ToString(); //KAN finnas här, eller ligga i services med hhtpcontext current (owin)
            User user = db.Users.Where(u => u.Email == loggedInUserMail).FirstOrDefault(); //hämta user på inskickad id ist? -> in i user respository som ligger i data.

            int cvId = GetLoggedInCvID(); //göra ny, ändra till inskickad userid?
            var workExp = db.Work_Experiences.Where(we => we.CVs.Any(cv => cv.CVID == cvId)).ToList();

            var education = db.Educations.Where(ed => ed.CVs.Any(cv => cv.CVID == cvId)).ToList();

            var skills = db.Skills.Where(s => s.CVs.Any(cv => cv.CVID == cvId)).ToList();

            var CreateCVViewModel = new CreateCVViewModel //skapa viewmodel i  klassen istället -> släng ut den i shared
            {
                Användare = user.Firstname,
                imgpath = "",
                Educations = education,
                Skills = skills,
                Work_Experiences = workExp
            };

            return View(CreateCVViewModel);
        }
        [Route("Cv/{userid:int}/ShowUserCv", Name = "ShowUserCv")]
        public ActionResult ShowUserCV(int userid)
        {

            User user = db.Users.Where(u => u.UserID == userid).FirstOrDefault();
            //ifnotnull
            CV cvet = db.CVs.Where(u => u.UserID == user.UserID).FirstOrDefault();
            int cvId = cvet.CVID;

            var workExp = db.Work_Experiences.Where(we => we.CVs.Any(cv => cv.CVID == cvId)).ToList();

            var education = db.Educations.Where(ed => ed.CVs.Any(cv => cv.CVID == cvId)).ToList();

            var skills = db.Skills.Where(s => s.CVs.Any(cv => cv.CVID == cvId)).ToList();

            var CreateCVViewModel = new CreateCVViewModel
            {
                Användare = user.Firstname,
                imgpath = "",
                Educations = education,
                Skills = skills,
                Work_Experiences = workExp
            };

            return View(CreateCVViewModel);
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

        public int GetLoggedInCvID() //-> KAN finnas här eller service, respository
        {
            
            string loggedInUserMail = User.Identity.Name.ToString();
            User user = db.Users.Where(u => u.Email == loggedInUserMail).FirstOrDefault();
            int userid = user.UserID;

            CV cv = db.CVs.Where(u => u.UserID == userid).FirstOrDefault();
            int id = cv.CVID;
            return id;
        }
        //Bildmetod
        [HttpPost]
        public ActionResult Upload()
        {
            foreach (string file in Request.Files)
            {
                var postedFile = Request.Files[file];
                postedFile.SaveAs(Server.MapPath("~/UploadedFiles/") + Path.GetFileName(postedFile.FileName));

                db.Images.Add(new Image { Name = postedFile.FileName, Path = Server.MapPath("~/UploadedFiles/") + Path.GetFileName(postedFile.FileName) });
                db.SaveChanges();

                //ViewBag.Path = Server.MapPath("~/UploadedFiles/") + Path.GetFileName(postedFile.FileName);
                //System.Diagnostics.Debug.WriteLine(ViewBag.Path);

                var imgId = db.Images.OrderByDescending(i => i.ImageID).FirstOrDefault();
                int cvId = GetLoggedInCvID();

                var tempCv = from c in db.CVs
                             where c.CVID == cvId
                             select c;

                foreach(CV c in tempCv)
                {
                    c.ImageID = imgId.ImageID;
                }

                db.SaveChanges();
            }
            return RedirectToAction("CreateCVVM");
        }
    }
}
