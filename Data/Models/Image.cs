using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.Models
{
    public class Image
    {
        public int ImageID { get; set; }
        public IEnumerable<HttpPostedFile> Picture { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

    }
}
