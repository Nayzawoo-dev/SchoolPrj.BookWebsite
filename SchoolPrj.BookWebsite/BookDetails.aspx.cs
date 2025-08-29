using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.UI.WebControls;
using static SchoolPrj.BookWebsite._BookDetails;

namespace SchoolPrj.BookWebsite
{
    public partial class _BookDetails : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected int bookId => int.Parse(Request.QueryString["BookId"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["BookId"] != null)
            {
                LoadBookDetails(bookId);
                LoadHeartCount();
            }
            if (!IsPostBack)
            {
                BindComments();
            }

        }
        protected bool IsHearted
        {
            get => ViewState["IsHearted"] != null && (bool)ViewState["IsHearted"];
            set => ViewState["IsHearted"] = value;
        }


        private void LoadHeartCount()
        {
            using (var conn = new SqlConnection(connStr))
            {
                var count = conn.Query<React>(
                    "SELECT HeartCount FROM Tb_React WHERE BookId = @BookId",
                    new React { BookId = bookId }
                ).ToList();
                if (count.Count == 0)
                {
                    lblHeartCount.Text = "0";
                }
                else
                {
                    lblHeartCount.Text = count.Count.ToString();
                }

                var react = conn.QueryFirstOrDefault<React>(
                    "SELECT * FROM Tb_React WHERE UserId = @UserId and BookId = @BookId",
                    new { UserId = Session["UserId"], BookId = bookId }
                );
                if (react != null)
                {
                    IsHearted = true;
                }
                else
                {
                    IsHearted = false;
                }
                UpdateHeartButton();
            }
        }

