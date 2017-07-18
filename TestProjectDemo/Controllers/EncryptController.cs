using Dropbox.Api;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TestProjectDemo.Controllers
{

    public class AccessToken
    {
        private string _token;
        private string Token
        {
            get
            {
                return _token;
            }

        }


        public AccessToken()
        {
            _token = GetAPIToken("Tsc.communitycloud@rangam.com", "rangam@123", "https://api.dropbox.com").Result;
        }
        private static async Task<string> GetAPIToken(string userName, string password, string apiBaseUri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //setup client
                    client.BaseAddress = new Uri(apiBaseUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //setup login data
                    var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"), 
                    new KeyValuePair<string, string>("username", userName), 
                    new KeyValuePair<string, string>("password", password), 
                });
                    try
                    {
                        //send request
                        HttpResponseMessage responseMessage = await client.PostAsync("Token", formContent);

                        //get access token from response body
                        var responseJson = await responseMessage.Content.ReadAsStringAsync();
                        JObject jObject = JObject.Parse(responseJson);

                        return jObject.GetValue("access_token").ToString();
                    }
                    catch
                    {
                        return null;
                    }
                    //return jObject.GetValue("access_token").ToString();
                    //return jObject.GetValue("access_token").ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static async Task<string> GetRequest(string token, string apiBaseUri, string requestPath)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }
                using (var client = new HttpClient())
                {
                    //setup client
                    client.BaseAddress = new Uri(apiBaseUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    //make request
                    HttpResponseMessage response = await client.GetAsync(requestPath);
                    var responseString = await response.Content.ReadAsStringAsync();
                    return responseString;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
    public class EncryptController : Controller
    {

        public async Task<FileResult> Index()
        {
            //GetDropboxFile();
           // AccessToken token = new AccessToken();
            DropboxClient client2 = new DropboxClient("DIhLmgHC-GAAAAAAAAA2qUpXqtwPpGtR69WaB91cb3k5fbcgCTqEifv7HW5Q4mRa");
            string folder = "Apps/AutismWorks/Employer/454/EmployerFile";
            string file = "kiPbsQBoztAkCFkvIHUR.png";
            using (var response = await client2.Files.DownloadAsync("/" + folder +"/"+ file))
            {
                var conttent = await response.GetContentAsByteArrayAsync();
               
                return File(conttent, System.Net.Mime.MediaTypeNames.Application.Octet, file);
            }
         
            //string token=

            return null;
        }
        public async void Downloadfile()
        {
            DropboxClient client2 = new DropboxClient("DIhLmgHC-GAAAAAAAAA2qUpXqtwPpGtR69WaB91cb3k5fbcgCTqEifv7HW5Q4mRa");
            string folder = "Dropbox";
            string file = "64.jpg";
            using (var response = await client2.Files.DownloadAsync("/" + folder + "/" + file))
            {

                using (var fileStream = System.IO.File.Create(Server.MapPath("~/Content")))
                {
                    (await response.GetContentAsStreamAsync()).CopyTo(fileStream);
                }
            }
        }


        //public ActionResult GetDropboxFile()
        //{

        //    TestProjectDemo.Models.DropBoxConfig config = new Models.DropBoxConfig();



        //    Dropbox.Api.FileSystemInfo file = config.DownloadFile("/Apps/iLNE-Age-5-35/Reward_Thumb.png");
        //    byte[] content = file.Data;

        //    return File(content, "", "Name");
        //}



        public static void DoGetHostEntry()
        {


            foreach (IPAddress address in Dns.GetHostAddresses("www.google.com"))
            {
                var test = address.ToString();
            }
        }
        public static string encrypt(string original, string passward)
        {
            try
            {
                string encrypted = null;
                TripleDESCryptoServiceProvider cryptoProvider = default(TripleDESCryptoServiceProvider);
                MD5CryptoServiceProvider md5hash = default(MD5CryptoServiceProvider);
                byte[] passwordhash = null;

                original = original.Trim();
                md5hash = new MD5CryptoServiceProvider();


                passwordhash = md5hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passward));
                md5hash = null;

                cryptoProvider = new TripleDESCryptoServiceProvider();
                cryptoProvider.Key = passwordhash;
                cryptoProvider.Mode = CipherMode.ECB;


                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(original);

                // Dim buffer() As Byte = Convert.FromBase64CharArray(original.ToCharArray, 0, original.ToCharArray.Length - 1)

                byte[] Encryptedbyte = cryptoProvider.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length);

                //encrypted = GetString(Encryptedbyte)
                encrypted = Convert.ToBase64String(cryptoProvider.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
                cryptoProvider = null;
                return encrypted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Decrypt(string encryptedString, string password)
        {
            try
            {
                string Decrypted = null;
                TripleDESCryptoServiceProvider CryptoProvider = default(TripleDESCryptoServiceProvider);
                MD5CryptoServiceProvider md5hash = default(MD5CryptoServiceProvider);
                encryptedString = encryptedString.Trim();
                byte[] passwordhash = null;

                md5hash = new MD5CryptoServiceProvider();
                passwordhash = md5hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                md5hash = null;

                CryptoProvider = new TripleDESCryptoServiceProvider();
                CryptoProvider.Key = passwordhash;
                CryptoProvider.Mode = CipherMode.ECB;

                // Dim buffer() As Byte = GetByteArray(encryptedString)
                //CryptoProvider.IV = Convert.ToByte("256")


                byte[] buffer = Convert.FromBase64String(encryptedString);
                // Dim buffer() As Byte = Convert.FromBase64CharArray(encryptedString.ToCharArray, 0, encryptedString.ToCharArray.Length - 1)


                byte[] desbuffer = CryptoProvider.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length);

                Decrypted = ASCIIEncoding.ASCII.GetString(desbuffer);

                CryptoProvider = null;

                return Decrypted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
