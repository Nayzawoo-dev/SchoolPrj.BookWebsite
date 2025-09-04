using System;
using System.Data.SqlClient;
using Dapper;
using System.Security.Cryptography;
using System.Configuration;
using System.Text.RegularExpressions;

namespace SchoolPrj.BookWebsite
{
    public partial class Register : System.Web.UI.Page
    {

        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            string fullName = txtFullName.Text.Trim().Replace(" ", "");
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = "User";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "*Username and Password are required.";
                lblMessage.Visible = true;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                return;
            }

            if (email.Contains(" ") || username.Contains(" ") || password.Contains(" "))
            {
                lblMessage.Text = "*Email and Username and Password are must no space";
                lblMessage.Visible = true;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                return;
            }

            // Generate Salt + Hash
            string salt, hash;
            CreatePasswordHash(password, out hash, out salt);

            // Profile image upload
            string profilePath = "/uploads/images/default.png";
            if (fuProfile.HasFile)
            {
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(fuProfile.FileName);
                profilePath = "/uploads/images/" + fileName;
                fuProfile.SaveAs(Server.MapPath(profilePath));
            }

            using (var connection = new SqlConnection(connStr))
            {
                string check1 = "select * from Users where Username = @Username";

                var result1 = connection.QueryFirstOrDefault<User>(check1, new User { Username = username });

                if (result1 != null)
                {
                    lblMessage.Text = "*Username already exists!";
                    lblMessage.Visible = true;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                    return;
                }

                string pattern = @"^[a-zA-Z0-9._]{8,16}@gmail\.com$";

                if (Regex.IsMatch(email, "[A-Z]"))
                {
                    lblMessage.Text = "*Email must be in lowercase.";
                    lblMessage.Visible = true;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                    return;
                }
                if (!Regex.IsMatch(email, pattern))
                {
                    lblMessage.Text = "*Email must be a valid Gmail address and between 8 to 14 characters long.";
                    lblMessage.Visible = true;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                    return;
                }
                string check = "select * from Users where Email = @Email";
                var result = connection.QueryFirstOrDefault<User>(check, new User { Email = email });
                if (result != null)
                {
                    lblMessage.Text = "*Email already exists!";
                    lblMessage.Visible = true;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                    return;
                }

                var ban = connection.QueryFirstOrDefault<Ban>(
                    "SELECT * FROM Tb_BanList WHERE UserEmail = @UserEmail",
                    new Ban { UserEmail = email });

                if (ban != null)
                {
                    lblMessage.Text = "Your email has been banned!";
                    lblMessage.Visible = true;
                    // Session clear
                    Session.Clear();
                    Session.Abandon();

                    // Authentication cookie (ASP.NET Forms Auth သုံးထားရင်)
                    if (Request.Cookies["UserAuth"] != null)
                    {
                        Response.Cookies["UserAuth"].Expires = DateTime.Now.AddDays(-1);
                    }

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                    return;
                }

                string sql = @"INSERT INTO Users (Username, PasswordHash, PasswordSalt, FullName, Email, Role, ProfileImagePath)
                               VALUES (@Username, @PasswordHash, @PasswordSalt, @FullName, @Email, @Role, @ProfileImagePath)";

                try
                {
                    connection.Execute(sql, new User
                    {
                        Username = username,
                        PasswordHash = hash,
                        PasswordSalt = salt,
                        FullName = fullName,
                        Email = email,
                        Role = role,
                        ProfileImagePath = profilePath
                    });
                    Session["RegisterMessage"] = "Registration successful! You can now log in.";
                    Response.Redirect("~/Login.aspx");

                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        private void CreatePasswordHash(string password, out string hash, out string salt)
        {
            // Generate Salt
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            salt = Convert.ToBase64String(saltBytes);

            // Generate Hash
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32); // 256-bit
                hash = Convert.ToBase64String(hashBytes);
            }
        }
    }
}
