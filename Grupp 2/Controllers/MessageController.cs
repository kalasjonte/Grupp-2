using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Models;
using Data.Respositories;
using Grupp_2.Models;

namespace Grupp_2.Controllers
{
    public class MessageController : Controller
    {
        private Datacontext db = new Datacontext();
        private UserRespository userRespository = new UserRespository();
        private MessageRepository messageRepository = new MessageRepository();

        public ActionResult Index()
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User user = userRespository.GetUserByEmail(loggedInUserMail);
            ViewBag.userID = user.UserID;

            var UserMessages = messageRepository.GetUserMessagesByUserID(user.UserID);
            List<MessagesViewModel> listMsgViewModels = new List<MessagesViewModel>();

            //Skapar en ny message-viewmodel för varje meddelande som finns i databasen.
            foreach (var item in UserMessages)
            {
                var MsgViewModel = new MessagesViewModel
                {
                    Reciver = user.UserID,
                    Read = item.Read,
                    Sender = item.Sender,
                    MessageID = item.MessageID
                };
                var msg = messageRepository.GetMessageByMessageID(item.MessageID);
                MsgViewModel.Content = msg.Content;
                listMsgViewModels.Add(MsgViewModel);

            }

            return View(listMsgViewModels);
        }

        public ActionResult MarkAsRead(int id)
        {
            messageRepository.MarkAsRead(id);
            return RedirectToAction("Index");
        }
        public ActionResult MarkAsUnRead(int id)
        {
            messageRepository.MarkAsUnRead(id);
            return RedirectToAction("Index");
        }


        public ActionResult MarkAllAsRead(int userID)
        {
            messageRepository.MarkAllAsRead(userID);
            return RedirectToAction("Index");
        }

        public ActionResult Create(int id)
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User userSend = userRespository.GetUserByEmail(loggedInUserMail);
            string name = null;
            if (userSend != null)
            {
                name = userSend.Firstname + " " +  userSend.Lastname;
            }

            var UserRec = userRespository.GetUserByUserID(id);
            var model = new MessagesViewModel
            {
                Sender = name,
                Reciver = id,
                ReciverName = UserRec.Firstname + " " + UserRec.Lastname
             };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageID,Content")] Message message) //denna körs aldrig, går t api ist
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Message/Delete/DeleteAjax/{id}")]
        public ActionResult DeleteAjax(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
