using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Education
    {
        [Key]
        public int EduID { get; set; }
        public string Title { get; set; }
    }
}
