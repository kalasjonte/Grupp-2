using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data.Respositories
{
    public class ProjectRespository
    {
        private Datacontext db { get; set; }

        public ProjectRespository()
        {
            db = new Datacontext();
        }

        public List<Project> GetAllProjects()
        {
            return db.Projects.ToList();

        }

        public List<Project> GetAllProjectIncludeUser()
        {
            return  db.Projects.Include(p => p.Users).ToList();
        }

        public List<Projects_Users> GetProjectUsersFromUserID(int id)
        {
            return db.Projects_Users.Where(pu => pu.UserID == id).ToList(); ;
        }

       public Projects_Users GetProjectUsersByProjectIDAndUserID (int projectID, int userID)
        {
            return db.Projects_Users.Where(pu => pu.ProjectID == projectID && pu.UserID == userID).FirstOrDefault();
        }
        public List<Projects_Users> GetProjectUsersByProjectID(int id)
        {
            return db.Projects_Users.Where(u => u.ProjectID == id).ToList();
        }

        public void DeleteProjectUserByProjectIDAndUserID(int projectID, int userID)
        {
            var delProjekt = this.GetProjectUsersByProjectIDAndUserID(projectID, userID);

            db.Projects_Users.Remove(delProjekt);
            db.SaveChanges();
        }

        public void AddNewProjectUser(int projectID, int userID)
        {
            db.Projects_Users.Add(new Projects_Users { ProjectID = projectID, UserID = userID });
            db.SaveChanges();
        }

        public void DeleteProjectUser(Projects_Users projects_User)
        {
            db.Projects_Users.Remove(projects_User);
            db.SaveChanges();
        }

        public void AddNewProject(Project project) //skapar även deltagande hos creator
        {
            db.Projects.Add(project);
            db.Projects_Users.Add(new Projects_Users { ProjectID = project.ProjectID, UserID = project.Creator });
            db.SaveChanges();
        }

        public void DeleteProjectById(int id)
        {
            Project project = db.Projects.Find(id);

            var projects_Users = db.Projects_Users.Where(e => e.ProjectID == id); //bryta ännumera?
            foreach (var item in projects_Users)
            {
                db.Projects_Users.Remove(item);
            }

            db.Projects.Remove(project);
            db.SaveChanges();
        }
    }
}
