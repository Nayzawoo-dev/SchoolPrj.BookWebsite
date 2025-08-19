using System;
using System.Data.SqlClient;
using Dapper;
using System.Security.Cryptography;
using System.Web;
using System.Configuration;

namespace SchoolPrj.BookWebsite
{
    public partial class _Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RegisterMessage"] != null)
            {
                lblMessage.CssClass = "text-green-500";
                lblMessage.Text = Session["RegisterMessage"].ToString();
                Session["RegisterMessage"] = null;

                Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
            }
        }
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection(connStr))
            {
                var user = connection.QuerySingleOrDefault<User>(
                    "SELECT * FROM Users WHERE Username = @Username or Email = @Email",
                    new User { Username = txtUsername.Text.Trim(), Email = txtUsername.Text.Trim() });


                var admin = connection.QueryFirstOrDefault<Admins>("SELECT * FROM Admins WHERE Username = @Username or Email = @Email",
                      new Admins { Username = txtUsername.Text.Trim(), Email = txtUsername.Text.Trim() });


                if (admin != null)
                {
                    bool res = CheckPassword(txtPassword.Text.Trim());
                    if(res != true)
                    {
                        goto ERROR1;
                    }
                    Session["UserId"] = admin.UserId;
                    Session["Username"] = admin.Username;
                    Session["Role"] = admin.Role;
                    Session["ProfileImage"] = admin.ProfileImagePath;

                    // Cookie (3 days login)
                    HttpCookie authCookie = new HttpCookie("UserAuth");
                    authCookie.Values["UserId"] = admin.UserId.ToString();
                    authCookie.Expires = DateTime.Now.AddDays(3);
                    Response.Cookies.Add(authCookie);
                    Response.Redirect("Book.aspx");
                }

            ERROR1:
                if (user != null && VerifyPassword(txtPassword.Text, user.PasswordHash, user.PasswordSalt))
                {
                    // Save Session
                    Session["UserId"] = user.UserId;
                    Session["Username"] = user.Username;
                    Session["Role"] = user.Role;
                    Session["ProfileImage"] = user.ProfileImagePath;

                    // Cookie (3 days login)
                    HttpCookie authCookie = new HttpCookie("UserAuth");
                    authCookie.Values["UserId"] = user.UserId.ToString();
                    authCookie.Expires = DateTime.Now.AddDays(3);
                    Response.Cookies.Add(authCookie);
                    Response.Redirect("Book.aspx");
                }
                else
                {
                    lblMessage.Text = "Invalid username or password!";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                }
            }
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                var hash = Convert.ToBase64String(pbkdf2.GetBytes(32));
                return hash == storedHash;
            }
        }

        private bool CheckPassword(string Password)
        {
            object res;
            using (var connection = new SqlConnection(connStr))
            {
                res = connection.QueryFirstOrDefault<Admins>("select * from Admins where Password = @Password",new {Password = Password});               
            }
            if(res == null)
            {
                return false;
            }
            return true;
        }
    }
}