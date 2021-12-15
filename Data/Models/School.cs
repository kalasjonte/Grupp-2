using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class School
    {
        [Key]
        public int SchoolID { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        


        [ForeignKey("School_Type")]
        public int Type { get; set; }
        public virtual School_Type School_Type { get; set; }

        public virtual ICollection<Education> Educations { get; set; }
    }
}
