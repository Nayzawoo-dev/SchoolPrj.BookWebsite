using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SchoolPrj.BookWebsite
{
    public partial class GoogleCallback : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            string code = Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code))
            {
                string clientId = WebConfigurationManager.AppSettings["GoogleClientId"];
                string clientSecret = WebConfigurationManager.AppSettings["GoogleClientSecret"];
                string redirectUri = "https://localhost:44339/GoogleCallback.aspx";

                using (var client = new HttpClient())
                {
                    var tokenResponse = client.PostAsync("https://oauth2.googleapis.com/token",
                        new FormUrlEncodedContent(new[]
                        {
                    new KeyValuePair<string,string>("code", code),
                    new KeyValuePair<string,string>("client_id", clientId),
                    new KeyValuePair<string,string>("client_secret", clientSecret),
                    new KeyValuePair<string,string>("redirect_uri", redirectUri),
                    new KeyValuePair<string,string>("grant_type","authorization_code")
                        })).Result;

                    var tokenContent = tokenResponse.Content.ReadAsStringAsync().Result;
                    dynamic tokenObj = JsonConvert.DeserializeObject(tokenContent);
                    string accessToken = tokenObj.access_token;

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                    var userInfoResponse = client.GetStringAsync("https://www.googleapis.com/oauth2/v2/userinfo").Result;
                    dynamic userInfo = JsonConvert.DeserializeObject(userInfoResponse);

                    string email = userInfo.email;
                    string name = userInfo.name;
                    string googleId = userInfo.id;

                    using (var connection = new SqlConnection(connStr))
                    {
                        var user = connection.QuerySingleOrDefault<User>("SELECT * FROM Users WHERE Email=@Email", new { Email = email });
                        var ban = connection.QuerySingleOrDefault<Ban>("SELECT * FROM Tb_BanList WHERE UserEmail=@UserEmail", new Ban{ UserEmail = email });
                        if (ban != null)
                        {
                            // Session clear
                            Session.Clear();
                            Session.Abandon();

                            // Authentication cookie (ASP.NET Forms Auth သုံးထားရင်)
                            if (Request.Cookies["UserAuth"] != null)
                            {
                                Response.Cookies["UserAuth"].Expires = DateTime.Now.AddDays(-1);
                            }
                            Response.Write("<script>alert('You are banned from this Email.'); window.location='Login.aspx';</script>");
                            return;
                        }
                        if (user == null)
                        {
                            string sql = @"INSERT INTO Users (Username, FullName, Email, Role, ProfileImagePath)
                               VALUES (@Username, @FullName, @Email, @Role, @ProfileImagePath)";
                            connection.Execute(sql, new User
                            {
                                Username = name.Replace(" ", ""),
                                FullName = "Default",
                                Email = email,
                                Role = "User",
                                ProfileImagePath = "/uploads/images/default.png"
                            });
                        }

                        Session["Username"] = name.Replace(" ", "");
                        Session["UserId"] = user?.UserId ?? connection.QuerySingle<int>("SELECT UserId FROM Users WHERE Email=@Email", new { Email = email });
                        Session["Role"] = "User";
                        Session["google"] = "google";
                        Session["ProfileImage"] = "/uploads/images/default.jpg"; // Default profile image
                        var id = connection.QuerySingle<int>("SELECT UserId FROM Users WHERE Email=@Email", new User { Email = email });
                        HttpCookie authCookie = new HttpCookie("UserAuth");
                        authCookie.Values["UserId"] = id.ToString();
                        authCookie.Expires = DateTime.Now.AddDays(5);
                        Response.Cookies.Add(authCookie);
                        Response.Redirect("Book.aspx");
                    }
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
