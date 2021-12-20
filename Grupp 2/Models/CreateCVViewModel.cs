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
        public string Användare { get; set; }
        [Required]
        public string imgpath { get; set; }
        public List<Education> Educations { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Work_Experience> Work_Experiences { get; set; }
    }
}