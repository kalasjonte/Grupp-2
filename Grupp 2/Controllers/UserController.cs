using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Data;
using Data.Models;
using Data.Respositories;

namespace Grupp_2.Controllers
{
    public class UserController : Controller
    {
        private Datacontext db = new Datacontext();
        private UserRespository userRespository = new UserRespository();

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Firstname,Lastname,Password,Adress,Email,PrivateProfile")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit()
        {
            string loggedInUserMail = User.Identity.Name.ToString();
            User user = userRespository.GetUserByEmail(loggedInUserMail);
            int? id = user.UserID;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Firstname,Lastname,Adress,Email,PrivateProfile")] User user)
        {
            var regex = @"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$";
            if (user.Firstname != null && user.Lastname != null && user.Adress != null)
            {
                var matchFirst = Regex.Match(user.Firstname, regex, RegexOptions.IgnoreCase);
                var matchLast = Regex.Match(user.Lastname, regex, RegexOptions.IgnoreCase);

                if (!matchFirst.Success || !matchLast.Success)
                {
                    TempData["alertMessage"] = "One of your name-fields contains invalid characters!";
                }
                else if (ModelState.IsValid)
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = user.UserID });
                }
            }
            else TempData["alertMessage"] = "One of your fields is empty!";
            return View(user);
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
