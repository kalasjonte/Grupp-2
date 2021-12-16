using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public bool PrivateProfile { get; set; }
        
        
        public virtual ICollection<Project> In_Projects { get; set; }
        public virtual ICollection<Project> Owned_Project { get; set; }


        public virtual ICollection<User_Message> User_Messages { get; set; }
    }
}
