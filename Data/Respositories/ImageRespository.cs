using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Data.Respositories
{
    public class ImageRespository
    {
        public Datacontext db { get; set; }

        public ImageRespository()
        {
            db = new Datacontext();
        }



        //public void SetDefaultImage() //-> om den inte existrerar
        //{
        //    var tempImg = db.Images.Where(i => i.ImageID == 1).FirstOrDefault();
        //    if (tempImg == null)
        //    {
        //        db.Images.Add(new Image
        //        {
        //           ImageID = 1,
        //           Name = "profilepicture.jpg",
        //           Path = Server.MapPath("~/UploadedFiles/") + "profilepicture.jpg" 
        //        });
        //    }
        //}
    }
}
