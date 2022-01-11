using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Data;
using Data.Models;
using Data.Respositories;
using Grupp_2.Models;

namespace Grupp_2.Controllers
{
    public class ProjectController : Controller
    {
        private Datacontext db = new Datacontext();
        private UserRespository userRespository = new UserRespository();
        private ProjectRespository ProjectRespository = new ProjectRespository();

        public ActionResult Insert(int id)
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User user = userRespository.GetUserByEmail(loggedInUserMail);

            ProjectRespository.AddNewProjectUser(id, user.UserID);

            return RedirectToAction("ProjectVM");
        }

        public ActionResult Remove(int id)
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User user = userRespository.GetUserByEmail(loggedInUserMail);
            var tempProjekt = ProjectRespository.GetProjectUsersByProjectIDAndUserID(id, user.UserID);

            ProjectRespository.DeleteProjectUser(tempProjekt);

            return RedirectToAction("ProjectVM");
        }

        public ActionResult Create()
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            ViewBag.Creator = new SelectList(db.Users.Where(u => u.Email == loggedInUserMail).ToList(), "UserID", "Firstname");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,Titel,Description,Creator")] Project project)
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            ViewBag.Creator = new SelectList(db.Users.Where(u => u.Email == loggedInUserMail).ToList(), "UserID", "Firstname");
            if (ModelState.IsValid)
            { 
                if (!string.IsNullOrWhiteSpace(project.Description) && !string.IsNullOrWhiteSpace(project.Titel))
                {
                    if (!ProjectRespository.TitelExists(project.Titel))
                    {

                        ProjectRespository.AddNewProject(project);

                        return RedirectToAction("ProjectVM");
                        
                    }
                    else
                    {
                        TempData["alertMessage"] = "Finns redan ett projekt med den titeln!";
                        return View(project);
                    }
                }

                else
                {
                    TempData["alertMessage"] = "Fyll i titel samt beskrivning!";
                    return View(project);

                }
            }
            ViewBag.Creator = new SelectList(userRespository.GetAllUsers(), "UserID", "Firstname", project.Creator);
            return View(project);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectID,Titel,Description,Creator")] Project project)
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            ViewBag.Creator = new SelectList(db.Users.Where(u => u.Email == loggedInUserMail).ToList(), "UserID", "Firstname");

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(project.Description) && !string.IsNullOrWhiteSpace(project.Titel))
                {
                    if (!ProjectRespository.TitelExists(project.Titel))
                    {
                    
                        db.Entry(project).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("ProjectVM");
                    }
                    else
                    {
                        TempData["alertMessage"] = "Finns redan ett projekt med den titeln!";
                        return View(project);
                    }
                }

                else
                {
                    TempData["alertMessage"] = "Fyll i titel samt beskrivning!";
                    return View(project);
                    
                }
            }
               
                
            
            ViewBag.Creator = new SelectList(userRespository.GetAllUsers(), "UserID", "Firstname", project.Creator);
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectRespository.DeleteProjectById(id);
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
            User user = userRespository.GetUserByEmail(loggedInUserMail);

            if (user != null)
            {
                int userId = user.UserID;
                ViewBag.Id = userId;
            }
            List<Project> projects = new List<Project>();

            projects = ProjectRespository.GetAllProjectIncludeUser();
            List<ProjectsViewModel> ProjectViews = new List<ProjectsViewModel>();
            
            using (var context = new Datacontext())
            {
                foreach (var item in projects)
                {
                    ProjectsViewModel ProjectsViewModel = null;
                    //Om användare inte är inloggad, namnger projektets skapare som anonym användare
                    if (user == null && item.User.PrivateProfile == true)
                    {
                        ProjectsViewModel = new ProjectsViewModel(item.Creator, item.Titel, item.Description, item.ProjectID);
                    }
                    //Displayar allas namn
                    else
                    {
                        string creatorname = item.User.Firstname + " " + item.User.Lastname;
                        ProjectsViewModel = new ProjectsViewModel(creatorname, item.Creator, item.Titel, item.Description, item.ProjectID);
                    }
                    var projusers = ProjectRespository.GetProjectUsersByProjectID(item.ProjectID);
                    List<string> namn = new List<string>();
                    List<string> namnNonHidden = new List<string>();

                    //Separerar namnlistor i privata och ickeprivata namn
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

                //ViewBag med alla projektId som inloggade användaren är med i
                if (user != null)
                {
                    var userIdCommon = ProjectRespository.GetProjectUsersFromUserID(user.UserID);
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
