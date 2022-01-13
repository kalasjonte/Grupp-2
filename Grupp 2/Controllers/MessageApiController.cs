﻿using Data;
using Data.Models;
using Data.Respositories;
using Grupp_2.Models;
using System.Collections.Generic;
using System.Web.Http;


namespace Grupp_2.Controllers
{
    //Här hanterar vi våra ajax-calls
    public class MessageApiController : ApiController 
    {
        private UserRespository userRespository = new UserRespository();
        private MessageRepository messageRepository = new MessageRepository();
        private CVRespository cVRespository = new CVRespository();

        [Route("api/SendAPI/")]
        [HttpPost]
        public IHttpActionResult SendMessage(MessagesViewModel model)
        {
            using (var db = new Datacontext())
            {
                var reciver = userRespository.GetUserByUserID(model.Reciver);
                var msg = new Message
                {
                    Content = model.Content
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
                        RecievingUser = model.Reciver,
                        MessageID = msg.MessageID,
                        Read = false,
                        Sender = model.Sender,

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

        [Route("api/GetApiVisning/")]
        [HttpGet]
        public int GetShowCountCV()
        {
                int cvID = cVRespository.GetCVIDByEmail(User.Identity.Name.ToString());
                int clicks = cVRespository.GetClicks(cvID);
                return clicks;
        }

        [Route("api/GetGithubReps/")]
        [HttpGet]
        public List<Project> GetGithubReps()
        {
            List<Project> projects = new List<Project>();
            return projects;
        }
    }
}