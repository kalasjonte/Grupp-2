using Data;
using Data.Models;
using Data.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grupp_2.Controllers
{
    public class HomeController : Controller
    {
        Datacontext db = new Datacontext();
        private ProjectRespository projectRespository = new ProjectRespository();
        private UserRespository userRespository = new UserRespository();
        public ActionResult Index()
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User user2 = userRespository.GetUserByEmail(loggedInUserMail);

            List<Project> projects = projectRespository.GetAllProjects();
            ViewBag.test = projects.Count(); //gör if sats säger noob jonte

            ViewBag.User2Id = null;
            if (user2 != null)
            {
                
                ViewBag.User2Id = user2.UserID;
                var userIdCommon = db.Projects_Users.Where(pu => pu.UserID == user2.UserID).ToList();
                List<string> projIds = new List<string>();

                foreach (var item in userIdCommon)
                {
                    projIds.Add(item.ProjectID.ToString());

                }

                ViewBag.Projects = projIds;

                
            }
            
            
                
                if (projects.Count() > 0)
                {
                    var project = projects.Last();
                    ViewBag.Projectnamn = "Titel: " + project.Titel;
                    var creator = userRespository.GetUserByUserID(project.Creator);

                    if (user2 == null && creator.PrivateProfile == true)
                    {
                        ViewBag.Creator = "Anonym Användare";
                    }
                    else
                    {
                        ViewBag.Creator = creator.Firstname;
                        ViewBag.CreatorId = creator.UserID;
                        ViewBag.ProjId = project.ProjectID;
                    }
                }



                List<User> users = null;
                if (user2 == null)
                {
                    users = userRespository.GetAllUserNotPrivate();
                }
                else
                {
                    users = userRespository.GetAllUsers();
                }

                if (users.Count() > 0)
                {
                    int check = users.Count();
                    ViewBag.Count = users.Count();

                    List<int> uID = new List<int>();
                    List<string> name = new List<string>();
                    for (int i = 0; i < 3 && i < check; i++)
                    {
                        var user = users.Last();
                        uID.Add(user.UserID);
                        name.Add(user.Firstname + " " + user.Lastname);
                        users.Remove(user);
                    }
                    ViewBag.UID = uID;
                    ViewBag.Name = name;
                }
                //----------------------------------------------------------------------------------------
                

                return View();
            

        }

        

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search(string searchString)
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User user = userRespository.GetUserByEmail(loggedInUserMail);

            if (user != null)
            {
                int userId = user.UserID;
                ViewBag.Id = userId;
            }


            return View(userRespository.GetUsersByString(searchString));
        }

        public ActionResult JoinProject(int id)
        {

            string loggedInUserMail = User.Identity.Name.ToString();
            User user = userRespository.GetUserByEmail(loggedInUserMail);

            projectRespository.AddNewProjectUser(id, user.UserID);

            return RedirectToAction("Index");
        }
        public ActionResult LeaveProject(int id)
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User user = userRespository.GetUserByEmail(loggedInUserMail);
            var tempProjekt = projectRespository.GetProjectUsersByProjectIDAndUserID(id, user.UserID);

            projectRespository.DeleteProjectUser(tempProjekt);

            return RedirectToAction("Index");
        }
    }

}