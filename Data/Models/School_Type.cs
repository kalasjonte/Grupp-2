using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    class School_Type
    {
        [Key]
        public int School_TypeID { get; set; }
        public string Type { get; set; }
    }
}
