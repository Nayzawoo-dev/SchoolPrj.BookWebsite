using System;
using System.Web;
using System.Web.Configuration;

namespace SchoolPrj.BookWebsite
{
    public partial class GoogleLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string clientId = WebConfigurationManager.AppSettings["GoogleClientId"];
            string redirectUri = "https://localhost:44339/GoogleCallback.aspx";
            string scope = HttpUtility.UrlEncode("openid email profile");
            string state = Guid.NewGuid().ToString(); // CSRF protection

            // Store state in session for later verification in callback
            Session["GoogleOAuthState"] = state;

            string url = $"https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id={clientId}&redirect_uri={HttpUtility.UrlEncode(redirectUri)}&scope={scope}&state={state}";

            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest(); // Ensures immediate redirect in ASP.NET
        }
    }
}