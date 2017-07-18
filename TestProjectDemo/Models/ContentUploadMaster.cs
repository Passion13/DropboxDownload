using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProjectDemo.Models
{
    public class ContentUploadMaster
    {
        public int ContentUploadingId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Author { get; set; }
        public string Keywords { get; set; }
        public string FileContent { get; set; }
        public string Size { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }

    }
}