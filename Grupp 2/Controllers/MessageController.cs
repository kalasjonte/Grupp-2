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

        // GET: Message
        public ActionResult Index()
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User user = userRespository.GetUserByEmail(loggedInUserMail);
            ViewBag.userID = user.UserID;

            var UserMessages = messageRepository.GetUserMessagesByUserID(user.UserID);
            List<MessagesViewModel> listMsgViewModels = new List<MessagesViewModel>();

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

        public ActionResult MarkAllAsRead(int userID)
        {
            messageRepository.MarkAllAsRead(userID);
            return RedirectToAction("Index");
        }


        // GET: Message/Details/5
        public ActionResult Details(int? id) //används inte?
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

        // GET: Message/Create
        public ActionResult Create(int id)
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User userSend = userRespository.GetUserByEmail(loggedInUserMail);
            string name = null;
            if (userSend != null)
            {
                name = userSend.Firstname + " " +  userSend.Lastname;
                //ViewBag.name = name;
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

        // POST: Message/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Message/Edit/5
        public ActionResult Edit(int? id) //används inte
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

        // POST: Message/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageID,Content")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Message/Delete/5
        public ActionResult Delete(int? id) //-> används i vg, vi kan fixa
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

        // POST: Message/Delete/5
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
