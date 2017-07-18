using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OAuthProtocol;
using System.Web;
using System.Collections.Generic;
using System.Text;

namespace Dropbox.Api
{
    public class DropboxApi
    {
        private readonly OAuthToken _accessToken;
        private readonly string _consumerKey;
        private readonly string _consumerSecret;

        public DropboxApi(string consumerKey, string consumerSecret, OAuthToken accessToken)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _accessToken = accessToken;
        }

        private string GetResponse(Uri uri)
        {
            string retval = "";
            try
            {
                var oauth = new OAuth();
                var requestUri = oauth.SignRequest(uri, _consumerKey, _consumerSecret, _accessToken);
                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.Method = WebRequestMethods.Http.Get;
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                retval = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("401"))
                {
                    retval = GetResponse(uri);
                }
            }
            return retval;

        }
        private byte[] GetByteResponse(Uri uri)
        {
            byte[] retval = new byte[0];
            try
            {
                var oauth = new OAuth();
                var requestUri = oauth.SignRequest(uri, _consumerKey, _consumerSecret, _accessToken);
                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.Method = WebRequestMethods.Http.Get;
                var response = request.GetResponse();
                Stream str = response.GetResponseStream();
                retval = null;
                retval = ReadFully(str);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("401"))
                {
                    retval = null;
                    retval = GetByteResponse(uri);
                }
            }
            return retval;
        }
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        public ShareResponse GetSharepath(string root, string path)
        {
            var uri = new Uri(new Uri(DropboxRestApi.BaseUri), String.Format("shares/auto/{0}/{1}?short_url=false", root, path));
            var json = GetResponse(uri);
            return ParseJson<ShareResponse>(json);
        }
        public byte[] GetThumbnail(string path)
        {

            var uri = new Uri(new Uri(DropboxRestApi.ApiContentServer), String.Format("thumbnails/auto{0}?format=jpeg&size=m", path));
            var json = GetByteResponse(uri);
            return json;
        }
        public byte[] GetThumbnailLarge(string path)
        {
            var uri = new Uri(new Uri(DropboxRestApi.ApiContentServer), String.Format("thumbnails/auto{0}?format=jpeg&size=l", path));
            var json = GetByteResponse(uri);
            return json;
        }
        private bool ResponseExists(Uri uri)
        {
            bool retval = false;
            try
            {
                var oauth = new OAuth();
                var requestUri = oauth.SignRequest(uri, _consumerKey, _consumerSecret, _accessToken);
                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.Method = WebRequestMethods.Http.Get;
                return request.HaveResponse;
            }
            catch (Exception Ex)
            {
                if (Ex.Message.ToLower().Contains("401"))
                {
                    retval = ResponseExists(uri);
                }
            }
            return retval;
        }

        private static T ParseJson<T>(string json) where T : class, new()
        {
            var jobject = JObject.Parse(json);
            return JsonConvert.DeserializeObject<T>(jobject.ToString());
        }

        public Account GetAccountInfo()
        {
            var uri = new Uri(new Uri(DropboxRestApi.BaseUri), "account/info");
            var json = GetResponse(uri);
            return ParseJson<Account>(json);
        }

        public File GetFiles(string root, string path)
        {
            var uri = new Uri(new Uri(DropboxRestApi.BaseUri), String.Format("metadata/{0}/{1}", root, path));
            var json = GetResponse(uri);
            return ParseJson<File>(json);
        }

        public FileSystemInfo CreateFolder(string root, string path)
        {
            try
            {
                var uri = new Uri(new Uri(DropboxRestApi.BaseUri), String.Format("metadata/{0}/{1}", root, path));
                var json = GetResponse(uri);
                return ParseJson<FileSystemInfo>(json);
            }
            catch (Exception Ex)
            {
                if (Ex.Message.Contains("404"))
                {
                    var uri = new Uri(new Uri(DropboxRestApi.BaseUri),
                        String.Format("fileops/create_folder?root={0}&path={1}",
                        root, UpperCaseUrlEncode(path)));
                    var json = GetResponse(uri);
                    return ParseJson<FileSystemInfo>(json);
                }
            }
            return null;
        }
        public ShareResponse GetSharepath(string FilePath)
        {
            FilePath = FilePath.TrimStart('/');
            var uri = new Uri(new Uri(DropboxRestApi.BaseUri), String.Format("shares/auto/{0}?short_url=false", FilePath));
            var json = GetResponse(uri);
            if (json == "")
            {
                return new ShareResponse();
            }
            return ParseJson<ShareResponse>(json);
        }
        public FileSystemInfo Move(string root, string fromPath, string toPath)
        {
            var uri = new Uri(new Uri(DropboxRestApi.BaseUri),
                String.Format("fileops/move?root={0}&from_path={1}&to_path={2}",
                root, UpperCaseUrlEncode(fromPath), UpperCaseUrlEncode(toPath)));
            var json = GetResponse(uri);
            return ParseJson<FileSystemInfo>(json);
        }

        public FileSystemInfo Delete(string root, string path)
        {
            var uri = new Uri(new Uri(DropboxRestApi.BaseUri),
                String.Format("fileops/delete?root={0}&path={1}",
                root, UpperCaseUrlEncode(path)));
            var json = GetResponse(uri);
            return ParseJson<FileSystemInfo>(json);
        }

        public FileSystemInfo DownloadFile(string root, string path)
        {
            FileSystemInfo retvalfile = new FileSystemInfo();
            try
            {

                var uri = new Uri(new Uri(DropboxRestApi.ApiContentServer),
                    String.Format("files/download?root={0}&path={1}",
                    root, UpperCaseUrlEncode(path)));

                var oauth = new OAuth();
                var requestUri = oauth.SignRequest(uri, _consumerKey, _consumerSecret, _accessToken);

                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.Method = WebRequestMethods.Http.Get;
                var response = request.GetResponse();

                var metadata = response.Headers["x-dropbox-metadata"];
                var file = ParseJson<FileSystemInfo>(metadata);

                using (Stream responseStream = response.GetResponseStream())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    do
                    {
                        bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                        memoryStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead > 0);

                    file.Data = memoryStream.ToArray();
                }

                retvalfile = file;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("401"))
                {
                    retvalfile = DownloadFile(root, path);
                }
            }
            return retvalfile;

        }
        public FileSystemInfo UploadFile(string root, string path, Stream fileStream)
        {
            FileSystemInfo retvalfile = new FileSystemInfo();
            try
            {
                var uri = new Uri(new Uri(DropboxRestApi.ApiContentServer),
                    String.Format("files_put/{0}/{1}",
                    root, UpperCaseUrlEncode(path)));

                var oauth = new OAuth();
                var requestUri = oauth.SignRequest(uri, _consumerKey, _consumerSecret, _accessToken, "PUT");

                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.Method = WebRequestMethods.Http.Put;
                request.KeepAlive = true;
                request.Timeout = System.Threading.Timeout.Infinite;
                byte[] buffer;
                int length = (int)fileStream.Length;
                buffer = new byte[length];
                fileStream.Read(buffer, 0, length);

                request.ContentLength = buffer.Length;
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(buffer, 0, buffer.Length);
                }
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var json = reader.ReadToEnd();
                retvalfile = ParseJson<FileSystemInfo>(json);

            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("401"))
                {
                    retvalfile = DownloadFile(root, path);
                }
            }
            return retvalfile;
        }

        public FileSystemInfo UploadFile(string root, string path, string file)
        {

            FileSystemInfo retvalfile = new FileSystemInfo();
            try
            {
                var uri = new Uri(new Uri(DropboxRestApi.ApiContentServer),
                    String.Format("files_put/{0}/{1}",
                    root, UpperCaseUrlEncode(path)));

                var oauth = new OAuth();
                var requestUri = oauth.SignRequest(uri, _consumerKey, _consumerSecret, _accessToken, "PUT");

                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.Method = WebRequestMethods.Http.Put;
                request.KeepAlive = true;
                request.Timeout = System.Threading.Timeout.Infinite;
                byte[] buffer;
                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    int length = (int)fileStream.Length;
                    buffer = new byte[length];
                    fileStream.Read(buffer, 0, length);
                }

                request.ContentLength = buffer.Length;
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var json = reader.ReadToEnd();
                retvalfile = ParseJson<FileSystemInfo>(json);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("401"))
                {
                    retvalfile = DownloadFile(root, path);
                }
            }
            return retvalfile;
        }

        private static string UpperCaseUrlEncode(string s)
        {
            char[] temp = HttpUtility.UrlEncode(s).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }

            var values = new Dictionary<string, string>()
            {
                { "+", "%20" },
                { "(", "%28" },
                { ")", "%29" }
            };

            var data = new StringBuilder(new string(temp));
            foreach (string character in values.Keys)
            {
                data.Replace(character, values[character]);
            }
            return data.ToString();
        }
    }
}
