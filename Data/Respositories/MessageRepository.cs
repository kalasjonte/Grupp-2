using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Respositories
{
    public class MessageRepository
    {
        public Datacontext db { get; set; }

        public MessageRepository()
        {
            db = new Datacontext();
        }

        public void SaveMessage(Message msg)
        {
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        public void SaveUserMessage(User_Message user_Message)
        {
            db.User_Messages.Add(user_Message);
            db.SaveChanges();
            
        }

        public int GetUnreadMessagesCount(int id)
        {
            return db.User_Messages.Where(um => um.Read == false && um.RecievingUser == id).Count();
        }
    }
}
