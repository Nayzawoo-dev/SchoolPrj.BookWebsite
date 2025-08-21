using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;

namespace SchoolPrj.BookWebsite
{
    public partial class _Profile : System.Web.UI.Page
    {

        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadProfile();
                BindSavedBooks();
            }

            if (Session["ProfileMessage"] != null)
            {
                lblMessage.Text = Session["ProfileMessage"].ToString();
                Session["ProfileMessage"] = null;
            }
        }

        private void LoadProfile()
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql = @"SELECT Username, Email, Role, ProfileImagePath FROM Users WHERE UserId = @UserId";
                var user = connection.QuerySingleOrDefault(sql, new { UserId = Session["UserId"] });
                if (user == null)
                {
                    string query = @"SELECT Username, Email, Role, ProfileImagePath FROM Admins WHERE UserId = @UserId";
                    var admin = connection.QuerySingleOrDefault(query, new { UserId = Session["UserId"] });
                    lblFullName.Text = admin.Username;
                    lblEmail.Text = admin.Email;
                    lblRole.Text = admin.Role;
                    imgProfile.ImageUrl = admin.ProfileImagePath;
                }
                if (user != null)
                {
                    lblFullName.Text = user.Username;
                    lblEmail.Text = user.Email;
                    lblRole.Text = user.Role;
                    imgProfile.ImageUrl = user.ProfileImagePath;
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
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

        private void BindSavedBooks()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            using (var con = new SqlConnection(connStr))
            {
                var books = con.Query<Save>("SELECT * FROM Tb_SaveBook WHERE UserId = @UserId", new Save{ UserId = userId }).ToList();
                rptSavedBooks.DataSource = books;
                pnlSaveSection.Visible = books.Count > 0;
                rptSavedBooks.DataBind();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int saveId = Convert.ToInt32(btn.CommandArgument);

            // Delete logic here  
            DeleteBook(saveId);
            BindSavedBooks();
        }

        protected void DeleteBook(int saveId)
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql = @"DELETE FROM [dbo].[Tb_SaveBook]
      WHERE SaveId = @SaveId";
                var res = connection.Execute(sql, new Save
                {
                    SaveId = saveId
                });
            }
        }

    }
}