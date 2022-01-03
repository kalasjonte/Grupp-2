using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Education
    {
        [Key]
        public int EduID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Vänligen lägg till en utbildning med minst {2} karaktärer.", MinimumLength = 3)]
        [Display(Name = "Titel:")]
        public string Title { get; set; }
        public virtual ICollection<CV> CVs { get; set; }
    }
}