        protected void btnHeart_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connStr))
            {
                if (Session["UserId"] == null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('❌ Please login to react this book!');", true);
                    return;
                }
                if (!IsHearted)
                {
                    // Increase Heart Count
                    var query = @"INSERT INTO Tb_React(UserId,BookId, HeartCount) VALUES(@UserId,@BookId,1)";
                    conn.Execute(query, new { UserId = Session["UserId"], BookId = bookId });
                    IsHearted = true;
                }
                else
                {
                    // Decrease Heart Count
                    var query = @"
                        DELETE FROM Tb_React
                        WHERE UserId = @UserId";
                    conn.Execute(query, new { UserId = Session["UserId"] });

                    IsHearted = false;
                }
            }

            LoadHeartCount();
            UpdateHeartButton();
        }

        private void UpdateHeartButton()
        {
            btnHeart.Text = IsHearted ? "❤️" : "♡";
            btnHeart.CssClass = IsHearted ? "heart-btn text-3sm heart-active" : "heart-btn text-3xl";
        }




        private void LoadBookDetails(int bookId)
        {

            using (var conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Tb_Book WHERE BookId = @BookId";
                var book = conn.QueryFirstOrDefault<Book>(sql, new Book { BookId = bookId });

                if (book != null)
                {
                    lblTitle.Text = book.BookTitle;
                    lblCategory.Text = book.Category;
                    lblAuthor.Text = book.Author;
                    lblDescription.Text = book.Description;
                    lnkPdf.HRef = book.PdfPath;
                    imgBook.Src = book.ImagePath;
                    imgBook.Alt = book.BookTitle;
                    if (Session["UserID"] == null)
                    {
                        // Logged in user – enable download
                        lnkDownload.HRef = "#";
                        lnkDownload.Attributes["onclick"] = "alert('Please log in to download the book.'); return false;";
                        return;
                    }

                    // Not logged in – keep link visible but show alert
                    lnkDownload.HRef = book.PdfPath; // prevent navigation
                    lnkDownload.Attributes.Remove("onclick"); // remove any previous onclick

                }
            }
        }
        protected void btnSaveBook_Click(object sender, EventArgs e)
        {
            // 1. User Login Check
            if (Session["UserId"] == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('❌ Please login to save this book!');", true);
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            int bookId = Convert.ToInt32(Request.QueryString["BookId"]);


            using (var connection = new SqlConnection(connStr))
            {
                // 2. Duplicate Check
                string checkQuery = @"SELECT COUNT(1) FROM Tb_SaveBook WHERE UserId = @UserId AND BookId = @BookId";
                int exists = connection.ExecuteScalar<int>(checkQuery, new Save { UserId = userId, BookId = bookId });

                if (exists > 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('⚠️ You already saved this book!');", true);
                    return;
                }

                // 3. Insert New Save
                string insertQuery = @"INSERT INTO Tb_SaveBook (UserId, BookId, BookImagePath, BookTitle, Category, Author)
                                       VALUES (@UserId, @BookId, @BookImagePath, @BookTitle, @Category, @Author)";

                var parameters = new Save
                {
                    UserId = userId,
                    BookId = bookId,
                    BookImagePath = imgBook.Src,
                    BookTitle = lblTitle.Text,
                    Category = lblCategory.Text,
                    Author = lblAuthor.Text
                };

                connection.Execute(insertQuery, parameters);
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('✅ Book saved successfully!');", true);
        }

        protected void btnAddComment_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('❌ Please login ');", true);
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            string comment = txtComment.Text.Trim();
            string videoPath = null;

            // Upload Video if exists
            if (fuVideo.HasFile)
            {
                string folder = Server.MapPath("~/uploads/videos/");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string filename = Guid.NewGuid() + Path.GetExtension(fuVideo.FileName);
                fuVideo.SaveAs(Path.Combine(folder, filename));

                videoPath = "~/uploads/videos/" + filename;
            }

            using (var conn = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO Tb_Comment (BookId, UserId, Comment, VideoPath, Date,UserProfile,UserName,ImagePath,Role)
                                 VALUES (@BookId, @UserId, @Comment, @VideoPath, @Date,@UserProfile,@UserName,@ImagePath,@Role)";

                string ProfileImagePath = Session["ProfileImage"]?.ToString();
                string Username = Session["Username"]?.ToString();
                string Role = Session["Role"]?.ToString();
                if (videoPath == null)
                {
                    conn.Execute(query, new _Comment { BookId = bookId, UserId = userId, Comment = comment, VideoPath = null, ImagePath = null, Date = DateTime.Now, UserProfile = ProfileImagePath, UserName = Username, Role = Role });
                }
                else

                if (videoPath.ToLower().Contains(".mp4") || videoPath.ToLower().Contains(".mov") || videoPath.ToLower().Contains(".avi") || videoPath.ToLower().Contains(".wmv") || videoPath.ToLower().Contains(".flv") || videoPath.ToLower().Contains(".mkv"))
                {
                    conn.Execute(query, new _Comment { BookId = bookId, UserId = userId, Comment = comment, VideoPath = videoPath, ImagePath = null, Date = DateTime.Now, UserProfile = ProfileImagePath, UserName = Username, Role = Role });
                }
                else
                {
                    conn.Execute(query, new _Comment { BookId = bookId, UserId = userId, Comment = comment, VideoPath = null, ImagePath = videoPath, Date = DateTime.Now, UserProfile = ProfileImagePath, UserName = Username, Role = Role });
                }

            }

            txtComment.Text = "";
            BindComments();
        }

        protected void rptComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "AddReply")
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                string ProfileImagePath = Session["ProfileImage"]?.ToString();
                string Username = Session["Username"]?.ToString();
                string Role = Session["Role"]?.ToString();
                int commentId = Convert.ToInt32(e.CommandArgument);
                TextBox txtReply = (TextBox)e.Item.FindControl("txtReply");
                if (!string.IsNullOrEmpty(txtReply.Text))
                {
                    using (var con = new SqlConnection(connStr))
                    {
                        string query = @"
                    INSERT INTO Tb_Reply 
                    (CommentId, UserId, ReplyText, UserProfile, UserName, Role,Date,ReplyUserName)
                    VALUES (@CommentId, @UserId, @ReplyText, @UserProfile, @UserName, @Role,@Date,@ReplyUserName)";
                        var res = con.QueryFirstOrDefault<_Comment>("select UserName from Tb_Comment where CommentId=@CommentId", new _Comment { CommentId = commentId });
                        string replyusername = res.UserName;
                        con.Execute(query, new Reply
                        {
                            CommentId = commentId,
                            UserId = userId, // Current user ID
                            ReplyText = txtReply.Text,
                            UserProfile = ProfileImagePath,
                            UserName = Username,
                            ReplyUserName = replyusername,
                            Date = DateTime.Now,
                            Role = Role
                        });
                    }

                    BindComments();
                }
            }

            if (e.CommandName == "Delete")
            {
                int commentId = Convert.ToInt32(e.CommandArgument);
                using (var con = new SqlConnection(connStr))
                {
                    string query = @"DELETE FROM [dbo].[Tb_Comment]
      WHERE CommentId = @CommentId";
                    con.Execute(query, new _Comment
                    {
                        CommentId = commentId,
                    });
                }
                BindComments();
            }
        }

        protected void rptReply_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddNestedReply")
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                string ProfileImagePath = Session["ProfileImage"]?.ToString();
                string Username = Session["Username"]?.ToString();
                string Role = Session["Role"]?.ToString();
                int replyId = Convert.ToInt32(e.CommandArgument);
                TextBox txtReply = (TextBox)e.Item.FindControl("txtNesReply");

                if (!string.IsNullOrEmpty(txtReply.Text))
                {
                    using (var con = new SqlConnection(connStr))
                    {
                        string query = @"
                    INSERT INTO Tb_Reply 
                    (CommentId, UserId, ReplyText, UserProfile, UserName, Role,Date,ReplyUserName)
                    VALUES (@CommentId, @UserId, @ReplyText, @UserProfile, @UserName, @Role,@Date,@ReplyUserName)";
                        var res = con.QueryFirstOrDefault<Reply>("select UserName from Tb_Reply where ReplyId=@ReplyId", new Reply { ReplyId = replyId });
                        string replyusername = res.UserName;
                        con.Execute(query, new Reply
                        {
                            CommentId = replyId,
                            UserId = userId, // Current user ID
                            ReplyText = txtReply.Text,
                            UserProfile = ProfileImagePath,
                            UserName = Username,
                            ReplyUserName = replyusername,
                            Date = DateTime.Now,
                            Role = Role
                        });
                    }

                    BindComments();
                }
            }

            if (e.CommandName == "delete")
            {
                int replyId = Convert.ToInt32(e.CommandArgument);
                using (var con = new SqlConnection(connStr))
                {
                    string query = @"DELETE FROM [dbo].[Tb_Reply]
      WHERE ReplyId = @ReplyId";
                    con.Execute(query, new Reply
                    {
                        ReplyId = replyId,
                    });
                }
                BindComments();
            }
        }


        private void BindComments()
        {
            using (var con = new SqlConnection(connStr))
            {
                // Load all comments
                var comments = con.Query<_Comment>(
                    "select * from Tb_Comment where BookId = @BookId order by CommentId desc", new _Comment
                    {
                        BookId = bookId
                    }).ToList();

                // Load replies for each comment
                foreach (var comment in comments)
                {
                    var replies = con.Query<Reply>(
                        "SELECT * FROM Tb_Reply WHERE CommentId = @CommentId",
                        new Reply { CommentId = comment.CommentId }).ToList();

                    comment.Replies = replies;
                }

                rptComments.DataSource = comments;
                rptComments.DataBind();
            }
        }

        public static string GetRelativeTime(DateTime dateTime)
        {
            var ts = DateTime.Now - dateTime;

            if (ts.TotalMinutes < 1)
                return "just now";
            if (ts.TotalMinutes < 60)
                return $"{Math.Floor(ts.TotalMinutes)} minutes ago";
            if (ts.TotalHours < 24)
                return $"{Math.Floor(ts.TotalHours)} hr ago";
            if (ts.TotalDays < 7)
                return $"{Math.Floor(ts.TotalDays)} day{(ts.TotalDays >= 2 ? "s" : "")} ago";
            return dateTime.ToString("MMM dd, yyyy");
        }

    }

    public class _Comment
    {
        public int CommentId { get; set; }

        public string UserProfile { get; set; }
        public string UserName { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public string VideoPath { get; set; } = null;
        public DateTime Date { get; set; }

        public string ImagePath { get; set; } = null;

        public List<Reply> Replies { get; set; } = new List<Reply>();

        public string Role { get; set; }
    }

    public class Reply
    {
        public int ReplyId { get; set; }
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string ReplyText { get; set; }
        public string UserProfile { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        public string ReplyUserName { get; set; }
        public DateTime Date { get; set; }
    }

}