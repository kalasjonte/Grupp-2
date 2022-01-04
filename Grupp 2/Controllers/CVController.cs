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
using Data.Respositories;
using Grupp_2.Models;

namespace Grupp_2.Controllers
{
    public class CVController : Controller
    {
        private Datacontext db = new Datacontext();
        private CVRespository DBCV = new CVRespository();
        private UserRespository UserRespository = new UserRespository();
        private ProjectRespository ProjectRespository = new ProjectRespository();

        // GET: CV
        public ActionResult Index()
        {
            
            return View(DBCV.GetAllCVS());
        }

        // GET: CV/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CV cV = DBCV.GetCVById(id);
            if (cV == null)
            {
                return HttpNotFound();
            }
            return View(cV);
        }

        // GET: CV/Create
        public ActionResult Create() //används inte va?
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Firstname");
            return View();
        }

        public ActionResult CreateCVVM()
        {
            var files = Directory.GetFiles(Server.MapPath("~/UploadedFiles"));
            int cvId = DBCV.GetCVIDByEmail(User.Identity.Name.ToString());


            var tempCv = DBCV.GetCVById(cvId);
            //var tempCv = db.CVs.Where(e => e.CVID == cvId).FirstOrDefault();

                var image = db.Images.Where(i => i.ImageID == tempCv.ImageID).FirstOrDefault();

                string path = image.Path;
                
            ViewBag.Path = ("/UploadedFiles/") + Path.GetFileName(image.Name);

            //.------------------------

            var workExp = DBCV.GetWorkExpFromCVID(cvId);

            var education = DBCV.GetEducationsFromCVID(cvId);

            var skills = DBCV.GetSkillsFromCVID(cvId);
            // ---------------


            string loggedInUserMail = User.Identity.Name.ToString();

            int userID = UserRespository.GetUserIDByEmail(loggedInUserMail);

            ViewBag.Id = userID;

            var projects = ProjectRespository.GetAllProjects();

            var projectUsers = ProjectRespository.GetProjectUsersFromUserID(userID);

            List<Project> tempList = new List<Project>();

            foreach (var item in projects)
            {
                foreach (var item2 in projectUsers)
                {
                    if (item.ProjectID == item2.ProjectID)
                    {
                        tempList.Add(item);
                    }
                }
            }


            //-------
            var CreateCVViewModel = new CreateCVViewModel
            {
                Educations = education,
                Skills = skills,
                Work_Experiences = workExp,
                Projects = tempList
            };
            var skillsList = new SelectList(DBCV.GetAllSkills(), "SkillID", "Title");
            ViewData["DBMySkills"] = skillsList;

            var workExperience = new SelectList(DBCV.GetAllWorkExps(), "WorkExpID", "Titel");
            ViewData["DBMyWorkExp"] = workExperience;

            var educations = new SelectList(DBCV.GetAllEducations(), "EduID", "Title");
            ViewData["DBMyEducations"] = educations;

            return View(CreateCVViewModel);
    }

        public ActionResult Remove(int id)
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            int userID = UserRespository.GetUserIDByEmail(loggedInUserMail);
            ProjectRespository.DeleteProjectUserByProjectIDAndUserID(id, userID);
           
            return RedirectToAction("CreateCVVM");
        }
        public ActionResult UpdateCvVm(string actionType)
        {
            int cvId = DBCV.GetCVIDByEmail(User.Identity.Name.ToString());
            CV cv = DBCV.GetCVById(cvId);


            if (actionType == "Lägg till färdighet på ditt cv")
            {
                try
                {
                    int skill = Int32.Parse(Request.Form["MySkills"]);
                    DBCV.AddSkillToCV(cv, skill);
                   
                }
                catch(Exception e) { ErrorMessage("Please add a skill to the database before doing this!"); }
                
                return RedirectToAction("CreateCVVM");

            }
            else if (actionType == "Lägg till arbete i cvet")
            {
                try
                {

                    int workxp = Int32.Parse(Request.Form["WorkExp"]);
                    DBCV.AddWorkExpToCV(cv,workxp);
                }
                catch(Exception e) { ErrorMessage("Please add a work experience to the database before doing this!"); }
                return RedirectToAction("CreateCVVM");
            }
            else if (actionType == "Lägg till Utbildning i cvet")
            {
                try
                {

                    int educ = Int32.Parse(Request.Form["MyEducations"]);

                    DBCV.AddEducationToCV(cv, educ);
                }
                catch(Exception e) { ErrorMessage("Please add an education to the database before doing this!"); }

                return RedirectToAction("CreateCVVM");
            }

            else if (actionType.Contains("Ta bort utbildning"))
            {
                
                string utbildning = actionType.Substring(19);
                DBCV.RemoveEducationFromCV(cv, utbildning);

                return RedirectToAction("CreateCVVM");
            }

            else if (actionType.Contains("Ta bort erfarenhet"))
            {
                

                string erfarenhet = actionType.Substring(19);
                DBCV.RemoveSkillFromCV(cv, erfarenhet);

                return RedirectToAction("CreateCVVM");
            }

            else if (actionType.Contains("Ta bort arbete"))
            {
                string arbete = actionType.Substring(15);

                DBCV.RemoveWorkExpFromCV(cv, arbete);

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
            User user = UserRespository.GetUserByEmail(loggedInUserMail); //hämta user på inskickad id ist? -> in i user respository som ligger i data.

            int cvId = DBCV.GetCVIDByEmail(User.Identity.Name.ToString()); //göra ny, ändra till inskickad userid?
            var workExp = DBCV.GetWorkExpFromCVID(cvId);

            var education = DBCV.GetEducationsFromCVID(cvId);

            var skills = DBCV.GetSkillsFromCVID(cvId);
            CV tempCv = DBCV.GetCVById(cvId);

            //------
            var img = db.Images.Where(i => i.ImageID == tempCv.ImageID).FirstOrDefault();

            var projects = ProjectRespository.GetAllProjects();


            var projectUsers = ProjectRespository.GetProjectUsersFromUserID(user.UserID);

            List<Project> tempList = new List<Project>();

            foreach (var item in projects)
            {
                foreach (var item2 in projectUsers)
                {
                    if (item.ProjectID == item2.ProjectID)
                    {
                        tempList.Add(item);
                    }
                }
            }

            var CreateCVViewModel = new CreateCVViewModel //skapa viewmodel i  klassen istället -> släng ut den i shared -> gör om till 2 konstruktörer, en med anonym användare
            {
                User = user.Firstname,
                Imgpath = ("/UploadedFiles/") + Path.GetFileName(img.Name),
                Educations = education,
                Skills = skills,
                Work_Experiences = workExp,
                Projects = tempList
            };

            return View(CreateCVViewModel);
        }

        [Route("Cv/{userid:int}/ShowUserCv", Name = "ShowUserCv")]
        public ActionResult ShowUserCV(int userid)
        {

            User user = UserRespository.GetUserByUserID(userid);
            //ifnotnull
            CV cvet = DBCV.GetCVByUserId(user.UserID);
            int cvId = cvet.CVID;

            var workExp = DBCV.GetWorkExpFromCVID(cvId);

            var education = DBCV.GetEducationsFromCVID(cvId);

            var skills = DBCV.GetSkillsFromCVID(cvId);

            var img = db.Images.Where(i => i.ImageID == cvet.ImageID).FirstOrDefault();

            var projects = ProjectRespository.GetAllProjects();

            var projectUsers = ProjectRespository.GetProjectUsersFromUserID(userid);

            List<Project> tempList = new List<Project>();

            foreach (var item in projects)
            {
                foreach (var item2 in projectUsers)
                {
                    if(item.ProjectID == item2.ProjectID)
                    {
                        tempList.Add(item);
                    }
                }
            }


            var CreateCVViewModel = new CreateCVViewModel
            {
                UserID = user.UserID,
                User = user.Firstname + " " + user.Lastname,
                Email = user.Email,
                Adress = user.Adress,
                Imgpath = ("/UploadedFiles/") + Path.GetFileName(img.Name),
                Educations = education,
                Skills = skills,
                Work_Experiences = workExp,
                Projects = tempList
            };

            return View(CreateCVViewModel);
        }

        // POST: CV/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CVID,UserID,ImgPath")] CV cV) //används inte
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

        
        public ActionResult Edit(int? id) //används inte?
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
        public ActionResult Delete(int? id) //används inte
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

        

        public ActionResult ErrorMessage(string message)
        {
            TempData["alertMessage"] = message;
            return RedirectToAction("CreateCVVM");
        }

        //Bildmetod
        [HttpPost]
        public ActionResult Upload()
        {
            foreach (string file in Request.Files)
            {
                var postedFile = Request.Files[file];
                try
                {
                    postedFile.SaveAs(Server.MapPath("~/UploadedFiles/") + Path.GetFileName(postedFile.FileName));
                    db.Images.Add(new Image { Name = postedFile.FileName, Path = Server.MapPath("~/UploadedFiles/") + Path.GetFileName(postedFile.FileName) });
                    db.SaveChanges();
                }
                catch (Exception e) { ErrorMessage("Please select a file first!");  }

                var imgId = db.Images.OrderByDescending(i => i.ImageID).FirstOrDefault();
                int cvId = DBCV.GetCVIDByEmail(User.Identity.Name.ToString());

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
