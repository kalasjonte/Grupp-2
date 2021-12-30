using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Projects_Users> GetProjectUsersFromUserID(int id)
        {
            return db.Projects_Users.Where(pu => pu.UserID == id).ToList(); ;
        }

       public Projects_Users GetProjectUsersByProjectIDAndUserID (int projectID, int userID)
        {
            return db.Projects_Users.Where(pu => pu.ProjectID == projectID && pu.UserID == userID).FirstOrDefault();
        }

        public void DeleteProjectUserByProjectIDAndUserID(int projectID, int userID)
        {
            var delProjekt = this.GetProjectUsersByProjectIDAndUserID(projectID, userID);

            db.Projects_Users.Remove(delProjekt);
            db.SaveChanges();
        }
    }
}
