using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Web.UI;

namespace SchoolPrj.BookWebsite
{
    public partial class CustomerSupport : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Auto-fill user information if logged in
                if (Session["UserId"] != null)
                {
                    AutoFillUserInfo();
                }
                if (Session["UserId"] != null && Session["Role"].ToString() != "User")
                {
                    // Session clear
                    Session.Clear();
                    Session.Abandon();

                    // Authentication cookie (ASP.NET Forms Auth သုံးထားရင်)
                    if (Request.Cookies["UserAuth"] != null)
                    {
                        Response.Cookies["UserAuth"].Expires = DateTime.Now.AddDays(-1);
                    }

                    // Website အစ Login Page သို့ ပြန်ပို့
                    Response.Redirect("~/Login.aspx");
                }

            }
        }

        private void AutoFillUserInfo()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Username, Email FROM Users WHERE UserId = @UserId";
                string adminSql = "SELECT Username, Email FROM Admins WHERE UserId = @UserId";

                var user = connection.QueryFirstOrDefault<dynamic>(sql, new { UserId = Session["UserId"] });
                var admin = connection.QueryFirstOrDefault<dynamic>(adminSql, new { UserId = Session["UserId"] });

                if (admin != null)
                {
                    txtFullName.Text = admin.Username;
                    txtEmail.Text = admin.Email;
                }
                else if (user != null)
                {
                    txtFullName.Text = user.Username;
                    txtEmail.Text = user.Email;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Validate form
            if (!Page.IsValid)
            {
                ShowMessage("⚠ Please fix the validation errors before submitting.", "error");
                return;
            }

            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string issueType = ddlIssueType.SelectedValue;
            string subject = txtSubject.Text.Trim();
            string message = txtMessage.Text.Trim();
            string priority = GetSelectedPriority();

            // Validate message length
            if (message.Length > 2000)
            {
                ShowMessage("⚠ Message is too long. Please keep it under 2000 characters.", "error");
                return;
            }

            try
            {
                // Save to database (optional)
                SaveSupportTicket(fullName, email, issueType, subject, message, priority);

                // Send email to support
                bool emailSent = SendSupportEmail(fullName, email, issueType, subject, message, priority);

                if (emailSent)
                {
                    ShowMessage("✅ Thank you for contacting us! We've received your message and will respond within 24-48 hours.", "success");
                    ClearForm();
                }
                else
                {
                    ShowMessage("⚠ Your message was saved but we couldn't send the notification email. We'll still get back to you soon.", "warning");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"❌ Sorry, there was an error sending your message. Please try again later. Error: {ex.Message}", "error");
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Support form error: {ex.Message}");
            }
        }

        private void SaveSupportTicket(string fullName, string email, string issueType, string subject, string message, string priority)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = @"
                        INSERT INTO SupportTickets 
                        (FullName, Email, IssueType, Subject, Message, Priority, UserId, Status, CreatedDate) 
                        VALUES 
                        (@FullName, @Email, @IssueType, @Subject, @Message, @Priority, @UserId, 'Open', GETDATE())";

                    int? userId = Session["UserId"] as int?;

                    connection.Execute(sql, new
                    {
                        FullName = fullName,
                        Email = email,
                        IssueType = issueType,
                        Subject = subject,
                        Message = message,
                        Priority = priority,
                        UserId = userId
                    });
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't show to user (email sending is more important)
                System.Diagnostics.Debug.WriteLine($"Error saving support ticket: {ex.Message}");
            }
        }

        private bool SendSupportEmail(string fullName, string email, string issueType, string subject, string message, string priority)
        {
            try
            {
                string supportEmail = "nayzawoo.ace@gmail.com";
                string emailSubject = $"[Support Ticket] {issueType}: {subject}";

                string body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; color: white; }}
        .content {{ padding: 30px; background: #f9f9f9; }}
        .info-box {{ background: white; padding: 15px; border-left: 4px solid #667eea; margin: 10px 0; }}
        .priority-high {{ border-left-color: #e53e3e; }}
        .priority-medium {{ border-left-color: #dd6b20; }}
        .priority-low {{ border-left-color: #38a169; }}
        .footer {{ padding: 20px; text-align: center; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>MyBookShelf Support Ticket</h1>
            <p>New customer support request received</p>
        </div>
        <div class='content'>
            <div class='info-box priority-{priority.ToLower()}'>
                <h3>Ticket Information</h3>
                <p><strong>Priority:</strong> {priority}</p>
                <p><strong>Issue Type:</strong> {issueType}</p>
                <p><strong>Subject:</strong> {subject}</p>
            </div>
            
            <div class='info-box'>
                <h3>Customer Information</h3>
                <p><strong>Name:</strong> {fullName}</p>
                <p><strong>Email:</strong> {email}</p>
                <p><strong>User ID:</strong> {(Session["UserId"] != null ? Session["UserId"].ToString() : "Not logged in")}</p>
            </div>
            
            <div class='info-box'>
                <h3>Message Details</h3>
                <p>{message.Replace("\n", "<br>")}</p>
            </div>
            
            <div class='info-box'>
                <h3>Response Instructions</h3>
                <p>Please respond to this email within 24-48 hours. You can reply directly to {email}.</p>
            </div>
        </div>
        <div class='footer'>
            <p>&copy; {DateTime.Now.Year} MyBookShelf. This is an automated support ticket notification.</p>
        </div>
    </div>
</body>
</html>";

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("nayzawoo.ace@gmail.com", "MyBookShelf Support");
                    mail.To.Add(supportEmail);
                    mail.Subject = emailSubject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.Normal;

                    // Add headers
                    mail.Headers.Add("X-Support-Ticket", "true");
                    mail.Headers.Add("X-Priority", priority);

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
                System.Diagnostics.Debug.WriteLine($"Support email sending failed: {ex.Message}");
                return false;
            }
        }

        private string GetSelectedPriority()
        {
            if (rbUrgent.Checked) return "Urgent";
            if (rbHigh.Checked) return "High";
            if (rbMedium.Checked) return "Medium";
            return "Low";
        }

        private void ClearForm()
        {
            if (Session["UserId"] == null)
            {
                txtFullName.Text = "";
                txtEmail.Text = "";
            }
            ddlIssueType.SelectedIndex = 0;
            txtSubject.Text = "";
            txtMessage.Text = "";
            rbLow.Checked = true;
        }

        private void ShowMessage(string msg, string type = "info")
        {
            lblMessage.Text = msg;
            lblMessage.Visible = true;

            switch (type.ToLower())
            {
                case "success":
                    lblMessage.CssClass = "text-green-700 mb-6 block text-sm bg-green-50 p-4 rounded-lg text-center border border-green-200";
                    break;
                case "error":
                    lblMessage.CssClass = "text-red-700 mb-6 block text-sm bg-red-50 p-4 rounded-lg text-center border border-red-200";
                    break;
                case "warning":
                    lblMessage.CssClass = "text-yellow-700 mb-6 block text-sm bg-yellow-50 p-4 rounded-lg text-center border border-yellow-200";
                    break;
                default:
                    lblMessage.CssClass = "text-blue-700 mb-6 block text-sm bg-blue-50 p-4 rounded-lg text-center border border-blue-200";
                    break;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
        }
    }
}