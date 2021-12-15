using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    }
}
