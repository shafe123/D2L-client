using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using D2LDotNet;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using D2LDotNet.Model;
using D2LDotNet.API;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ProductVersion[] versions = APIProperties.GetProductVersions();
        foreach (ProductVersion v in versions)
        {
            Response.Write(v.ProductCode + ": " + v.LatestVersion + "<br />Supported Versions:<br />");
            foreach (string v2 in v.SupportedVersions)
                Response.Write(v2 + "<br />");
            Response.Write("<br />");
        }
    }
}