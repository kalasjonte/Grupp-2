using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class School
    {
        [Key]
        public int SchoolID { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
    }
}
