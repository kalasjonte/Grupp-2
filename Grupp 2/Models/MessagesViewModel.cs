using Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Grupp_2.Models
{
   
        public class MessagesViewModel
        {
            public int MessageID { get; set; }
            [Display(Name = "Innehåll:")]
            public string Content { get; set; }
            public int Reciver { get; set; }
            public bool Read { get; set; }
            [Display(Name = "Avsändare:")]
            public string Sender { get; set; }

            [Display(Name = "Till:")]
            public string ReciverName { get; set; }

        }
    
}