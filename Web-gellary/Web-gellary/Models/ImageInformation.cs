using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_gellary.Models
{
    public class ImageInformation
    {
        public int CountLike { get; set; }
        public int CountView { get; set; }
        public string AuthorName { get; set; }
        public string UrlUser { get; set; }
        public string Avatar { get; set; }
        public string UploadData { get; set; }    
    }

    public class Comment
    {
        public string TextComment { get; set; }
        public string DataComment { get; set; }
        public string UrlUser { get; set; }
        public string Avatar { get; set; }
        public string AuthorName { get; set; }
    }
} 