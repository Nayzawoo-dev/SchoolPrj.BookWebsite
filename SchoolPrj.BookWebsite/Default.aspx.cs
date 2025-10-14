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
                    var res = connection.QueryFirstOrDefault<int>(
                        "SELECT COUNT(1) FROM Tb_BanList WHERE UserId = @UserId",
                        new { UserId = HttpContext.Current.Session["UserId"] });

                    if (res > 0)
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
        }
    }
}