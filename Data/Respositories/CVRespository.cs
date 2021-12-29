using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Respositories
{
    public class CVRespository
    {
        public Datacontext db { get; set; }

        public CVRespository()
        {
            db = new Datacontext();
        }

        public void CreateCV(int id)
        {
            db.CVs.Add(new Data.Models.CV 
            { 
                UserID = id,
                ImageID = 5 
            });
            db.SaveChanges();
        }
    }
}
