using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProjectDemo.Models
{
    public class MyImage
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public HttpPostedFileBase File { get; set; }
        public byte[] FileData { get; set; }
    }
}