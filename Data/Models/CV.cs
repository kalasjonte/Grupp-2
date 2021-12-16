using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CV
    {
        [Key]
        public int CVID { get; set; }

        public string ImgPath { get; set; }

        [ForeignKey("User")]
        public int UID { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<Work_Experience> Work_Experiences { get; set; }
    }
}
