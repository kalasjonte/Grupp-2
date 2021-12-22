using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Grupp_2.Models
{
    public class CreateSchoolViewModel
    {
        public List<School_Type> School_Types { get; set; }

        public string SchoolPlace { get; set; }
        
        public string SchoolName { get; set; }
    }
}