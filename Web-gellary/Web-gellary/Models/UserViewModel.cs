using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_gellary.Models
{
    public class UserViewModel
    {
        public string Url {get;set;}
        public string Nick { get; set; }
        public string Avatar { get; set; }
        public int CountUploadImages { get; set; }
        public string Status { get; set; }
    }
}