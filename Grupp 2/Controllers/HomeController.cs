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
            return View();
        }

        public ActionResult Project()
        {
            ViewBag.Message = "Projekt som är inlagda i systemet.";

            return View(db.Projects);
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