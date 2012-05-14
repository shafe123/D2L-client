using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using D2LDotNet;
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

        SupportedVersion support = APIProperties.GetSupportedVersions(D2LPRODUCT.ePortfolio, "1.0");
        Response.Write("Is 1.0 supported?" + "<br />" + support.Supported.ToString() + "<br/ >");

        support = APIProperties.GetSupportedVersions(D2LPRODUCT.ePortfolio, "2.0");
        Response.Write("Is 2.0 supported?" + "<br />" + support.Supported.ToString() + "<br/ >");
    }
}