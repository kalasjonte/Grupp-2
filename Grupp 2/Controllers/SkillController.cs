using System.Data;
using System.Linq;
using System.Web.Mvc;
using Data;
using Data.Models;

namespace Grupp_2.Controllers
{
    public class SkillController : Controller
    {
        private Datacontext db = new Datacontext();

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SkillID,Title")] Skill skill)
        {
            Skill tempSkill = db.Skills.Where(x => x.Title == skill.Title).FirstOrDefault();

            if (tempSkill == null)
            {
                if (ModelState.IsValid)
                {
                    db.Skills.Add(skill);
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
            }
            else if (tempSkill.Title.ToLower() == skill.Title.ToLower())
            {
                TempData["alertMessage"] = "Denna färdighet existerar redan i systemet!";
                return RedirectToAction("Create");
            }
            return View(skill);
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
