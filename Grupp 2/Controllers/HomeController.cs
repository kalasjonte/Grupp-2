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
            var projects  = db.Projects.ToList() ;
            var project = projects.Last();
            if (project != null)
            {
                ViewBag.Projectnamn = "Titel: " + project.Titel;
                var creator = db.Users.Where(s => s.UserID == project.Creator).FirstOrDefault();
                ViewBag.Creator = creator.Firstname;
            }
            var users = db.Users.ToList();

            if (users.Count() > 0)
            {
                int check = users.Count();
                ViewBag.Count = users.Count();

                List<int> uID = new List<int>();
                for (int i = 0; i < 3 && i < check; i++)
                {
                    var user = users.Last();
                    uID.Add(user.UserID);
                    users.Remove(user);
                }
                ViewBag.UID = uID;
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
           
            return View(db.School_Types.Where(x => x.Type.Contains(searchString) || searchString == null).ToList());
        }
    }
}