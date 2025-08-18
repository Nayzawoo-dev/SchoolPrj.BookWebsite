using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using System.Security.Cryptography;
using System.Configuration;
using static SchoolPrj.BookWebsite._Login;

namespace SchoolPrj.BookWebsite
{
    public partial class EditProfile : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        private void LoadProfile()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Users WHERE UserId = @UserId";
                string query = "select * from Admins where UserId = @UserId";
                var user = connection.QuerySingleOrDefault(sql, new { UserId = Session["UserId"] });
                var admin = connection.QuerySingleOrDefault(query, new { UserId = Session["UserId"] });
                if (admin != null)
                {
                    txtFullName.Text = admin.Username;
                    txtEmail.Text = admin.Email;
                    imgProfile.ImageUrl = admin.ProfileImagePath;
                }
                if (user != null)
                {
                    txtFullName.Text = user.Username;
                    txtEmail.Text = user.Email;
                    imgProfile.ImageUrl = user.ProfileImagePath;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            string profilePath = imgProfile.ImageUrl; // default old image

            // Upload new profile image if selected
            if (fileUploadProfile.HasFile)
            {
                string folderPath = Server.MapPath("~/uploads/");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fileName = Guid.NewGuid() + Path.GetExtension(fileUploadProfile.FileName);
                string filePath = Path.Combine(folderPath, fileName);
                fileUploadProfile.SaveAs(filePath);
                profilePath = "~/uploads/" + fileName;
            }

            // Hash password if changed
            if (Session["Role"].ToString() == "User")
            {
                string sql;
                object parameters;
                if (!string.IsNullOrEmpty(password))
                {
                    var saltBytes = new byte[16];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(saltBytes);
                    }
                    string salt = Convert.ToBase64String(saltBytes);

                    string hashedPassword = HashPassword(password, salt);

                    sql = @"UPDATE Users SET Username=@Username, Email=@Email, ProfileImagePath=@ProfileImagePath,
                        PasswordHash=@PasswordHash, PasswordSalt=@PasswordSalt
                        WHERE UserId=@UserId";
                    parameters = new { Username = fullName, Email = email, ProfileImagePath = profilePath, PasswordHash = hashedPassword, PasswordSalt = salt, UserId = Session["UserId"] };
                }
                else
                {
                    sql = @"UPDATE Users SET Username=@Username, Email=@Email, ProfileImagePath=@ProfileImagePath
                        WHERE UserId=@UserId";
                    parameters = new { Username = fullName, Email = email, ProfileImagePath = profilePath, UserId = Session["UserId"] };
                }

                using (var connection = new SqlConnection(connectionString))
                {
                    var res = connection.QuerySingleOrDefault("select * from Users where Username= @Username", new User { Username = fullName });
                    if (res != null)
                    {
                        lblMessage.CssClass = "text-red-600 font-medium";
                        lblMessage.Text = "❌ Username already exists!";
                        return;
                    }
                    connection.Execute(sql, parameters);
                    goto result;
                }

            }

            if (Session["Role"].ToString() == "Admin")
            {
                string sql;
                object parameters;
                if (!string.IsNullOrEmpty(password))
                {
                    sql = @"UPDATE Admins SET Username=@Username, Email=@Email, ProfileImagePath=@ProfileImagePath WHERE UserId = @UserId";
                    parameters = new { Username = fullName, Email = email, ProfileImagePath = profilePath, Password = password, UserId = Session["UserId"] };
                }
                sql = @"UPDATE Admins SET Username=@Username, Email=@Email, ProfileImagePath=@ProfileImagePath, Password = @Password WHERE UserId = @UserId";
                parameters = new {Username = fullName, Email = email, ProfileImagePath = profilePath, Password = password, UserId = Session["UserId"] };

                using(var connection = new SqlConnection(connectionString))
                {
                    var res = connection.QuerySingleOrDefault("select * from Admins where Username= @Username", new Admins { Username = fullName });
                    if (res != null)
                    {
                        lblMessage.CssClass = "text-red-600 font-medium";
                        lblMessage.Text = "❌ Username already exists!";
                        return;
                    }
                    connection.Execute(sql, parameters);
                    goto result;
                }
            }

        result:
            // Update session (so master page reflects new data)
            Session["Username"] = fullName;
            Session["ProfileImage"] = profilePath;
            Session["ProfileMessage"] = "Profile updated successfully!";
            Response.Redirect("~/Profile.aspx");
        }

        private string HashPassword(string password, string salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000))
            {
                return Convert.ToBase64String(deriveBytes.GetBytes(32));
            }
        }
    }
}
