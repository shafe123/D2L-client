using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using D2LDotNet.Model;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace D2LDotNet
{
    public class Utilities
    {
        public static long UnixTimeStampValue()
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan difference = now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return (long)difference.TotalSeconds;
        }

        public static string UnixTimeStampString()
        {
            return UnixTimeStampValue().ToString();
        }

        public static string Getx_a()
        {
            return "x_a=" + D2LSettings.AppID;
        }

        public static string Getx_c(string method, string path, string timestamp)
        {
            string signature = method.ToUpperInvariant();
            signature += "&" + path.ToLowerInvariant();
            signature += "&" + timestamp;

            //Application signature result. This should be a one-way hash of information including path, timestamp, and HTTP method, together with the client application’s key.
            string x_c;
            //Get the UTF8 encoded values of the Application signature result, and the Application Key
            byte[] keyByte = Encoding.UTF8.GetBytes(D2LSettings.AppKey);
            byte[] sigByte = Encoding.UTF8.GetBytes(signature);

            //Compute a 256-bit hash based off of the key and signature
            HMACSHA256 hmacSha = new HMACSHA256(keyByte);
            byte[] hash = hmacSha.ComputeHash(sigByte);

            //not sure why this step is necessary
            string base64string = Convert.ToBase64String(hash);
            base64string = base64string.Replace("=", "");
            base64string = base64string.Replace("+", "-");
            base64string = base64string.Replace("/", "_");

            x_c = "x_c=" + base64string;

            return x_c;
        }

        public static string Getx_t(string timestamp)
        {
            return "x_t=" + timestamp;
        }

        public static string ApplicationLevelRequest(APICall ApiCall)
        {
            string timestamp = UnixTimeStampString();
            Uri SendToD2L = new Uri(D2LSettings.D2LWebAddress + ApiCall.path);

            UriBuilder build = new UriBuilder(SendToD2L);
            
            string query = Getx_a();
            query += "&" + Getx_c(ApiCall.method, ApiCall.path, timestamp);
            query += "&" + Getx_t(timestamp);

            build.Query = query;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(build.Uri);
            return MakeRequest(request, ApiCall.method);
        }

        public static string MakeRequest(HttpWebRequest request, string method)
        {
            string result;
            request.Method = method;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                result = "";   
            }

            return result;
        }

        public static string ConvertHttpMethod(HttpMethod method)
        {
            string httpMethod;

            switch (method)
            {
                case HttpMethod.Get:
                    httpMethod = "GET";
                    break;
                case HttpMethod.Post:
                    httpMethod = "POST";
                    break;
                case HttpMethod.Put:
                    httpMethod = "PUT";
                    break;
                case HttpMethod.Delete:
                    httpMethod = "DELETE";
                    break;
                default:
                    httpMethod = "";
                    break;
            }

            return httpMethod;
        }

        public static T Deserialize<T>(string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
