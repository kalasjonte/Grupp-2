using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Respositories
{
    public class UserRespository
    {
        public Datacontext db { get; set; }

        public UserRespository()
        {
            db = new Datacontext();
        }

        public List<User> GetAllUsers()
        {
            return db.Users.Where(u => u.Deactivate == false).ToList();
        }

        public List<User> GetAllUserNotPrivate() //kunde ej köra foreach loop remove för att konstigts
        {
            List<User> users = GetAllUsers();
            List<User> templist = new List<User>();

            foreach (var item in users)
            {
                if (item.PrivateProfile == false)
                {
                    templist.Add(item);
                }
            }

            return templist;
        }

        public List<User> GetUsersByString(string searchString)
        {
            List<string> tempList = new List<string>();
            if (searchString != null && searchString.Contains(" "))
            {
                tempList = searchString.Split().ToList();
                string firstName = tempList.ElementAt(0);
                string lastName = tempList.ElementAt(1);
                
                return db.Users.Where(x => x.Firstname.Contains(firstName) || x.Lastname.Contains(lastName) && x.Deactivate == false).ToList();
            }

            return db.Users.Where(x => x.Firstname.Contains(searchString) || x.Lastname.Contains(searchString) || searchString == null && x.Deactivate == false).ToList();
        }

        public List<User> GetUsersByStringVG(string searchString) //måste to lower på allt i think
        {
            List<string> tempList = new List<string>();
            List<int> userids = new List<int>();
            List<User> users = new List<User>();

            List<User> returlist = new List<User>();
            if (!String.IsNullOrWhiteSpace(searchString) && searchString.Contains(" "))
            {
                tempList = searchString.ToLower().Split().ToList();
                List<Skill> skills = new List<Skill>();
                
                
                foreach (var item in tempList)
                {
                    if (item != "") //måste ha då den splittar på mellanslag men får in tomma saker ändå
                    {
                        if (db.Users.Any(x => x.Firstname.Contains(item) || x.Lastname.Contains(item)))
                        {
                            foreach (var user in db.Users.ToList())
                            {
                                if (user.Firstname.Contains(item) || user.Lastname.Contains(item))
                                {
                                    users.Add(user);
                                }
                            }
                        }
                    }
                }

                userids = GetUserIDSBySkill(tempList);

                foreach (var item in userids)
                {
                    users.Add(GetUserByUserID(item));
                }
                returlist = users.Distinct().ToList();
                return returlist;
            }
            users = db.Users.Where(x => x.Firstname.Contains(searchString) || x.Lastname.Contains(searchString) || searchString == null && x.Deactivate == false).ToList();
            userids = GetUserIDSBySkillstring(searchString);
            foreach (var item in userids)
            {
                users.Add(GetUserByUserID(item));
            }
            
            returlist = users.Distinct().ToList(); 
            return returlist;
            
        }

        public List<int> GetUserIDSBySkill(List<string> searchStrings)
        {
            List<int> userids = new List<int>();
            List<User> users = new List<User>();
            foreach (var searchString in searchStrings)
            {

                if (db.Skills.Any(x => x.Title.ToLower().Contains(searchString))) //så vi inte loopar när den inte finns
                {
                    foreach (var item in db.Skills.ToList())
                    {
                        if (item.Title.ToLower().Contains(searchString))
                        {
                            foreach (var cv in db.CVs.ToList())
                            {
                                foreach (var cvSkill in cv.Skills.ToList())
                                {
                                    if (cvSkill.SkillID == item.SkillID)
                                    {
                                        userids.Add(cv.UserID);
                                    }
                                }
                            }
                        }
                    }
                }

                if (db.Work_Experiences.Any(x => x.Titel.ToLower().Contains(searchString))) 
                {
                    foreach (var item in db.Work_Experiences.ToList())
                    {
                        if (item.Titel.ToLower().Contains(searchString))
                        {
                            foreach (var cv in db.CVs.ToList())
                            {
                                foreach (var workExp in cv.Work_Experiences.ToList())
                                {
                                    if (workExp.WorkExpID == item.WorkExpID)
                                    {
                                        userids.Add(cv.UserID);
                                    }
                                }
                            }
                        }
                    }
                }

                if (db.Educations.Any(x => x.Title.ToLower().Contains(searchString))) 
                {
                    foreach (var item in db.Educations.ToList())
                    {
                        if (item.Title.ToLower().Contains(searchString))
                        {
                            foreach (var cv in db.CVs.ToList())
                            {
                                foreach (var education in cv.Educations.ToList())
                                {
                                    if (education.EduID == item.EduID)
                                    {
                                        userids.Add(cv.UserID);
                                    }
                                }
                            }
                        }
                    }
                }

            }

            return userids;
        }


        public List<int> GetUserIDSBySkillstring(string searchString)
        {
            List<int> userids = new List<int>();

            if (db.Skills.Any(x => x.Title.ToLower().Contains(searchString))) //så vi inte loopar när den inte finns
            {
                foreach (var item in db.Skills.ToList())
                {
                    if (item.Title.ToLower().Contains(searchString))
                    {
                        foreach (var cv in db.CVs.ToList())
                        {
                            foreach (var cvSkill in cv.Skills.ToList())
                            {
                                if (cvSkill.SkillID == item.SkillID)
                                {
                                    userids.Add(cv.UserID);
                                }
                            }
                        }
                    }
                }
            }

            if (db.Work_Experiences.Any(x => x.Titel.ToLower().Contains(searchString)))
            {
                foreach (var item in db.Work_Experiences.ToList())
                {
                    if (item.Titel.ToLower().Contains(searchString))
                    {
                        foreach (var cv in db.CVs.ToList())
                        {
                            foreach (var workExp in cv.Work_Experiences.ToList())
                            {
                                if (workExp.WorkExpID == item.WorkExpID)
                                {
                                    userids.Add(cv.UserID);
                                }
                            }
                        }
                    }
                }
            }

            if (db.Educations.Any(x => x.Title.ToLower().Contains(searchString)))
            {
                foreach (var item in db.Educations.ToList())
                {
                    if (item.Title.ToLower().Contains(searchString))
                    {
                        foreach (var cv in db.CVs.ToList())
                        {
                            foreach (var education in cv.Educations.ToList())
                            {
                                if (education.EduID == item.EduID)
                                {
                                    userids.Add(cv.UserID);
                                }
                            }
                        }
                    }
                }
            }
            return userids;
        }







            
        



    public void RegisterUser(string email,string adress,string firstname , string lastname) 
        {
            db.Users.Add(new Data.Models.User
            {
                Email = email,
                Adress = adress,
                Firstname = firstname,
                Lastname = lastname
            });
            db.SaveChanges();
        }
        
        public User GetUserByUserID(int id)
        {
            return db.Users.Where(u => u.UserID == id).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return db.Users.Where(u => u.Email == email).FirstOrDefault();
        }
        public int GetUserIDByEmail(string email)
        {
            var user = db.Users.Where(u => u.Email == email).FirstOrDefault();
            return user.UserID;
        }

        public void DeactivateUser(string email)
        {
            User user = GetUserByEmail(email);
            user.Deactivate = true;
            db.SaveChanges();
        }

        public void ReActivateUserIfDeactive(string email)
        {
            User user = GetUserByEmail(email);
            if (user.Deactivate == true)
            {
                user.Deactivate = false;
                db.SaveChanges();
            }
            else
                return;
        }
    }
}
