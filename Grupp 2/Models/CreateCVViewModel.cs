using Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Grupp_2.Models
{
    public class CreateCVViewModel
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string Imgpath { get; set; }

        public int UserID { get; set; }

        public string Email { get; set; }

        public string Adress { get; set; }
        public List<Education> Educations { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Work_Experience> Work_Experiences { get; set; }

        public List<Project> Projects { get; set; }

        public CreateCVViewModel(string name, string path, List<Education> educations, List<Skill> skills, List<Work_Experience> work_Experiences, List<Project> projects )
        {
            User = name;
            Imgpath = path;
            Educations = educations;
            Skills = skills;
            Work_Experiences = work_Experiences;
            Projects = projects;
        }

        public CreateCVViewModel( string path, List<Education> educations, List<Skill> skills, List<Work_Experience> work_Experiences, List<Project> projects)
        {
            User = "Anonym Användare";
            Imgpath = path;
            Educations = educations;
            Skills = skills;
            Work_Experiences = work_Experiences;
            Projects = projects;
        }

        public CreateCVViewModel(List<Education> educations, List<Skill> skills, List<Work_Experience> work_Experiences, List<Project> projects)
        {
            Educations = educations;
            Skills = skills;
            Work_Experiences = work_Experiences;
            Projects = projects;
            
        }

        public CreateCVViewModel(int id, string name, string path, string email, string adress, List<Education> educations, List<Skill> skills, List<Work_Experience> work_Experiences, List<Project> projects)
        {
            UserID = id;
            User = name;
            Imgpath = path;
            Email = email;
            Adress = adress;
            Educations = educations;
            Skills = skills;
            Work_Experiences = work_Experiences;
            Projects = projects;
            
        }
    }
}