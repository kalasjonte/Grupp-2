using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class User_Messages
    {
        [Key, Column(Order = 0)]
        public int RecievingUser { get; set; }

        [Key, Column(Order = 1)]
        public int MessageID { get; set; }

        public virtual User User { get; set; }
        public virtual Message Message { get; set; }
        public bool Read { get; set; }

        public string Sender { get; set; }
        
        
    }
}
