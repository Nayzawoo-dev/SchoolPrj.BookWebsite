using System;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;

namespace SchoolPrj.BookWebsite
{
    public partial class VerifyEmail : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            string verificationCode = Request.QueryString["code"];

            if (string.IsNullOrEmpty(verificationCode))
            {
                lblMessage.Text = "Invalid verification link.";
                lblMessage.CssClass = "text-red-600";
                return;
            }

            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    // Find user with this verification code
                    var user = connection.QueryFirstOrDefault(
                        "SELECT UserId, Username, IsEmailConfirmed FROM Users WHERE EmailVerificationCode = @Code",
                        new { Code = verificationCode });

                    if (user == null)
                    {
                        lblMessage.Text = "Invalid or expired verification link.";
                        lblMessage.CssClass = "text-red-600";
                        return;
                    }

                    if (user.IsEmailConfirmed)
                    {
                        lblMessage.Text = $"Email already verified. You can login now.";
                        lblMessage.CssClass = "text-green-600";
                        return;
                    }

                    // Update user as verified
                    int rowsAffected = connection.Execute(
                        "UPDATE Users SET IsEmailConfirmed = 1, EmailVerificationCode = NULL WHERE EmailVerificationCode = @Code",
                        new { Code = verificationCode });

                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = $"Email verified successfully! Welcome {user.Username}, you can now login.";
                        lblMessage.CssClass = "text-green-600";
                    }
                    else
                    {
                        lblMessage.Text = "Verification failed. Please try again or contact support.";
                        lblMessage.CssClass = "text-red-600";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred during verification. Please try again.";
                lblMessage.CssClass = "text-red-600";
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Email verification error: {ex.Message}");
            }
        }
    }
}