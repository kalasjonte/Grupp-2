using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using Data;
using Data.Models;
using Data.Respositories;
using Grupp_2.Models;

namespace Grupp_2.Controllers
{
    public class CVController : Controller
    {
        //Instansierar datacontext + repos
        private Datacontext db = new Datacontext();

        private CVRespository DBCV = new CVRespository();
        private UserRespository UserRespository = new UserRespository();
        private ProjectRespository ProjectRespository = new ProjectRespository();

        //Metod för att visa "Ditt CV"
        public ActionResult CreateCVVM()
        {
            //Hämtar filer vi har i mappen UploadedFiles
            var files = Directory.GetFiles(Server.MapPath("~/UploadedFiles"));
            

            int cvId = DBCV.GetCVIDByEmail(User.Identity.Name.ToString());
            var tempCv = DBCV.GetCVById(cvId);
            
            var image = db.Images.Where(i => i.ImageID == tempCv.ImageID).FirstOrDefault();
            string path = image.Path;

                ViewBag.Path = ("/UploadedFiles/") + Path.GetFileName(image.Name);
               

                var workExp = DBCV.GetWorkExpFromCVID(cvId);
                var education = DBCV.GetEducationsFromCVID(cvId);
                var skills = DBCV.GetSkillsFromCVID(cvId);

            string loggedInUserMail = User.Identity.Name.ToString();
            int userID = UserRespository.GetUserIDByEmail(loggedInUserMail);
            ViewBag.Id = userID;

            var projects = ProjectRespository.GetAllProjects();
            var projectUsers = ProjectRespository.GetProjectUsersFromUserID(userID);


            //Hämtar projekt som användare är med i (inkl själv skapade projekt)
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

            //Skapar ViewModel för vyn
            var CreateCVViewModel = new CreateCVViewModel(education, skills, workExp, tempList);

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

        //Metod för att uppdatera i "Ditt CV" (Lägga till / ta bort Färdigheter, Utbildningar och Arbetserfarenheter)
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
                catch (Exception e) { ErrorMessage("Please add a skill to the database before doing this!"); }

                return RedirectToAction("CreateCVVM");

            }
            else if (actionType == "Lägg till arbete i cvet")
            {
                try
                {
                    int workxp = Int32.Parse(Request.Form["WorkExp"]);
                    DBCV.AddWorkExpToCV(cv, workxp);
                }
                catch (Exception e) { ErrorMessage("Please add a work experience to the database before doing this!"); }
                return RedirectToAction("CreateCVVM");
            }
            else if (actionType == "Lägg till Utbildning i cvet")
            {
                try
                {
                    int educ = Int32.Parse(Request.Form["MyEducations"]);
                    DBCV.AddEducationToCV(cv, educ);
                }
                catch (Exception e) { ErrorMessage("Please add an education to the database before doing this!"); }

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
                return RedirectToAction("CreateCVVM");
            }
        }

        public ActionResult ShowCVVM() 
        {
            string loggedInUserMail = User.Identity.Name.ToString(); 
            User user = UserRespository.GetUserByEmail(loggedInUserMail); 

            int cvId = DBCV.GetCVIDByEmail(User.Identity.Name.ToString()); 
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
            string path = ("/UploadedFiles/") + Path.GetFileName(img.Name);
            string namn = user.Firstname + " " + user.Lastname;

            var CreateCVViewModel = new CreateCVViewModel(namn, path, education, skills, workExp, tempList); 

            

            return View(CreateCVViewModel);
        }

        [Route("Cv/{userid:int}/ShowUserCv", Name = "ShowUserCv")]
        public ActionResult ShowUserCV(int userid)
        {
            User user = UserRespository.GetUserByUserID(userid);
            
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
                    if (item.ProjectID == item2.ProjectID)
                    {
                        tempList.Add(item);
                    }
                }
            }

            DBCV.AddClick(cvet);
            string path = ("/UploadedFiles/") + Path.GetFileName(img.Name);
            string namn = user.Firstname + " " + user.Lastname;
            CreateCVViewModel model =  new CreateCVViewModel(user.UserID, namn, path, user.Email, user.Adress, education, skills, workExp, tempList, user.GithubUsername);

            if (!User.Identity.IsAuthenticated) //Om icke inloggad => displayar privata användare som "Anonym användare"
            {
                foreach (var item in model.Projects)
                {
                    if (item.User.PrivateProfile == true)
                    {
                        item.User.Firstname = "Anonym användare";
                    }
                }

            }
            System.Diagnostics.Debug.WriteLine(model.Imgpath);

            return View(model);
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

        //Metod för att ladda upp bilder i "Ditt CV"
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
                catch (Exception e) { ErrorMessage("Please select a file first!"); }

                var imgId = db.Images.OrderByDescending(i => i.ImageID).FirstOrDefault();
                int cvId = DBCV.GetCVIDByEmail(User.Identity.Name.ToString());

                var tempCv = from c in db.CVs
                             where c.CVID == cvId
                             select c;

                foreach (CV c in tempCv)
                {
                    c.ImageID = imgId.ImageID;
                }

                db.SaveChanges();
            }
            return RedirectToAction("CreateCVVM");
        }

        public ActionResult MatchUser(int id) //förfina
        {
            CV cv = DBCV.GetCVByUserId(id);
            string search = null;

            foreach (var item in cv.Skills)
            {
                search += item.Title + " ";
            }

            foreach (var item in cv.Work_Experiences)
            {
                search += item.Titel + " ";
            }

            foreach (var item in cv.Educations)
            {
                search += item.Title + " ";
            }

            


            List<User> users = UserRespository.GetUsersByStringVG(search); // ta bort alla som är anonyma
            User removeMe = UserRespository.GetUserByUserID(id);
            users.Remove(removeMe);
            Random random = new Random();
            int getThis = random.Next(0, users.Count());
            User user = users[getThis];
            

            return RedirectToAction("ShowUserCV", new { userid = user.UserID });

        }

        public ActionResult ExportXML(int id)  
        {
            string path = ControllerContext.HttpContext.Server.MapPath("~/UploadedFiles/");
            User user = UserRespository.GetUserByUserID(id);

            CreateCVViewModel CCVM = new CreateCVViewModel();
            CCVM = CCVM.CreateCVViewModelByUserId(id);

            


            FileStream textWriter = new FileStream(path + CCVM.User + ".xml" , FileMode.OpenOrCreate);
            DataContractSerializer serializer = new DataContractSerializer(typeof(CreateCVViewModel));
            

            serializer.WriteObject(textWriter, CCVM);
            textWriter.Close();
            return RedirectToAction("ShowUserCV", new { userid = CCVM.UserID });
        }

    }
}
