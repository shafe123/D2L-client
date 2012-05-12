using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using D2LDotNet.Model;

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

        public static string ApplicationLevelQueryString(APICall ApiCall)
        {
            string timestamp = UnixTimeStampString();
            
            string query = Getx_a();
            query += "&" + Getx_c(ApiCall.method, ApiCall.path, timestamp);
            query += "&" + Getx_t(timestamp);

            return query;
        }
    }
}
