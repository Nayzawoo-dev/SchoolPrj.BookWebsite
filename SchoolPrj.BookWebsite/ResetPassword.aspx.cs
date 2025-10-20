using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;

namespace SchoolPrj.BookWebsite
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];

                if (string.IsNullOrEmpty(token))
                {
                    ShowMessage("*Invalid or missing reset token.", "error");
                    btnResetPassword.Enabled = false;
                    return;
                }

                // Validate token
                if (!IsValidResetToken(token))
                {
                    ShowMessage("*Invalid or expired reset token. Please request a new reset link.", "error");
                    btnResetPassword.Enabled = false;
                    return;
                }

                hdnResetToken.Value = token;
                lblMessage.Visible = false;
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string token = hdnResetToken.Value;

            if (string.IsNullOrEmpty(token))
            {
                ShowMessage("*Invalid reset token.", "error");
                return;
            }

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                ShowMessage("*Please fill in all password fields.", "error");
                return;
            }

            if (newPassword != confirmPassword)
            {
                ShowMessage("*Passwords do not match.", "error");
                return;
            }

            if (newPassword.Length < 6)
            {
                ShowMessage("*Password must be at least 6 characters long.", "error");
                return;
            }

            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    // Verify token is still valid
                    var tokenInfo = connection.QueryFirstOrDefault(
                        @"SELECT UserId, Email FROM Users 
                          WHERE PasswordResetToken = @Token 
                          AND PasswordResetExpiration > @Now 
                          AND IsEmailConfirmed = 1",
                        new { Token = token, Now = DateTime.Now });

                    if (tokenInfo == null)
                    {
                        ShowMessage("*Invalid or expired reset token. Please request a new reset link.", "error");
                        btnResetPassword.Enabled = false;
                        return;
                    }

                    // Generate new password hash and salt
                    string newSalt, newHash;
                    CreatePasswordHash(newPassword, out newHash, out newSalt);

                    // Update user password and clear reset token
                    int result = connection.Execute(
                        @"UPDATE Users 
                          SET PasswordHash = @PasswordHash, 
                              PasswordSalt = @PasswordSalt,
                              PasswordResetToken = NULL,
                              PasswordResetExpiration = NULL,
                              UpdatedDate = GETDATE()
                          WHERE PasswordResetToken = @Token",
                        new
                        {
                            PasswordHash = newHash,
                            PasswordSalt = newSalt,
                            Token = token
                        });

                    if (result > 0)
                    {
                        // Store success message in session for login page
                        Session["PasswordResetMessage"] = "✔ Password reset successfully! You can now login with your new password.";

                        // Redirect to login page
                        Response.Redirect("Login.aspx");
                    }
                    else
                    {
                        ShowMessage("*Failed to reset password. Please try again.", "error");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                ShowMessage("*Database error occurred. Please try again.", "error");
                System.Diagnostics.Debug.WriteLine($"ResetPassword SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                ShowMessage("*An error occurred. Please try again.", "error");
                System.Diagnostics.Debug.WriteLine($"ResetPassword Error: {ex.Message}");
            }
        }

        private bool IsValidResetToken(string token)
        {
            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    var tokenInfo = connection.QueryFirstOrDefault(
                        @"SELECT 1 FROM Users 
                          WHERE PasswordResetToken = @Token 
                          AND PasswordResetExpiration > @Now 
                          AND IsEmailConfirmed = 1",
                        new { Token = token, Now = DateTime.Now });

                    return tokenInfo != null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Token validation error: {ex.Message}");
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

            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
        }
    }
}