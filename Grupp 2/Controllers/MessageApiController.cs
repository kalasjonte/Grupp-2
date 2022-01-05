﻿using Data;
using Data.Models;
using Data.Respositories;
using System.Web.Http;


namespace Grupp_2.Controllers
{
    public class MessageApiController : ApiController
    {
        private UserRespository userRespository = new UserRespository();
        private MessageRepository messageRepository = new MessageRepository();

        [Route("api/SendAPI/{id}/{content}/{sender}")]
        [HttpGet]
        public IHttpActionResult SendMessage(int id, string content, string sender)
        {
            using (var db = new Datacontext())
            {
                var reciver = userRespository.GetUserByUserID(id);
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
                    messageRepository.SaveMessage(msg);

                    var usermsg = new User_Message
                    {
                        RecievingUser = id,
                        MessageID = msg.MessageID,
                        Read = false,
                        Sender = sender,

                    };
                    messageRepository.SaveUserMessage(usermsg);
                    return Ok();
                }
            }
        }

        [Route("api/GetApi/")]
        [HttpGet]
        public int GetUnreadMessages()
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            using (var db = new Datacontext())
            {
                User user = userRespository.GetUserByEmail(loggedInUserMail);
                int id = user.UserID;
                int unreadcount = messageRepository.GetUnreadMessagesCount(id);
                return unreadcount;
            }

        }
    }
}