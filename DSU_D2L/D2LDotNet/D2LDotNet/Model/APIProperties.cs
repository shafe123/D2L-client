using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D2LDotNet.Model;
using System.Net;
using System.IO;

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

            string json = Utilities.ApplicationLevelRequest(new APICall(HttpMethod.Get, path));
            ProductVersion[] versions = Utilities.Deserialize<ProductVersion[]>(json);

            return versions;
        }

        //version is the number (for example: 1.0)
        public static SupportedVersion GetSupportedVersions(D2LPRODUCT product, string version)
        {
            string productCode = "";
            switch (product)
            {
                case D2LPRODUCT.ePortfolio:
                    productCode = "ep";
                    break;
                case D2LPRODUCT.LearningEnvironment:
                    productCode = "le";
                    break;
                case D2LPRODUCT.LearningPlatform:
                    productCode = "lp";
                    break;
                case D2LPRODUCT.LearningRepository:
                    productCode = "lr";
                    break;
            }
                    

            string path = "/d2l/api/" + productCode + "/versions/" + version;

            string json = Utilities.ApplicationLevelRequest(new APICall(HttpMethod.Get, path));
            SupportedVersion supportedVersion = Utilities.Deserialize<SupportedVersion>(json);

            return supportedVersion;
        }
    }
}
