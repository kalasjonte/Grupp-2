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

namespace Grupp_2.Controllers
{
    public class User_MessageController : Controller
    {
        private Datacontext db = new Datacontext();

        // GET: User_Message
        public ActionResult Index()
        {
            var user_Messages = db.User_Messages.Include(u => u.Message);
            return View(user_Messages.ToList());
        }

        // GET: User_Message/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Message user_Message = db.User_Messages.Find(id);
            if (user_Message == null)
            {
                return HttpNotFound();
            }
            return View(user_Message);
        }

        // GET: User_Message/Create
        public ActionResult Create()
        {
            ViewBag.MessageID = new SelectList(db.Messages, "MessageID", "Content");
            return View();
        }

        // POST: User_Message/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecievingUser,MessageID,Read,Sender")] User_Message user_Message)
        {
            if (ModelState.IsValid)
            {
                db.User_Messages.Add(user_Message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MessageID = new SelectList(db.Messages, "MessageID", "Content", user_Message.MessageID);
            return View(user_Message);
        }

        // GET: User_Message/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Message user_Message = db.User_Messages.Find(id);
            if (user_Message == null)
            {
                return HttpNotFound();
            }
            ViewBag.MessageID = new SelectList(db.Messages, "MessageID", "Content", user_Message.MessageID);
            return View(user_Message);
        }

        // POST: User_Message/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecievingUser,MessageID,Read,Sender")] User_Message user_Message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_Message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MessageID = new SelectList(db.Messages, "MessageID", "Content", user_Message.MessageID);
            return View(user_Message);
        }

        // GET: User_Message/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Message user_Message = db.User_Messages.Find(id);
            if (user_Message == null)
            {
                return HttpNotFound();
            }
            return View(user_Message);
        }

        // POST: User_Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User_Message user_Message = db.User_Messages.Find(id);
            db.User_Messages.Remove(user_Message);
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
