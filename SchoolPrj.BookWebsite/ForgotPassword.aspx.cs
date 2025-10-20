using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;

namespace SchoolPrj.BookWebsite
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any existing messages
                lblMessage.Visible = false;
            }
        }

        protected void btnSendResetLink_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                ShowMessage("*Please enter your email address.", "error");
                return;
            }

            if (!IsValidEmail(email))
            {
                ShowMessage("*Please enter a valid email address.", "error");
                return;
            }

            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    // Check if email exists in database
                    var user = connection.QueryFirstOrDefault(
                        "SELECT UserId, Username, Email FROM Users WHERE Email = @Email",
                        new { Email = email });

                    if (user == null)
                    {
                        ShowMessage("*No account found with this email address.", "error");
                        return;
                    }

                    // Generate reset token and expiration
                    string resetToken = Guid.NewGuid().ToString();
                    DateTime expirationTime = DateTime.Now.AddHours(24); // Token valid for 24 hours

                    // Save reset token to database
                    int result = connection.Execute(
                        @"UPDATE Users 
                          SET PasswordResetToken = @ResetToken, 
                              PasswordResetExpiration = @Expiration 
                          WHERE Email = @Email",
                        new
                        {
                            ResetToken = resetToken,
                            Expiration = expirationTime,
                            Email = email
                        });

                    if (result > 0)
                    {
                        // Send reset email
                        bool emailSent = SendPasswordResetEmail(user.Email, user.Username, resetToken);

                        if (emailSent)
                        {
                            ShowMessage("✔ Password reset link has been sent to your email. Please check your inbox (including spam folder).", "success");
                            txtEmail.Text = ""; // Clear the email field
                        }
                        else
                        {
                            ShowMessage("✔ Account found but we couldn't send the reset email. Please try again later or contact support.", "warning");
                        }
                    }
                    else
                    {
                        ShowMessage("*Failed to generate reset token. Please try again.", "error");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                ShowMessage("*Database error occurred. Please try again.", "error");
                System.Diagnostics.Debug.WriteLine($"ForgotPassword SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                ShowMessage("*An error occurred. Please try again.", "error");
                System.Diagnostics.Debug.WriteLine($"ForgotPassword Error: {ex.Message}");
            }
        }

        private bool SendPasswordResetEmail(string toEmail, string username, string resetToken)
        {
            try
            {
                string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                string resetUrl = $"{baseUrl}/ResetPassword.aspx?token={HttpUtility.UrlEncode(resetToken)}";

                string subject = "Reset Your Password - MyBookShelf";
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
        .warning {{ background: #fff3cd; border: 1px solid #ffeaa7; padding: 10px; border-radius: 5px; margin: 15px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>MyBookShelf</h1>
            <p>Password Reset Request</p>
        </div>
        <div class='content'>
            <h2>Hello {username},</h2>
            <p>We received a request to reset your password for your MyBookShelf account.</p>
            <p style='text-align: center;'>
                <a href='{resetUrl}' class='button'>Reset Your Password</a>
            </p>
            <p>Or copy and paste this link in your browser:</p>
            <p style='word-break: break-all;'>{resetUrl}</p>
            
            <div class='warning'>
                <strong>Important:</strong>
                <ul>
                    <li>This link will expire in 24 hours</li>
                    <li>If you didn't request this reset, please ignore this email</li>
                    <li>Your password will not change until you create a new one</li>
                </ul>
            </div>
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
                    mail.Priority = MailPriority.High;

                    mail.Headers.Add("X-Mailer", "MyBookShelf");
                    mail.Headers.Add("X-Priority", "1");

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
                System.Diagnostics.Debug.WriteLine($"Password reset email sending failed: {ex.Message}");
                return false;
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