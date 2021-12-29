using Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Grupp_2.Models
{
    public class CreateCVViewModel
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string Imgpath { get; set; }

        public int UserID { get; set; }

        public string Email { get; set; }

        public string Adress { get; set; }
        public List<Education> Educations { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Work_Experience> Work_Experiences { get; set; }

        public List<Project> Projects { get; set; }
    }
}