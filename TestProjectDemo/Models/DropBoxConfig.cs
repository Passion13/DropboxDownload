using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Web;
using Dropbox.Api;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using OAuthProtocol;
using DropNet;

namespace TestProjectDemo.Models
{
    //public class DropBoxConfig
    //{
    //    DropBoxAuthentication irDropBoxAuthentication = new DropBoxAuthentication();
    //    //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString);
    //    public string ConsumerKey { get; set; }
    //    public string ConsumerSecret { get; set; }
    //    //static string ConsumerKey = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DropboxAppKey"]);
    //    //static string ConsumerSecret = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DropboxAppSecret"]);
    //    public DropBoxConfig()
    //    {

    //        ConsumerKey =  Convert.ToString("ws0rvu2nlfiwa85");
    //        ConsumerSecret = Convert.ToString("cdnoevs3pig9hoo");
    //    }

        
    //    public Dropbox.Api.FileSystemInfo DownloadFile(string filename, string SavePath = "")
    //    {


    //        DropNetClient _client = new DropNetClient(ConsumerKey, ConsumerSecret);
    //        var token=_client.GetAccessToken();

    //        OAuthProtocol.OAuthToken accessToken = null;


    //        ////accessToken = new OAuthToken("sw5ko8pl2msec0jd", "88d6bh16t66xive");
    //        //accessToken = new OAuthToken("DIhLmgHC-GAAAAAAAAA2hHc8J4c1mYiyDUtGmV9KhTuwl78VKQHd9wwn1JRqqxZ9", "cdnoevs3pig9hoo");
    //        accessToken = GetAccessToken(ConsumerKey, ConsumerSecret);
               

    //        // Uncomment the following line or manually provide a valid token so that you
    //        // don't have to go through the authorization process each time.;

    //        //  var accessToken = new OAuthToken("token", "secret");

    //        var api = new DropboxApi(ConsumerKey, ConsumerSecret, accessToken);
    //        Guid gd = Guid.NewGuid();
    //        if (SavePath == "")
    //        {
    //            SavePath = "dropbox";
    //        }
    //        return api.DownloadFile(SavePath, filename);

    //    }



    //    private static OAuthProtocol.OAuthToken GetAccessToken(string ConsumerKey, string ConsumerSecret)
    //    {

    //        var oauth = new OAuth();

    //        var requestToken = oauth.GetRequestToken(new Uri(DropboxRestApi.BaseUri), ConsumerKey, ConsumerSecret);

    //        var authorizeUri = oauth.GetAuthorizeUri(new Uri(DropboxRestApi.AuthorizeBaseUri), requestToken);
    //        //Process.Start(authorizeUri.AbsoluteUri);
    //        //Thread.Sleep(5000); // Leave some time for the authorization step to complete

    //        return requestToken;
    //    }

    //    public ShareResponse GetSharepath(string filename, string SavePath = "")
    //    {
    //        OAuthProtocol.OAuthToken accessToken = null;
           
    //                accessToken = GetAccessToken(ConsumerKey, ConsumerSecret);
              
    //        var api = new DropboxApi(ConsumerKey, ConsumerSecret, accessToken);
    //        ShareResponse shrretval = api.GetSharepath(filename);
    //        shrretval.url = shrretval.url.Replace("dl=0", "dl=1");
    //        return shrretval;

    //    }

    //    public string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
    //    {
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            // Convert Image to byte[]
    //            image.Save(ms, format);
    //            byte[] imageBytes = ms.ToArray();

    //            // Convert byte[] to Base64 String
    //            string base64String = Convert.ToBase64String(imageBytes);
    //            return base64String;
    //        }
    //    }
    //    public Image byteArrayToImage(byte[] byteArrayIn)
    //    {
    //        MemoryStream ms = new MemoryStream(byteArrayIn);
    //        Image returnImage = Image.FromStream(ms);
    //        return returnImage;
    //    }
    //}



    public partial class DropBox
    {
        public int ID { get; set; }
        public string Token { get; set; }
        public string Secret { get; set; }
    }
    public class FilePathLogic
    {
        public IEnumerable<HttpPostedFileWrapper> files { get; set; }

        public static string PartFilename(string inputstring)
        {
            inputstring = inputstring.Replace("/", "/");
            inputstring = inputstring.Replace("//", "/");
            inputstring = inputstring.Replace("\\", "/");
            string[] str = inputstring.Split('/');
            return str[str.Length - 1];
        }


    }
}