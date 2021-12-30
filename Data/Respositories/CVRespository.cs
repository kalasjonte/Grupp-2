using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using System.Data.Entity;

namespace Data.Respositories
{
    public class CVRespository
    {
        public Datacontext db { get; set; }

        public CVRespository()
        {
            db = new Datacontext();
        }

        public void CreateCV(int id)
        {
            db.CVs.Add(new Data.Models.CV 
            { 
                UserID = id,
                ImageID = 5 
            });
            db.SaveChanges();
        }

        public List<CV> GetAllCVS()
        {
            return db.CVs.Include(c => c.User).ToList(); 
            
        }

        public CV GetCVById(int? id)
        {
            if(id == null)
            {
                //trycatch
            }
            return db.CVs.Find(id);
        }

        public CV GetCVByUserId(int userID)
        {
           
            return db.CVs.Where(u => u.UserID == userID).FirstOrDefault();
        }

        public int GetCVIDByEmail(string email) //-> KAN finnas här eller service, respository
        {

            string loggedInUserMail = email;

            UserRespository userRespository = new UserRespository();
            int userid = userRespository.GetUserIDByEmail(loggedInUserMail);

            CV cv = db.CVs.Where(u => u.UserID == userid).FirstOrDefault();
            int id = cv.CVID;
            return id;
        }

        public List<Work_Experience> GetWorkExpFromCVID(int cvId)
        {
            return db.Work_Experiences.Where(we => we.CVs.Any(cv => cv.CVID == cvId)).ToList();
        }

        public List<Education> GetEducationsFromCVID(int cvId)
        {
            return db.Educations.Where(we => we.CVs.Any(cv => cv.CVID == cvId)).ToList();
        }

        public List<Skill> GetSkillsFromCVID(int cvId)
        {
            return db.Skills.Where(we => we.CVs.Any(cv => cv.CVID == cvId)).ToList();
        }

        public List<Skill> GetAllSkills()
        {
            return db.Skills.ToList();
        }

        public List<Work_Experience> GetAllWorkExps()
        {
            return db.Work_Experiences.ToList();
        }

        public List<Education> GetAllEducations()
        {
            return db.Educations.ToList();
        }

        public void AddSkillToCV(CV cv, int skillID)
        {
            var skillAdd = db.Skills.Where(s => s.SkillID == skillID).FirstOrDefault();
            cv.Skills.Add(skillAdd);
            db.SaveChanges();
        }

        public void AddWorkExpToCV(CV cv, int workExpID)
        {
            var workadd = db.Work_Experiences.Where(s => s.WorkExpID == workExpID).FirstOrDefault();


            cv.Work_Experiences.Add(workadd);
            db.SaveChanges();
        }

        public void AddEducationToCV(CV cv, int educationID)
        {
            var eduadd = db.Educations.Where(s => s.EduID == educationID).FirstOrDefault();


            cv.Educations.Add(eduadd);
            db.SaveChanges();
        }

        public void RemoveEducationFromCV(CV cv, string utbildning)
        {
            var edudel = db.Educations.Where(s => s.Title.Equals(utbildning)).FirstOrDefault();

            cv.Educations.Remove(edudel);
            db.SaveChanges();
        }

        public void RemoveSkillFromCV(CV cv, string erfarenhet)
        {
            var skillDel = db.Skills.Where(s => s.Title.Equals(erfarenhet)).FirstOrDefault();

            cv.Skills.Remove(skillDel);
            db.SaveChanges();
        }

        public void RemoveWorkExpFromCV(CV cv, string arbete)
        {
            var workDel = db.Work_Experiences.Where(s => s.Titel.Equals(arbete)).FirstOrDefault();

            cv.Work_Experiences.Remove(workDel);
            db.SaveChanges();
        }
    }
}
