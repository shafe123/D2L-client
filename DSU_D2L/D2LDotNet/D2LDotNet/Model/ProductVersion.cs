using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D2LDotNet.Model
{
    [Serializable]
    public class ProductVersion
    {
        public String LatestVersion;
        public String ProductCode;
        public String[] SupportedVersions;
        public ProductVersion()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
