using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grupp_2.Models
{
    public class MessagesViewModel
    {
        public int MessageID { get; set; }
        public string Content { get; set; }
        public int Reciver { get; set; }
        public bool Read { get; set; }
        public string Sender { get; set; }
    }
}