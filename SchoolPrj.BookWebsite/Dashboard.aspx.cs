using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolPrj.BookWebsite
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindUsers();
                total();
            if (Session["UserId"] == null || Session["Role"].ToString() != "Admin")
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindUsers(txtSearch.Text.Trim());
        }

        protected void btnUnderstand_Click(object sender, EventArgs e)
        {
            pnlNoUsers.Visible = false;
            BindUsers();
        }

        private void BindUsers(string search = "")
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql;
                if (!string.IsNullOrEmpty(search))
                {
                    sql = "SELECT UserId, Username, Email, BanStatus FROM Users where IsEmailConfirmed = 1 AND (Username LIKE @Search OR Email LIKE @Search)";
                    search = "%" + search + "%";
                }else
                {
                   sql = "SELECT UserId, Username, Email, BanStatus FROM Users where IsEmailConfirmed = 1";
                }

                var users = connection.Query<User>(sql, new { Search = search }).ToList();

                pnlNoUsers.Visible = users.Count == 0;
                gvUsers.DataSource = users;
                gvUsers.DataBind();
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Ban")
            {
                UpdateBanStatus(userId, 1);
                DeleteStatus(userId);
            }

            else if (e.CommandName == "Unban")
            {
                UpdateBanStatus(userId, 0);
            }

            BindUsers();
        }

        private void UpdateBanStatus(int userId, int status)
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql = "UPDATE Users SET BanStatus = @status WHERE UserId = @userId";
                connection.Execute(sql, new { status, userId });
            }
        }

        private void DeleteStatus(int userId)
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql = "Delete from Tb_Comment WHERE UserId = @userId";
                string sql2 = "Delete from Tb_React WHERE UserId = @userId";
                string sql3 = "Delete from Tb_Reply WHERE UserId = @userId";
                connection.Execute(sql, new { userId });
                connection.Execute(sql2, new { userId });
                connection.Execute(sql3, new { userId });
            }
        }

        protected void total()
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql = "SELECT COUNT(*) FROM Users where IsEmailConfirmed = 1";
                string sql2 = "SELECT COUNT(*) FROM Tb_Book";
                var totalUsers = connection.ExecuteScalar<int>(sql);
                var totalBooks = connection.ExecuteScalar<int>(sql2);
                lblTotalUsers.Text = totalUsers.ToString();
                lblTotalBook.Text = totalBooks.ToString();
            }
        }
    }
}
