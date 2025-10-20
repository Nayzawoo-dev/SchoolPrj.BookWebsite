using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;

namespace SchoolPrj.BookWebsite
{
    public partial class _Login : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check for success messages from other pages
                if (Session["RegisterMessage"] != null)
                {
                    ShowMessage(Session["RegisterMessage"].ToString(), "success");
                    Session["RegisterMessage"] = null;
                }

                if (Session["VerifyMessage"] != null)
                {
                    ShowMessage(Session["VerifyMessage"].ToString(), "success");
                    Session["VerifyMessage"] = null;
                }

                if (Session["PasswordResetMessage"] != null)
                {
                    ShowMessage(Session["PasswordResetMessage"].ToString(), "success");
                    Session["PasswordResetMessage"] = null;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string usernameOrEmail = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Validation
            if (string.IsNullOrEmpty(usernameOrEmail) || string.IsNullOrEmpty(password))
            {
                ShowMessage("*Please enter both username/email and password.", "error");
                return;
            }

            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    // Check if user exists in Users table
                    var user = connection.QuerySingleOrDefault<User>(
                        @"SELECT UserId, Username, PasswordHash, PasswordSalt, Email, Role, 
                                  ProfileImagePath, IsEmailConfirmed, EmailVerificationCode, BanStatus
                           FROM Users 
                           WHERE Email = @Email OR Username = @Username",
                        new { Email = usernameOrEmail, Username = usernameOrEmail });

                    // Check if admin exists
                    var admin = connection.QueryFirstOrDefault<Admins>(
                        "SELECT * FROM Admins WHERE Email = @Email OR Username = @Username",
                        new { Email = usernameOrEmail , Username = usernameOrEmail });
                    // Check if user is banned
                    if (user != null && user.BanStatus > 0)
                    {
                        ShowMessage("*Your account has been banned. Please contact support for more information.", "error");
                        return;
                    }
                    // Handle Admin Login
                    if (admin != null)
                    {
                        if (CheckAdminPassword(password, admin.Password))
                        {
                            SetUserSession(admin.UserId, admin.Username, admin.Role, admin.ProfileImagePath);
                            CreateAuthCookie(admin.UserId.ToString(), 5); // 5 days for admin
                            Response.Redirect("Book.aspx");
                            return;
                        }
                        else
                        {
                            ShowMessage("*Invalid admin password!", "error");
                            return;
                        }
                    }

                    // Handle User Login
                    if (user != null)
                    {
                        // Check if email is verified
                        if (!user.IsEmailConfirmed)
                        {
                            ShowMessage("*Please verify your email before logging in. Check your email (including spam folder) for verification link.", "warning");

                            // Optionally resend verification email
                            ResendVerificationEmail(user.Email, user.Username);
                            return;
                        }

                        // Verify password
                        if (VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                        {
                            SetUserSession(user.UserId, user.Username, user.Role, user.ProfileImagePath);
                            CreateAuthCookie(user.UserId.ToString(), 3); // 3 days for user
                            Response.Redirect("Book.aspx");
                        }
                        else
                        {
                            ShowMessage("*Invalid password!", "error");
                        }
                    }
                    else
                    {
                        ShowMessage("*Invalid username or email!", "error");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                ShowMessage("*Database error occurred. Please try again.", "error");
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Login SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                ShowMessage("*An error occurred during login. Please try again.", "error");
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Login Error: {ex.Message}");
            }
        }

        private bool CheckAdminPassword(string inputPassword, string storedPassword)
        {
            // Simple comparison for admin (you might want to hash admin passwords too)
            return inputPassword == storedPassword;
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(storedSalt))
            {
                return false;
            }

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

        private void SetUserSession(int userId, string username, string role, string profileImage)
        {
            Session["UserId"] = userId;
            Session["Username"] = username;
            Session["Role"] = role;
            Session["ProfileImage"] = profileImage ?? "/uploads/images/default.png";
        }

        private void CreateAuthCookie(string userId, int expireDays)
        {
            HttpCookie authCookie = new HttpCookie("UserAuth");
            authCookie.Values["UserId"] = userId;
            authCookie.Expires = DateTime.Now.AddDays(expireDays);
            Response.Cookies.Add(authCookie);
        }

        private void ResendVerificationEmail(string email, string username)
        {
            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    // Get or create new verification code
                    string verificationCode = Guid.NewGuid().ToString();

                    var result = connection.Execute(
                        @"UPDATE Users 
                          SET EmailVerificationCode = @VerificationCode 
                          WHERE Email = @Email",
                        new { VerificationCode = verificationCode, Email = email });

                    if (result > 0)
                    {
                        SendVerificationEmail(email, username, verificationCode);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't show to user
                System.Diagnostics.Debug.WriteLine($"Resend verification email error: {ex.Message}");
            }
        }

        private bool SendVerificationEmail(string toEmail, string username, string verificationCode)
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
            <p>Email Verification Request</p>
        </div>
        <div class='content'>
            <h2>Hello {username},</h2>
            <p>We noticed you tried to login but your email hasn't been verified yet. Please verify your email address by clicking the button below:</p>
            <p style='text-align: center;'>
                <a href='{verifyUrl}' class='button'>Verify Email Address</a>
            </p>
            <p>Or copy and paste this link in your browser:</p>
            <p style='word-break: break-all;'>{verifyUrl}</p>
            <p>If you didn't request this email, please ignore it.</p>
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

                    mail.Headers.Add("X-Mailer", "MyBookShelf");
                    mail.Headers.Add("X-Priority", "3");

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("nayzawoo.ace@gmail.com", "ijkz bsvx vsrp aukg");
                        smtp.EnableSsl = true;
                        smtp.Timeout = 30000;

                        smtp.Send(mail);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Verification email sending failed: {ex.Message}");
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

            // Register script to hide message after delay
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
        }
    }
}