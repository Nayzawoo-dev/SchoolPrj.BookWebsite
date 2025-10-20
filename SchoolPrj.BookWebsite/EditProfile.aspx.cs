using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using static SchoolPrj.BookWebsite._Login;

namespace SchoolPrj.BookWebsite
{
    public partial class EditProfile : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserId"] != null)
            {
                using (var connection = new SqlConnection(connectionString))
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

            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadProfile();
            }

            // Hide password fields for Google users
            if (Session["google"] != null && Session["google"].ToString() == "google")
            {
                Opw.Visible = false;
                Pw.Visible = false;
                ConfirmPw.Visible = false;
                pwchange.Visible = false;
            }
        }

        private void LoadProfile()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Users WHERE UserId = @UserId";
                string query = "SELECT * FROM Admins WHERE UserId = @UserId";

                var user = connection.QuerySingleOrDefault<dynamic>(sql, new { UserId = Session["UserId"] });
                var admin = connection.QuerySingleOrDefault<dynamic>(query, new { UserId = Session["UserId"] });

                if (admin != null)
                {
                    txtEmail.Text = admin.Email;
                    imgProfile.ImageUrl = admin.ProfileImagePath;
                }
                else if (user != null)
                {
                    txtEmail.Text = user.Email;
                    imgProfile.ImageUrl = user.ProfileImagePath;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            using (var connection = new SqlConnection(connectionString))
            {
                var existname = connection.Query<User>("select * from Users where Username = @Username ", new
                {
                    Username = fullName,
                }).ToList();
                var existadmin = connection.Query<Admins>("select * from Admins where Username = @Username ", new
                {
                    Username = fullName,
                }).ToList();
                if (existname.Count > 0 || existadmin.Count > 0)
                {
                    ShowMessage("Username Already Exist", "warning");
                    return;
                }
            }

            if (string.IsNullOrEmpty(fullName))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = "SELECT * FROM Users WHERE UserId = @UserId";
                    string query = "SELECT * FROM Admins WHERE UserId = @UserId";

                    var user = connection.QuerySingleOrDefault<User>(sql, new { UserId = Session["UserId"] });
                    var admin = connection.QuerySingleOrDefault<Admins>(query, new { UserId = Session["UserId"] });

                    if (admin != null)
                    {
                        fullName = admin.Username;
                    }
                    else if (user != null)
                    {
                        fullName = user.Username;
                    }
                }
            }

            // Check if password change is requested
            bool isChangingPassword = !string.IsNullOrEmpty(newPassword);

            if (isChangingPassword)
            {
                // Validate new password
                if (newPassword.Length < 6)
                {
                    ShowMessage("*New password must be at least 6 characters long.", "error");
                    return;
                }

                if (newPassword != confirmPassword)
                {
                    ShowMessage("*New passwords do not match.", "error");
                    return;
                }

                // Check old password for non-Google users
                if (Session["google"] == null || Session["google"].ToString() != "google")
                {
                    if (string.IsNullOrEmpty(oldPassword))
                    {
                        ShowMessage("*Please enter your old password to change password.", "error");
                        return;
                    }

                    // Verify old password
                    if (!VerifyOldPassword(oldPassword))
                    {
                        ShowMessage("*Old password is incorrect. Please enter your current password.", "error");
                        return;
                    }
                }
            }

            // Handle profile image upload
            string profilePath = imgProfile.ImageUrl; // default old image

            if (fileUploadProfile.HasFile)
            {
                string ext = Path.GetExtension(fileUploadProfile.FileName).ToLower();
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

                if (Array.IndexOf(allowedExtensions, ext) == -1)
                {
                    ShowMessage("⚠ Only image files (JPG, JPEG, PNG, GIF, BMP) are allowed.", "error");
                    return;
                }

                // Validate file size (max 5MB)
                if (fileUploadProfile.PostedFile.ContentLength > 5 * 1024 * 1024)
                {
                    ShowMessage("⚠ Image size should be less than 5MB.", "error");
                    return;
                }

                string folderPath = Server.MapPath("/uploads/images/");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fileName = Guid.NewGuid() + Path.GetExtension(fileUploadProfile.FileName);
                string filePath = Path.Combine(folderPath, fileName);
                fileUploadProfile.SaveAs(filePath);
                profilePath = "/uploads/images/" + fileName;
            }

            try
            {
                // Update user information based on role
                if (Session["Role"].ToString() == "User")
                {
                    UpdateUserProfile(fullName, email, profilePath, newPassword, isChangingPassword);
                }
                else if (Session["Role"].ToString() == "Admin")
                {
                    UpdateAdminProfile(fullName, email, profilePath, newPassword, isChangingPassword);
                }

                // Update related tables (comments, replies)
                UpdateRelatedTables(fullName, profilePath);

                // Update session
                Session["Username"] = fullName;
                Session["ProfileImage"] = profilePath;
                Session["ProfileMessage"] = "Profile updated successfully!";

                Response.Redirect("~/Profile.aspx");
            }
            catch (Exception ex)
            {
                ShowMessage($"*Error updating profile: {ex.Message}", "error");
            }
        }

        private bool VerifyOldPassword(string oldPassword)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                if (Session["Role"].ToString() == "User")
                {
                    var user = connection.QueryFirstOrDefault<dynamic>(
                        "SELECT PasswordHash, PasswordSalt FROM Users WHERE UserId = @UserId",
                        new { UserId = Session["UserId"] });

                    if (user != null && user.PasswordHash != null && user.PasswordSalt != null)
                    {
                        return VerifyPassword(oldPassword, user.PasswordHash, user.PasswordSalt);
                    }
                }
                else if (Session["Role"].ToString() == "Admin")
                {
                    var admin = connection.QueryFirstOrDefault<dynamic>(
                        "SELECT Password FROM Admins WHERE UserId = @UserId",
                        new { UserId = Session["UserId"] });

                    if (admin != null)
                    {
                        return oldPassword == admin.Password;
                    }
                }
            }
            return false;
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            try
            {
                var saltBytes = Convert.FromBase64String(storedSalt);
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
                {
                    var hash = Convert.ToBase64String(pbkdf2.GetBytes(32));
                    return hash == storedHash;
                }
            }
            catch
            {
                return false;
            }
        }

        private void UpdateUserProfile(string fullName, string email, string profilePath, string newPassword, bool isChangingPassword)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql;
                object parameters;
                if (isChangingPassword)
                {
                    // Generate new salt and hash for new password
                    var saltBytes = new byte[16];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(saltBytes);
                    }
                    string salt = Convert.ToBase64String(saltBytes);
                    string hashedPassword = HashPassword(newPassword, salt);

                    sql = @"UPDATE Users SET Username = @Username, Email = @Email, 
                            ProfileImagePath = @ProfileImagePath, PasswordHash = @PasswordHash, 
                            PasswordSalt = @PasswordSalt, UpdatedDate = GETDATE()
                            WHERE UserId = @UserId";

                    parameters = new
                    {
                        Username = fullName,
                        Email = email,
                        ProfileImagePath = profilePath,
                        PasswordHash = hashedPassword,
                        PasswordSalt = salt,
                        UserId = Session["UserId"]
                    };
                }
                else
                {
                    sql = @"UPDATE Users SET Username = @Username, Email = @Email, 
                            ProfileImagePath = @ProfileImagePath, UpdatedDate = GETDATE()
                            WHERE UserId = @UserId";

                    parameters = new
                    {
                        Username = fullName,
                        Email = email,
                        ProfileImagePath = profilePath,
                        UserId = Session["UserId"]
                    };
                }

                connection.Execute(sql, parameters);
            }
        }

        private void UpdateAdminProfile(string fullName, string email, string profilePath, string newPassword, bool isChangingPassword)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql;
                object parameters;
                if (isChangingPassword)
                {
                    sql = @"UPDATE Admins SET Username = @Username, Email = @Email, 
                            ProfileImagePath = @ProfileImagePath, Password = @Password
                            WHERE UserId = @UserId";

                    parameters = new
                    {
                        Username = fullName,
                        Email = email,
                        ProfileImagePath = profilePath,
                        Password = newPassword,
                        UserId = Session["UserId"]
                    };
                }
                else
                {
                    sql = @"UPDATE Admins SET Username = @Username, Email = @Email, 
                            ProfileImagePath = @ProfileImagePath
                            WHERE UserId = @UserId";

                    parameters = new
                    {
                        Username = fullName,
                        Email = email,
                        ProfileImagePath = profilePath,
                        UserId = Session["UserId"]
                    };
                }

                connection.Execute(sql, parameters);
            }
        }

        private void UpdateRelatedTables(string fullName, string profilePath)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var res = connection.Query<Reply>("select * from Tb_Reply");
                foreach (var item in res)
                {
                    if (item.ReplyUserName == Session["Username"].ToString() && item.UserName == Session["Username"].ToString())
                    {
                        connection.Execute("Update Tb_Reply SET ReplyUserName = @ReplyUserName,UserName = @UserName Where ReplyUserName = @OldReplyUserName AND UserName = @OldUserName", new
                        {
                            ReplyUserName = fullName,
                            UserName = fullName,
                            OldReplyUserName = item.ReplyUserName,
                            OldUserName = item.UserName,
                        });
                    }
                    if (item.ReplyUserName == Session["Username"].ToString())
                    {
                        connection.Execute("Update Tb_Reply SET ReplyUserName = @Newname where ReplyUserName = @Oldname", new
                        {
                            Newname = fullName,
                            Oldname = Session["Username"]
                        });
                    }
                    if (item.UserName == Session["Username"].ToString())
                    {
                        connection.Execute("Update Tb_Reply SET UserName = @Newname where UserName = @Oldname", new
                        {
                            Newname = fullName,
                            Oldname = Session["Username"]
                        });
                    }
                }


                // Update user profile in replies
                connection.Execute("UPDATE Tb_Reply SET UserProfile = @UserProfile WHERE UserId = @UserId",
                    new { UserProfile = profilePath, UserId = Session["UserId"] });
                // Update Tb_Comment table
                connection.Execute("UPDATE Tb_Comment SET UserName = @UserName, UserProfile = @UserProfile WHERE UserId = @UserId",
                    new { UserName = fullName, UserProfile = profilePath, UserId = Session["UserId"] });
            }
        }

        private string HashPassword(string password, string salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256))
            {
                return Convert.ToBase64String(deriveBytes.GetBytes(32));
            }
        }

        private void ShowMessage(string msg, string type = "info")
        {
            lblMessage.Text = msg;
            lblMessage.Visible = true;

            switch (type.ToLower())
            {
                case "success":
                    lblMessage.CssClass = "text-green-700 mb-4 block text-sm bg-green-50 p-3 rounded-lg text-center border border-green-200";
                    break;
                case "error":
                    lblMessage.CssClass = "text-red-700 mb-4 block text-sm bg-red-50 p-3 rounded-lg text-center border border-red-200";
                    break;
                case "warning":
                    lblMessage.CssClass = "text-yellow-700 mb-4 block text-sm bg-yellow-50 p-3 rounded-lg text-center border border-yellow-200";
                    break;
                default:
                    lblMessage.CssClass = "text-blue-700 mb-4 block text-sm bg-blue-50 p-3 rounded-lg text-center border border-blue-200";
                    break;
            }
        }

    }
}