using Data;
using Data.Models;
using Data.Respositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Grupp_2.Models
{
    [DataContract]
    [KnownType(typeof(string))]
    [KnownType(typeof(int))]
    [KnownType(typeof(List<Education>))]
    [KnownType(typeof(List<Skill>))]
    [KnownType(typeof(List<Work_Experience>))]
    [KnownType(typeof(Education))]
    [KnownType(typeof(Project))]
    [KnownType(typeof(Work_Experience))]
    [KnownType(typeof(Skill))]
    [Serializable]
    public class CreateCVViewModel 
    {
        [Required]
        [DataMember]
        public string User { get; set; }
        [Required]
        [DataMember]
        public string Imgpath { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Adress { get; set; }
        [DataMember]
        public List<Education> Educations { get; set; }
        [DataMember]
        public List<Skill> Skills { get; set; }
        [DataMember]
        public List<Work_Experience> Work_Experiences { get; set; }
        [DataMember]
        public List<Project> Projects { get; set; }

        public CreateCVViewModel(string name, string path, List<Education> educations, List<Skill> skills, List<Work_Experience> work_Experiences, List<Project> projects)
        {
            User = name;
            Imgpath = path;
            Educations = educations;
            Skills = skills;
            Work_Experiences = work_Experiences;
            Projects = projects;
        }

        public CreateCVViewModel(string path, List<Education> educations, List<Skill> skills, List<Work_Experience> work_Experiences, List<Project> projects)
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


        


        public CreateCVViewModel CreateCVViewModelByUserId(int userid)
        {
            Datacontext db = new Datacontext();
            db.Configuration.ProxyCreationEnabled = false; //behövs för serializering för annars får vi ett proxyobjekt (alla siffror / nummer) som serialiserarn inte förväntar sig

            UserRespository userRespository = new UserRespository();

            userRespository.db.Configuration.ProxyCreationEnabled = false;
            CVRespository cVRespository = new CVRespository();

            cVRespository.db.Configuration.ProxyCreationEnabled = false;

            ProjectRespository projectRespository = new ProjectRespository();
            projectRespository.db.Configuration.ProxyCreationEnabled = false;

            User user = userRespository.GetUserByUserID(userid);

            CV cvet = cVRespository.GetCVByUserId(user.UserID);
            int cvId = cvet.CVID;

            var workExp = cVRespository.GetWorkExpFromCVID(cvId);

            var education = cVRespository.GetEducationsFromCVID(cvId);

            var skills = cVRespository.GetSkillsFromCVID(cvId);

            var img = db.Images.Where(i => i.ImageID == cvet.ImageID).FirstOrDefault();

            var projects = projectRespository.GetAllProjects();

            var projectUsers = projectRespository.GetProjectUsersFromUserID(userid);

            List<Project> tempList = new List<Project>();

            foreach (var item in projects)
            {
                foreach (var item2 in projectUsers)
                {
                    if (item.ProjectID == item2.ProjectID)
                    {
                        tempList.Add(item);
                    }
                }
            }

            string path = img.Name;
            string namn = user.Firstname + " " + user.Lastname;
            CreateCVViewModel model = new CreateCVViewModel(user.UserID, namn, path, user.Email, user.Adress, education, skills, workExp, tempList);
            return model;
        }

        public CreateCVViewModel()
        {

        }
    }
}