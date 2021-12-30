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
            var projects = projectRespository.GetAllProjects();
            var project = projects.Last();
            if (project != null)
            {
                ViewBag.Projectnamn = "Titel: " + project.Titel;
                var creator = userRespository.GetUserByUserID(project.Creator);
                ViewBag.Creator = creator.Firstname;
                ViewBag.ProjId = project.ProjectID;
            }
            var users = userRespository.GetAllUsers();

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
            string loggedInUserMail = User.Identity.Name.ToString();
            User user2 = userRespository.GetUserByEmail(loggedInUserMail);
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