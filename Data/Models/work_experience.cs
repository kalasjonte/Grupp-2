using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    class Work_experience
    {
        [Key]
        public int WorkExpID { get; set; }
        public string Titel { get; set; }

    }
}
