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
        
        public User GetUserByEmail(string email)
        {
            return db.Users.Where(u => u.Email == email).FirstOrDefault();
        }

        public int GetUserIDByEmail(string email)
        {
            var user = db.Users.Where(u => u.Email == email).FirstOrDefault();
            return user.UserID;
        }
    }
}
