using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Work_Experience
    {
        [Key]
        public int WorkExpID { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "Erfarenheten måste minst vara {2} karaktärer långt.", MinimumLength = 3)]
        [Display(Name = "Titel:")]
        public string Titel { get; set; }

        public virtual ICollection<CV> CVs { get; set; }

    }
}
