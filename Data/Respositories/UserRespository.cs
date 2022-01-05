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
