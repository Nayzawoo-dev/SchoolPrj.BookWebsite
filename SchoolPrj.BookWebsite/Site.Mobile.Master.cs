using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolPrj.BookWebsite
{
    public partial class Site_Mobile : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                loginLink.Visible = false;
                loginLinkMobile.Visible = false;
                HyperLink1.Visible = false;
                HyperLink1Mobile.Visible = false;
                profilePanel.Visible = true;
                profilePanelMobile.Visible = true;
                lblUserName.Text = Session["Username"].ToString();
                lblUserNameMobile.Text = Session["Username"].ToString();
                imgProfile.ImageUrl = Session["ProfileImage"].ToString();
                imgProfileMobile2.ImageUrl = Session["ProfileImage"].ToString();
            }
            else
            {
                loginLink.Visible = true;
                loginLinkMobile.Visible = true;
                profilePanel.Visible = false;
                profilePanelMobile.Visible = false;
            }
        }
    }
}