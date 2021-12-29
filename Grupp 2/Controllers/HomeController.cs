using Data;
using Data.Models;
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
        public ActionResult Index()
        {
            var projects = db.Projects.ToList();
            var project = projects.Last();
            if (project != null)
            {
                ViewBag.Projectnamn = "Titel: " + project.Titel;
                var creator = db.Users.Where(s => s.UserID == project.Creator).FirstOrDefault();
                ViewBag.Creator = creator.Firstname;
                ViewBag.ProjId = project.ProjectID;
            }
            var users = db.Users.ToList();

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
            User user2 = db.Users.Where(u => u.Email == loggedInUserMail).FirstOrDefault();
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
            User user = db.Users.Where(u => u.Email == loggedInUserMail).FirstOrDefault();

            if (user != null)
            {
                int userId = user.UserID;
                ViewBag.Id = userId;
            }


            return View(db.Users.Where(x => x.Firstname.Contains(searchString) || searchString == null).ToList());
        }

        public ActionResult JoinProject(int id)
        {

            string loggedInUserMail = User.Identity.Name.ToString();
            User user = db.Users.Where(u => u.Email == loggedInUserMail).FirstOrDefault();

            db.Projects_Users.Add(new Projects_Users { ProjectID = id, UserID = user.UserID });
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult LeaveProject(int id)
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User user = db.Users.Where(u => u.Email == loggedInUserMail).FirstOrDefault();
            var tempProjekt = db.Projects_Users.Where(pu => pu.ProjectID == id && pu.UserID == user.UserID).FirstOrDefault();

            db.Projects_Users.Remove(tempProjekt);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}