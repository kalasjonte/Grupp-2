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

            return RedirectToAction("ProjectVM");
        }
        public ActionResult Remove(int id)
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User user = db.Users.Where(u => u.Email == loggedInUserMail).FirstOrDefault();
            var tempProjekt = db.Projects_Users.Where(pu => pu.ProjectID == id && pu.UserID == user.UserID).FirstOrDefault();

            db.Projects_Users.Remove(tempProjekt);
            db.SaveChanges();

            return RedirectToAction("ProjectVM");
        }



        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        
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
            
            string loggedInUserMail = User.Identity.Name.ToString();
            ViewBag.Creator = new SelectList(db.Users.Where(u => u.Email == loggedInUserMail).ToList(), "UserID", "Firstname");

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
                db.Projects_Users.Add(new Projects_Users { ProjectID = project.ProjectID, UserID = project.Creator });
                db.SaveChanges();
                return RedirectToAction("ProjectVM");
            }

            ViewBag.Creator = new SelectList(db.Users, "UserID", "Firstname", project.Creator);
            return View(project);
        }
        public ActionResult DuplicateErrorProj()
        {
            TempData["alertMessage"] = "Det finns redan ett projekt med denna titel i systemet!";
            return View();
        }


        
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
            string loggedInUserMail = User.Identity.Name.ToString();
            ViewBag.Creator = new SelectList(db.Users.Where(u => u.Email == loggedInUserMail).ToList(), "UserID", "Firstname");
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
                return RedirectToAction("ProjectVM");
            }
            ViewBag.Creator = new SelectList(db.Users, "UserID", "Firstname", project.Creator);
            return View(project);
        }

        

        
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

            var projects_Users = db.Projects_Users.Where(e => e.ProjectID == id);
            foreach (var item in projects_Users)
            {
                db.Projects_Users.Remove(item);
            }

            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("ProjectVM");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult ProjectVM()
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User user = db.Users.Where(u => u.Email == loggedInUserMail).FirstOrDefault();

            if (user != null)
            {
                int userId = user.UserID;
                ViewBag.Id = userId;
            }




            List<Project> projects = new List<Project>();


            projects = db.Projects.Include(p => p.Users).ToList();
            List<ProjectsViewModel> ProjectViews = new List<ProjectsViewModel>();
            //Jontes
            using (var context = new Datacontext())
            {
                foreach (var item in projects)
                {
                    var ProjectsViewModel = new ProjectsViewModel
                    {
                        Creator = item.User.Firstname + " " + item.User.Lastname,
                        CreatorID = item.Creator,
                        Titel = item.Titel,
                        Description = item.Description,
                        ProjectID = item.ProjectID


                    };
                    var projusers = context.Projects_Users.Where(u => u.ProjectID == item.ProjectID).ToList();
                    List<string> namn = new List<string>();
                    List<string> namnNonHidden = new List<string>();

                    foreach (var projects_Users in projusers)
                    {
                        var anv = context.Users.Where(u => u.UserID == projects_Users.UserID).ToList();

                        foreach (var anvItem in anv)
                        {
                            namn.Add(anvItem.Firstname);
                            if (anvItem.PrivateProfile == false)
                            {
                                namnNonHidden.Add(anvItem.Firstname);
                            }
                        }

                    }
                    ProjectsViewModel.Users = namn;
                    ProjectsViewModel.UsersNotHidden = namnNonHidden;

                    ProjectViews.Add(ProjectsViewModel);
                }

                //Jontes slut





                //ViewBag med alla projektId som inloggade användaren är med i
                if (user != null)
                {
                    var userIdCommon = db.Projects_Users.Where(pu => pu.UserID == user.UserID).ToList();
                    List<string> projIds = new List<string>();
                    foreach (var item in userIdCommon)
                    {
                        projIds.Add(item.ProjectID.ToString());

                    }
                    ViewBag.Projects = projIds;
                }


                return View(ProjectViews);
            }
        }
    }
}
