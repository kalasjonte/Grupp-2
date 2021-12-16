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
        public string Description { get; set; }
        


        [ForeignKey("User")]
        public int Creator { get; set; }
        public virtual User User { get; set; }
        
        public virtual ICollection<User> Users { get; set; }


    }
}
