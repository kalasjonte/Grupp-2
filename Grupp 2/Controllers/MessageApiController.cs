using Data;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace Grupp_2.Controllers
{
    public class MessageApiController : ApiController
    {

        [Route("api/SendAPI/{id}/{content}/{sender}")]
        [HttpGet]
        public IHttpActionResult SendMessage(int id, string content, string sender)
        {
            using (var db = new Datacontext())
            {
                var reciver = db.Users.Where(u => u.UserID == id).FirstOrDefault();
                var msg = new Message
                {
                    Content = content
                };

                if (reciver == null)
                {
                    return BadRequest();
                }
                else
                {
                    db.Messages.Add(msg);
                    db.SaveChanges();

                    var usermsg = new User_Message
                    {
                        RecievingUser = id,
                        MessageID = msg.MessageID,
                        Read = false,
                        Sender = sender,
                        
                    };
                    db.User_Messages.Add(usermsg);
                    db.SaveChanges();
                    return Ok();
                }
            }
        }

        [Route("api/GetApi/")]
        [HttpGet]
        public int GetUnreadMessages()
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            using ( var db = new Datacontext())
            {
                User user = db.Users.Where(u => u.Email == loggedInUserMail).FirstOrDefault();
                int id = user.UserID;
                int unreadcount = db.User_Messages.Where(um => um.Read == false && um.RecievingUser == id).Count();
                return unreadcount;
            }

        }





    }
}