using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace SchoolPrj.BookWebsite
{
    public partial class _Default : Page
    {
        private string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserId"] != null)
            {
                using (var connection = new SqlConnection(connStr))
                {
                    var res = connection.QueryFirstOrDefault<User>(
                        "select * from Users where UserId = @UserId",
                        new { UserId = HttpContext.Current.Session["UserId"] });

                    if (res != null && res.BanStatus > 0)
                    {
                        HttpContext.Current.Session.Clear();
                        HttpContext.Current.Session.Abandon();

                        if (HttpContext.Current.Request.Cookies["UserAuth"] != null)
                        {
                            HttpContext.Current.Response.Cookies["UserAuth"].Expires = DateTime.Now.AddDays(-1);
                        }

                        HttpContext.Current.Response.Redirect("~/Login.aspx?msg=banned", true);
                    }
                }
            }
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {
                    HyperLink1.Visible = false;
                    lnkRegister.Visible = false;
                }
                else
                {
                    HyperLink1.Visible = true;
                    lnkRegister.Visible = true;
                }
            }
            if (Session["UserId"] != null && Session["Role"].ToString() == "Admin")
            {
                // Session clear
                Session.Clear();
                Session.Abandon();

                // Authentication cookie (ASP.NET Forms Auth သုံးထားရင်)
                if (Request.Cookies["UserAuth"] != null)
                {
                    Response.Cookies["UserAuth"].Expires = DateTime.Now.AddDays(-1);
                }

                // Website အစ Login Page သို့ ပြန်ပို့
                Response.Redirect("~/Login.aspx");
            }
        }
    }
}