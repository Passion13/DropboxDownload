using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Imaging;

namespace TestProjectDemo.Controllers
{
    public class MultipleFileController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            FileStream fs = new FileStream(Server.MapPath("~/Content/Chrysanthemum.jpg"), FileMode.Open, FileAccess.Read, FileShare.Read);
            BitmapSource img = BitmapFrame.Create(fs);
            BitmapMetadata meta = (BitmapMetadata)img.Metadata;

            string Author = "";
            string Tags = "";

            string Path = Server.MapPath("~/Content/Chrysanthemum.jpg");

            Author = meta.Author != null ? meta.Author[0] : null;
            Tags = meta.Keywords != null ? meta.Keywords[0] : null;

            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] files)
        {

        
            Stream fileStream = files[0].InputStream;
        
   
            BitmapSource img = BitmapFrame.Create(fileStream);
            BitmapMetadata meta = (BitmapMetadata)img.Metadata;

            string Author = "";
            string Tags = "";

            string Path = Server.MapPath("~/Content/Chrysanthemum.jpg");

            Author = meta.Author != null ? meta.Author[0] : "Unknown";
            Tags = meta.Keywords != null ? meta.Keywords[0] : "no Tags";

            return View();
        }

    }
}
