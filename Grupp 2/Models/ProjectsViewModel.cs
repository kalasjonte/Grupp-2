using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grupp_2.Models
{
    public class ProjectsViewModel
    {
        
        public string Creator { get; set; }
        public int CreatorID { get; set; }
        public string Titel { get; set; }
        public string Description { get; set; }
        public int ProjectID { get; set; }
        public List<string> Users { get; set; }
        public List<string> UsersNotHidden { get; set; }


        public ProjectsViewModel(string creatorName, int creatorID, string titel, string description, int projectID)
        {
            Creator = creatorName;
            CreatorID = creatorID;
            Titel = titel;
            Description = description;
            ProjectID = projectID;
        }


        public ProjectsViewModel(int creatorID, string titel, string description, int projectID)
        {
            Creator = "Anonym Användare";
            CreatorID = creatorID;
            Titel = titel;
            Description = description;
            ProjectID = projectID;
        }

    }
}
