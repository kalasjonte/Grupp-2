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
    public class ProjectController : Controller
    {
        private Datacontext db = new Datacontext();

        
        
        public ActionResult Insert(int id)
        {
            //kod för att inserta i sammansatta tabellen Users_Projects
            
            return RedirectToAction("index");
        }
        
        
        // GET: Project
        public ActionResult Index()
        {

            string loggedInUserMail = User.Identity.Name.ToString();
            User user = db.Users.Where(u => u.Email == loggedInUserMail).FirstOrDefault();

            if (user != null)
            {
                int userId = user.UserID;
                ViewBag.Id = userId;
            }

            //ändra linq mot den sammansatta tabellen istället
            var usersInProjects = db.Users.ToList();
            List<string> allUsers = new List<string>();
            List<string> usersNoPrivate = new List<string>();

            foreach (var item in usersInProjects)
            {
                allUsers.Add(item.Firstname);

                if (item.PrivateProfile == false)
                {
                    usersNoPrivate.Add(item.Firstname);
                }
            }
            //ViewBag med alla users, ska visas när personen som kollar är inloggad
            ViewBag.Users = allUsers;

            //ViewBag med users - alla med privata profiler
            ViewBag.UsersNoPrivate = usersNoPrivate;

            var projects = db.Projects.Include(p => p.User);
            return View(projects.ToList());
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            ViewBag.Creator = new SelectList(db.Users, "UserID", "Firstname");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,Titel,Description,Creator")] Project project)
        {
            if (ModelState.IsValid)
            {
                foreach (var proj in db.Projects) 
                { 
                    if(proj.Titel.ToLower() == project.Titel.ToLower())
                    {
                        return RedirectToAction("DuplicateErrorProj");
                    }
                }
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Creator = new SelectList(db.Users, "UserID", "Firstname", project.Creator);
            return View(project);
        }
        public ActionResult DuplicateErrorProj()
        {
            TempData["alertMessage"] = "Det finns redan ett projekt med denna titel i systemet!";
            return View();
        }


        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.Creator = new SelectList(db.Users, "UserID", "Firstname", project.Creator);
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectID,Titel,Description,Creator")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Creator = new SelectList(db.Users, "UserID", "Firstname", project.Creator);
            return View(project);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
    }
}
