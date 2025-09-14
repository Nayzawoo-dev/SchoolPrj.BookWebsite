using Dapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Configuration;

namespace SchoolPrj.BookWebsite
{
    public partial class GoogleCallback : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string code = Request.QueryString["code"];
                if (!string.IsNullOrEmpty(code))
                {
                    string clientId = WebConfigurationManager.AppSettings["GoogleClientId"];
                    string clientSecret = WebConfigurationManager.AppSettings["GoogleClientSecret"];
                    string redirectUri = "https://PhatSaYar123.bsite.net/GoogleCallback.aspx";

                    using (var client = new HttpClient())
                    {
                        // 🔹 Exchange code for token
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
                        JObject tokenObj = JObject.Parse(tokenContent);

                        string accessToken = (string)tokenObj["access_token"];
                        if (string.IsNullOrEmpty(accessToken))
                        {
                            throw new Exception("Google did not return an access token.");
                        }

                        // 🔹 Fetch user info
                        client.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                        var userInfoResponse = client.GetStringAsync("https://www.googleapis.com/oauth2/v2/userinfo").Result;
                        JObject userInfo = JObject.Parse(userInfoResponse);

                        string email = (string)userInfo["email"];
                        string name = (string)userInfo["name"];
                        string googleId = (string)userInfo["id"];

                        // 🔹 DB handling
                        using (var connection = new SqlConnection(connStr))
                        {
                            var user = connection.QuerySingleOrDefault<User>(
                                "SELECT * FROM Users WHERE Email=@Email", new { Email = email });

                            var ban = connection.QuerySingleOrDefault<Ban>(
                                "SELECT * FROM Tb_BanList WHERE UserEmail=@UserEmail", new { UserEmail = email });

                            if (ban != null)
                            {
                                Session.Clear();
                                Session.Abandon();
                                if (Request.Cookies["UserAuth"] != null)
                                {
                                    Response.Cookies["UserAuth"].Expires = DateTime.Now.AddDays(-1);
                                }
                                Response.Write("<script>alert('You are banned from this Email.'); window.location='Login.aspx';</script>");
                                return;
                            }

                            // Insert new user if not exists
                            if (user == null)
                            {
                                string sql = @"INSERT INTO Users (Username, FullName, Email, Role, ProfileImagePath)
                                               VALUES (@Username, @FullName, @Email, @Role, @ProfileImagePath)";
                                connection.Execute(sql, new User
                                {
                                    Username = name,
                                    FullName = "Default",
                                    Email = email,
                                    Role = "User",
                                    ProfileImagePath = "/uploads/images/default.png"
                                });
                            }

                            int userId = user?.UserId ?? connection.QuerySingle<int>(
                                "SELECT UserId FROM Users WHERE Email=@Email", new { Email = email });

                            if (userId > 0)
                            {
                                var res = connection.QuerySingle<User>("select * from Users where Email = @Email", new User
                                {
                                    Email = email,
                                });
                                if(res != null)
                                {
                                    Session["Username"] = res.Username;
                                    Session["ProfileImage"] = res.ProfileImagePath;
                                }
                                else
                                {
                                    Session["Username"] = name.Replace(" ", "");
                                    Session["ProfileImage"] = "/uploads/images/default.png";
                                }
                            }

                            // Sessions
                            Session["UserId"] = userId;
                            Session["Role"] = "User";
                            Session["google"] = "google";

                            // Cookie
                            HttpCookie authCookie = new HttpCookie("UserAuth");
                            authCookie.Values["UserId"] = userId.ToString();
                            authCookie.Expires = DateTime.Now.AddDays(5);
                            Response.Cookies.Add(authCookie);

                            Response.Redirect("Book.aspx");
                        }
                    }
                }
                else
                {
                    Response.Write("No authorization code returned from Google.");
                }
            }
            catch (Exception ex)
            {
                // Debug output (remove in production)
                Response.Clear();
                Response.Write("<h2>Google OAuth Callback Error</h2>");
                Response.Write("<strong>Message:</strong> " + ex.Message + "<br><br>");
                Response.Write("<strong>StackTrace:</strong><br>" + ex.StackTrace.Replace(Environment.NewLine, "<br>"));
            }
        }

    }
}
