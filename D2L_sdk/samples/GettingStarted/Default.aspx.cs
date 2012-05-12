using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using D2L.Extensibility.AuthSdk;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections;
using System.Text;
using D2L.Extensibility.AuthSdk;
using System.Web.Script.Serialization;

public partial class _Default : System.Web.UI.Page
{
    protected const String _defaultAppID = "G9nUpvbZQyiPrk3um2YAkQ";
    protected const String _defaultAppKey = "ybZu7fm_JKJTFwKEHfoZ7Q";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["appID"] == null)
        {
            Session["appID"] = _defaultAppID;
            Session["appKey"] = _defaultAppKey;
        }
        appIDField.Text = Session["appID"].ToString();
        appKeyField.Text = Session["appKey"].ToString();
        hostField.Text = "valence.desire2learn.com";
        if (!Page.IsPostBack)
        {
            var factory = new D2LAppContextFactory();

            //assigns the AppID and AppKey into the AppID and AppKey in the AppContext
            var appContext = factory.Create(Session["appID"].ToString(), Session["appKey"].ToString());

            int port;
            try
            {
                //try to read the port specified
                port = Convert.ToInt32(portField.Text);
            }
            catch (Exception error)
            {
                //fallback to https port
                port = 443;
            }
            //holds the scheme, hostname and which port to use
            HostSpec hostInfo = new HostSpec("https", hostField.Text, port);

            //saves the callback url as well as sets up the AppID, AppKey, UserID, and UserKey
            //if run without x_a, x_b, will be null
            ID2LUserContext userContext = appContext.CreateUserContext(Request.Url, hostInfo);
            if (userContext != null)
            {
                
                //This is how applications may want to store the info and handle authentication. This example must deal with changing information and therefore
                //cannot rely on an important variable being the same.
                Session["userContext"] = userContext;
                UserContextProperties properties = userContext.SaveUserContextProperties();
                Session["userID"] = properties.UserId;
                Session["userKey"] = properties.UserKey;
            }
            
        }
        if (Session["userID"] != null)
        {
            userIDField.Text = Session["userID"].ToString();
            userKeyField.Text = Session["userKey"].ToString();
        }

    }

    protected void GetVersions(object sender, EventArgs e)
    {
        D2LAppContextFactory factory = new D2LAppContextFactory();
        ID2LAppContext appContext = factory.Create(Session["appID"].ToString(), Session["appKey"].ToString());

        int port;
        try
        {
            //try to read the port specified
            port = Convert.ToInt32(portField.Text);
        }
        catch (Exception)
        {
            //fallback to https port
            port = 443;
        }

        HostSpec hostInfo = new HostSpec("https", hostField.Text, port);

        //anonymous user context has the standard AppID/AppKey with null UserID/UserKey
        //grabs the apiHost information(scheme, hostname, port) and stores that as well
        ID2LUserContext userContext = appContext.CreateAnonymousUserContext(
        hostInfo);
        CallGetVersions(userContext, 2);
    }

    protected void CallGetVersions(ID2LUserContext userContext, int retryAttempts)
    {
        //in this case we are using an anonymous user
        Uri uri = userContext.CreateAuthenticatedUri(
            "/d2l/api/versions/", "GET");

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.Method = "GET";

        try
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string responseBody = reader.ReadToEnd();

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        ProductVersions[] versions = serializer.Deserialize<ProductVersions[]>(responseBody);
                        String output = "";
                        foreach (ProductVersions v in versions)
                        {
                            output += v.ProductCode + ": " + v.LatestVersion + "<br />";
                        }
                        resultBox.InnerHtml = output;

                    }
                }
            }
        }
        //catches status codes in the 4 and 5 hundred series
        catch (WebException we)
        {
            D2LWebException exceptionWrapper = new D2LWebException(we);

            RequestResult result = userContext.InterpretResult(exceptionWrapper);
            switch (result)
            {
                //if the timestamp is invalid and we haven't exceeded the retry limit then the call is made again with the adjusted timestamp
                case RequestResult.RESULT_INVALID_TIMESTAMP:
                    if (retryAttempts > 0)
                    {
                        CallWhoAmI(userContext, retryAttempts - 1);
                    }
                    break;

            }
        
            
        }
    }

    protected void Authenticate(object sender, EventArgs e)
    {
        //These would normally not be stored in the session and would not change. In this sample we save them to the session to accomodate testing to see new ID's and keys are working. 
        Session["appID"] = appIDField.Text;
        Session["appKey"] = appKeyField.Text;
        
        D2LAppContextFactory factory = new D2LAppContextFactory();
        
        ID2LAppContext appContext = factory.Create(Session["appID"].ToString(), Session["appKey"].ToString());
        
        int port;
        try
        {
            //try to read the port specified
            port = Convert.ToInt32(portField.Text);
        }
        catch (Exception)
        {
            //fallback to https port
            port = 443;
        }


        Uri resultUri = new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port, Request.Url.AbsolutePath).Uri;
        Uri uri = appContext.CreateUrlForAuthentication(new HostSpec("https",hostField.Text, port), resultUri);

        Response.Redirect(uri.ToString());
    }

    protected void WhoAmI(object sender, EventArgs e)
    {
        //if the settings couldn't be changed between calls the next few steps would be replaced with using Session["userContext"] 
        D2LAppContextFactory factory = new D2LAppContextFactory();
        ID2LAppContext appContext = factory.Create(Session["appID"].ToString(), Session["appKey"].ToString());

        int port;
        try
        {
            //try to read the port specified
            port = Convert.ToInt32(portField.Text);
        }
        catch (Exception error)
        {
            //fallback to https port
            port = 443;
        }

        String userID = "t";
        String userKey = "t";
        if (Session["userID"] != null)
        {
            userID = Session["userID"].ToString(); 
            userKey = Session["userKey"].ToString();
        }
        HostSpec hostInfo = new HostSpec("https", hostField.Text, port);
        ID2LUserContext userContext = appContext.CreateUserContext(
           userID, userKey, hostInfo);
        CallWhoAmI(userContext, 2);
       
    }

    protected void CallWhoAmI(ID2LUserContext userContext,int retryAttempts)
    {
         Uri uri = userContext.CreateAuthenticatedUri(
            "/d2l/api/lp/1.0/users/whoami?fakeparam=lol", "GET");

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.Method = "GET";
        request.AllowAutoRedirect = false;

        try
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string responseBody = reader.ReadToEnd();
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        WhoAmIUser user = serializer.Deserialize<WhoAmIUser>(responseBody);
                        resultBox.InnerHtml = "<div>First Name: " + user.FirstName + "</div><div>Last Name: " + user.LastName + "</div><div>D2LID: " + user.Identifier + "</div>"; ;
                    }
                }
            }
        }
        //catches status codes in the 4 and 5 hundred series
        catch (WebException we)
        {
            D2LWebException exceptionWrapper = new D2LWebException(we);
            RequestResult result = userContext.InterpretResult(exceptionWrapper);
            System.Diagnostics.Debug.WriteLine(result.ToString());
            switch (result)
            {
                //if there is no user or the current user doesn't have permission to perform the action
                case RequestResult.RESULT_INVALID_SIG:
                    resultBox.InnerHtml = "Error Must Authenticate as User With Permission";
                    break;
                //if the timestamp is invalid and we haven't exceeded the retry limit then the call is made again with the adjusted timestamp
                case RequestResult.RESULT_INVALID_TIMESTAMP:
                    if (retryAttempts > 0)
                    {
                        CallWhoAmI(userContext, retryAttempts - 1);
                    }
                    break;
            }
                   
        }
    }

    protected void ClearResults(object sender, EventArgs e)
    {
        resultBox.InnerHtml = "";
        resultHeading.InnerHtml = "";
    }

    protected void SaveInfo(object sender, EventArgs e)
    {
        Session["appID"] = appIDField.Text;
        Session["appKey"] = appKeyField.Text;
        Session["userID"] = userIDField.Text;
        Session["userKey"] = userKeyField.Text;
    }

    protected void ResetInfo(object sender, EventArgs e)
    {
        Session.Clear();
        //redirect without the query string so credentials are not reloaded
        Response.Redirect(Request.Url.AbsolutePath);
    }
}
