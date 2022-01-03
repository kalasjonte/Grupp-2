using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }
        public string Titel { get; set; }

        
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        


        [ForeignKey("User")]
        [Display(Name = "´Skapare")]
        public int Creator { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Projects_Users> Users { get; set; }
    }
}
