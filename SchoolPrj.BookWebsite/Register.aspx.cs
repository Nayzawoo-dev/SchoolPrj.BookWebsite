using System;
using System.Data.SqlClient;
using Dapper;
using System.Security.Cryptography;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Web;

namespace SchoolPrj.BookWebsite
{
    public partial class Register : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if(Session["UserId"] != null && Session["Role"].ToString() == "User")
                {
                    Response.Redirect("~/Default.aspx");
                } else if(Session["UserId"] != null && Session["Role"].ToString() == "Admin")
                {
                    Response.Redirect("~/Book.aspx");
                }
            }
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            string fullName = txtFullName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = "User";

            // Validation
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowMessage("*All fields are required.", "error");
                return;
            }

            if (!IsValidEmail(email))
            {
                ShowMessage("*Please enter a valid email address.", "error");
                return;
            }

            if (password.Length < 6)
            {
                ShowMessage("*Password must be at least 6 characters long.", "error");
                return;
            }

            try
            {
                string salt, hash;
                CreatePasswordHash(password, out hash, out salt);

                string profilePath = "/uploads/images/default.jpg";
                if (fuProfile.HasFile)
                {
                    try
                    {
                        // Validate file type
                        string fileExtension = Path.GetExtension(fuProfile.FileName).ToLower();
                        if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png" && fileExtension != ".gif")
                        {
                            ShowMessage("*Please upload only image files (JPG, JPEG, PNG, GIF).", "error");
                            return;
                        }

                        // Validate file size (max 5MB)
                        if (fuProfile.PostedFile.ContentLength > 5 * 1024 * 1024)
                        {
                            ShowMessage("*Image size should be less than 5MB.", "error");
                            return;
                        }

                        // Create uploads directory if it doesn't exist
                        string uploadsDir = Server.MapPath("~/uploads/images/");
                        if (!Directory.Exists(uploadsDir))
                        {
                            Directory.CreateDirectory(uploadsDir);
                        }

                        string fileName = Guid.NewGuid().ToString() + fileExtension;
                        profilePath = "/uploads/images/" + fileName;
                        fuProfile.SaveAs(Server.MapPath(profilePath));
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("*Error uploading image: " + ex.Message, "error");
                        return;
                    }
                }

                using (var connection = new SqlConnection(connStr))
                {
                    // Check if username or email already exists
                    var existUser = connection.QueryFirstOrDefault(
                        "SELECT Username, Email FROM Users WHERE Username = @Username OR Email = @Email",
                        new { Username = username, Email = email });

                    if (existUser != null)
                    {
                        if (existUser.Username == username)
                            ShowMessage("*Username already exists!", "error");
                        else
                            ShowMessage("*Email already exists!", "error");
                        return;
                    }

                    string verificationCode = Guid.NewGuid().ToString();

                    string sql = @"INSERT INTO Users 
                    (Username, PasswordHash, PasswordSalt, FullName, Email, Role, ProfileImagePath, 
                     IsEmailConfirmed, EmailVerificationCode,BanStatus)
                    VALUES (@Username, @PasswordHash, @PasswordSalt, @FullName, @Email, @Role, 
                            @ProfileImagePath, 0, @EmailVerificationCode, 0)";

                    int rowsAffected = connection.Execute(sql, new
                    {
                        Username = username,
                        PasswordHash = hash,
                        PasswordSalt = salt,
                        FullName = fullName,
                        Email = email,
                        Role = role,
                        ProfileImagePath = profilePath,
                        EmailVerificationCode = verificationCode
                    });

                    if (rowsAffected > 0)
                    {
                        // Send verification email
                        bool emailSent = SendVerificationEmail(email, fullName, verificationCode);

                        if (emailSent)
                        {
                            ShowMessage("Please check your email (including spam folder) to verify your account.", "success");

                            // Clear form
                            txtFullName.Text = "";
                            txtUsername.Text = "";
                            txtEmail.Text = "";
                            txtPassword.Text = "";
                        }
                        else
                        {
                            ShowMessage("✔ Registration completed! But we couldn't send verification email. Please contact support.", "warning");
                        }
                    }
                    else
                    {
                        ShowMessage("*Registration failed. Please try again.", "error");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                ShowMessage("*Database error: " + sqlEx.Message, "error");
            }
            catch (Exception ex)
            {
                ShowMessage("*An error occurred: " + ex.Message, "error");
            }
        }

        private bool SendVerificationEmail(string toEmail, string fullName, string verificationCode)
        {
            try
            {
                string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                string verifyUrl = $"{baseUrl}/VerifyEmail.aspx?code={HttpUtility.UrlEncode(verificationCode)}";

                string subject = "Verify Your Email - MyBookShelf";
                string body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; color: white; }}
        .content {{ padding: 30px; background: #f9f9f9; }}
        .button {{ display: inline-block; padding: 12px 30px; background: #667eea; color: white; text-decoration: none; border-radius: 5px; font-weight: bold; }}
        .footer {{ padding: 20px; text-align: center; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>MyBookShelf</h1>
            <p>Welcome to our reading community!</p>
        </div>
        <div class='content'>
            <h2>Hello {fullName},</h2>
            <p>Thank you for registering with MyBookShelf. To complete your registration, please verify your email address by clicking the button below:</p>
            <p style='text-align: center;'>
                <a href='{verifyUrl}' class='button'>Verify Email Address</a>
            </p>
            <p>Or copy and paste this link in your browser:</p>
            <p style='word-break: break-all;'>{verifyUrl}</p>
            <p>This verification link will expire in 24 hours.</p>
            <p>If you didn't create an account with us, please ignore this email.</p>
        </div>
        <div class='footer'>
            <p>&copy; {DateTime.Now.Year} MyBookShelf. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("nayzawoo.ace@gmail.com", "MyBookShelf");
                    mail.To.Add(toEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.Normal;

                    // Add headers to prevent spam marking
                    mail.Headers.Add("X-Mailer", "MyBookShelf");
                    mail.Headers.Add("X-Priority", "3");
                    mail.Headers.Add("X-MSMail-Priority", "Normal");

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("nayzawoo.ace@gmail.com", "ijkz bsvx vsrp aukg");
                        smtp.EnableSsl = true;
                        smtp.Timeout = 30000; // 30 seconds timeout

                        smtp.Send(mail);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log email error (you can implement proper logging here)
                System.Diagnostics.Debug.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }

        private void CreatePasswordHash(string password, out string hash, out string salt)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            salt = Convert.ToBase64String(saltBytes);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                hash = Convert.ToBase64String(pbkdf2.GetBytes(32));
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ShowMessage(string msg, string type = "info")
        {
            lblMessage.Text = msg;
            lblMessage.Visible = true;

            // Set different styles based on message type
            switch (type.ToLower())
            {
                case "success":
                    lblMessage.CssClass = "bg-green-50 text-green-700 p-4 rounded-xl mb-6 block text-center border border-green-200";
                    break;
                case "error":
                    lblMessage.CssClass = "bg-red-50 text-red-700 p-4 rounded-xl mb-6 block text-center border border-red-200";
                    break;
                case "warning":
                    lblMessage.CssClass = "bg-yellow-50 text-yellow-700 p-4 rounded-xl mb-6 block text-center border border-yellow-200";
                    break;
                default:
                    lblMessage.CssClass = "bg-blue-50 text-blue-700 p-4 rounded-xl mb-6 block text-center border border-blue-200";
                    break;
            }
        }
    }
}