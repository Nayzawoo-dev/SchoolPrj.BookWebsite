using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolPrj.BookWebsite
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsers();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindUsers(txtSearch.Text.Trim());
        }

        protected void gvUsers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ban")
            {
                string email = e.CommandArgument.ToString();
                BanUser(email);
                BindUsers();
            }
        }

        public void BanUser(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string checkBan = "SELECT * FROM Users WHERE Email = @Email";

                var user = connection.QuerySingleOrDefault<User>(checkBan, new User { Email = email });
                // Add to BanList
                string insertBan = @"INSERT INTO [dbo].[Tb_BanList]
           ([UserId]
           ,[UserEmail]
           ,[Username]
           ,[PasswordHash]
           ,[PasswordSalt]
           ,[Role]
           ,[ProfileImagePath])
     VALUES
           (
            @UserId
           ,@UserEmail
           ,@Username
           ,@PasswordHash
           ,@PasswordSalt
           ,@Role
           ,@ProfileImagePath)";
                connection.Execute(insertBan, new Ban {UserId = user.UserId, UserEmail = user.Email, Username = user.Username, PasswordHash = user.PasswordHash, PasswordSalt = user.PasswordSalt, Role = user.Role, ProfileImagePath = user.ProfileImagePath });

                // Delete from Users
                string deleteUser = "DELETE FROM Users WHERE Email = @Email";
                string deleteReact = "DELETE FROM Tb_React WHERE UserId = @UserId";
                string deleteComments = "DELETE FROM Tb_Comment WHERE UserId = @UserId";
              int res  =  connection.Execute(deleteUser, new User { Email = email , UserId = user.UserId });
              int rest =  connection.Execute(deleteReact, new React{ UserId = user.UserId });
              int rei  =  connection.Execute(deleteComments, new _Comment { UserId = user.UserId }); 
            }
        }

        public void BindUsers(string search = "")
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users";
                if (!string.IsNullOrEmpty(search))
                {
                    query += " WHERE Email LIKE @Search OR Username LIKE @Search";
                    search = "%" + search + "%";
                }
                var users = connection.Query<User>(query, new { Search = search }).ToList();
                if (users.Count == 0)
                {
                    pnlNoUsers.Visible = true;
                }
                else
                {
                    pnlNoUsers.Visible = false;
                }
                gvUsers.DataSource = users;
                gvUsers.DataBind();
            }
        }

        protected void btnUnderstand_Click(object sender, EventArgs e)
        {
            BindUsers();
            pnlNoUsers.Visible = false;
        }

    }
}
