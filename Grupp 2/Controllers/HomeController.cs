﻿using Data;
using Data.Models;
using Data.Respositories;
using System.Collections.Generic;
using System.Linq;
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
            User loggedInUser = userRespository.GetUserByEmail(loggedInUserMail);
            
            List<Project> projects = projectRespository.GetAllProjects();
            ViewBag.ProjectCount = projects.Count(); //gör if sats säger noob jonte
            
            ViewBag.LoggedInUserId = null;
            if (loggedInUser != null)
            {
                ViewBag.LoggedInUserId = loggedInUser.UserID;
                var userIdInProjects = db.Projects_Users.Where(pu => pu.UserID == loggedInUser.UserID).ToList();
                List<string> projIds = new List<string>();

                foreach (var item in userIdInProjects)
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

                if (loggedInUser == null && creator.PrivateProfile == true)
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
            if (loggedInUser == null)
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