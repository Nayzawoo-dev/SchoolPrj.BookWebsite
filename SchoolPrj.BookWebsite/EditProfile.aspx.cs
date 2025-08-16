using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using System.Security.Cryptography;
using System.Configuration;

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
                var user = connection.QuerySingleOrDefault(sql, new { UserId = Session["UserId"] });

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
                connection.Execute(sql, parameters);
            }

            // Update session (so master page reflects new data)
            Session["Username"] = fullName;
            Session["ProfileImage"] = profilePath;

            lblMessage.CssClass = "text-green-600 font-medium";
            lblMessage.Text = "✅ Profile updated successfully!";
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
