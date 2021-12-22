using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Models;
using System.Web.Routing;
using System.Security.Cryptography.Xml;

namespace Grupp_2.Controllers
{
    public class ImagesController : Controller
    {
        private Datacontext db = new Datacontext();

        [HttpPost]
        public ActionResult Upload()  //Here just store 'Image' in a folder in Project Directory 
                         //  name 'UplodedFiles'
        {
            foreach (string file in Request.Files)
            {
                var postedFile = Request.Files[file];
                postedFile.SaveAs(Server.MapPath("~/UploadedFiles/") + Path.GetFileName(postedFile.FileName));
            }
            return RedirectToAction("Index");
        }
        public ActionResult Index() //I retrive Images List by using this Controller
        {
            var uploadedFiles = new List<Image>();

            var files = Directory.GetFiles(Server.MapPath("~/UploadedFiles"));

            foreach (var file in files)
            {
                var picture = new Image() { Name = Path.GetFileName(file) };

                picture.Path = ("~/UploadedFiles/") + Path.GetFileName(file);
                uploadedFiles.Add(picture);
            }

            return View(uploadedFiles);
        }
    }
}
