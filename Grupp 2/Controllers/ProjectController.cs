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

            string loggedInUserMail = User.Identity.Name.ToString();
            User user = db.Users.Where(u => u.Email == loggedInUserMail).FirstOrDefault();

            db.Projects_Users.Add(new Projects_Users { ProjectID = id, UserID = user.UserID });
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Remove(int id)
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User user = db.Users.Where(u => u.Email == loggedInUserMail).FirstOrDefault();
            var tempProjekt = db.Projects_Users.Where(pu => pu.ProjectID == id && pu.UserID == user.UserID).FirstOrDefault();

            db.Projects_Users.Remove(tempProjekt);
            db.SaveChanges();

            return RedirectToAction("Index");
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


            //var userProfiles = _dataContext.UserProfile
            //                   .Where(t => idList.Contains(t.Id));

            //ändra linq mot den sammansatta tabellen istället
            var tempIdList = db.Projects_Users.ToList();
            List<int> idList = new List<int>();
            foreach(var item in tempIdList)
            {
                idList.Add(item.UserID);

            }
            
            var usersInProjects = db.Users.Where(u => idList.Contains(u.UserID)).ToList();
            List<string> allUsers = new List<string>();
            List<string> usersNoPrivate = new List<string>();

            List<int> tempList = new List<int>();

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

            //ViewBag med alla projektId som inloggade användaren är med i
            if (user != null)
            {
                var userIdCommon = db.Projects_Users.Where(pu => pu.UserID == user.UserID).ToList();
                List<string> projIds = new List<string>();
                foreach (var item in userIdCommon)
                {
                    projIds.Add(item.ProjectID.ToString());
                    System.Diagnostics.Debug.WriteLine(item.ProjectID);

                }
                ViewBag.Projects = projIds;
            }

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
