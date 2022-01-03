using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Skill
    {
        [Key]
        public int SkillID { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "Färdigheten måste minst vara {2} karaktärer långt.", MinimumLength = 3)]
        [Display(Name = "Titel:")]
        public string Title { get; set; }

        public virtual ICollection<CV> CVs { get; set; }
    }
}
