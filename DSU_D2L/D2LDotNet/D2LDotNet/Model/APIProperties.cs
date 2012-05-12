using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D2LDotNet.Model;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace D2LDotNet.API
{
    public class APIProperties
    {
        public APIProperties()
        {

        }

        public static ProductVersion[] GetProductVersions()
        {
            string path = "/d2l/api/versions/";
            Uri SendToD2L = new Uri(D2LSettings.D2LWebAddress + path);
            string method = HttpMethod.Get;

            //Append the query to the original URI
            UriBuilder build = new UriBuilder(SendToD2L);
            build.Query = Utilities.ApplicationLevelQueryString(new APICall(method, path));
            Uri final = build.Uri;

            //Send the request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(final);
            request.Method = method;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            string responseBody = reader.ReadToEnd();

                            ProductVersion[] versions = JsonConvert.DeserializeObject<ProductVersion[]>(responseBody);

                            return versions;
                        }
                    }
                }
            }
            //catches status codes in the 4 and 5 hundred series
            catch (WebException we)
            {
                return null;
            }
        }
    }
}
