using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    class Message
    {
        [Key]
        public int MessageID { get; set; }
        public string Content { get; set; }
    }
}
