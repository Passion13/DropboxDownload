using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Imaging;
using TestProjectDemo.Models;


namespace TestProjectDemo.Controllers
{
    public class FolderStructureController : Controller
    {

        public ActionResult Index()
        {

            string fileName = "";
            string sourcePath = @"D:\Folder\";
            string targetPath = Server.MapPath("~/Content/images");
            List<string> folder = new List<string>();
            List<string> subfolder = new List<string>();

            DirectoryInfo obj = new DirectoryInfo(sourcePath);


            foreach (var k in obj.GetDirectories())
            {
                folder.Add(k.FullName);

                DirectoryInfo obj1 = new DirectoryInfo(k.FullName + "\\");

                if (!Directory.Exists(targetPath + "\\" + new DirectoryInfo(k.FullName).Name))
                {
                    Directory.CreateDirectory(targetPath + "\\" + new DirectoryInfo(k.FullName).Name);
                }
                foreach (var item in obj1.GetDirectories())
                {
                    subfolder.Add(item.FullName);

                    if (!Directory.Exists(targetPath + "\\" + new DirectoryInfo(k.FullName).Name + "\\" + new DirectoryInfo(item.FullName).Name))
                    {
                        Directory.CreateDirectory(targetPath + "\\" + new DirectoryInfo(k.FullName).Name + "\\" + new DirectoryInfo(item.FullName).Name);
                    }
                    string[] files = System.IO.Directory.GetFiles(sourcePath+ "\\" + new DirectoryInfo(k.FullName).Name + "\\" + new DirectoryInfo(item.FullName).Name);
                    string destFile = "";
                    
                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        fileName = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine(targetPath + "\\" + new DirectoryInfo(k.FullName).Name + "\\" + new DirectoryInfo(item.FullName).Name, fileName);
                        System.IO.File.Copy(s, destFile, true);
                        //insert image meta Data Information into database 
                        FileStream fs = new FileStream(destFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                        BitmapSource img = BitmapFrame.Create(fs);
                        BitmapMetadata meta = (BitmapMetadata)img.Metadata;

                        ContentUploadMaster model = new ContentUploadMaster() { 
                            Author = meta.Author != null ? meta.Author[0] : null, 
                            Keywords = meta.Keywords != null ? meta.Keywords[0] : null,
                            Title = meta.Title,
                            Subject = meta.Subject,
                            FilePath = destFile,
                            Extension = meta.Format,
                            Name = fileName
                        };
                       

                       
                    }
                 
                }
            }
            return View();
        }

    }
}
