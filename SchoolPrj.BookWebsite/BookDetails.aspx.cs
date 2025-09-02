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
using System.Web.UI;
using System.Web.UI.WebControls;
using static SchoolPrj.BookWebsite._BookDetails;

namespace SchoolPrj.BookWebsite
{
    public partial class _BookDetails : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected int bookId => int.Parse(Request.QueryString["BookId"]);
        protected int userId => Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindComments();
                LoadBookDetails(bookId);
                LoadHeartCount();
            }

            if (Request.QueryString["BookId"] != null)
            {
                LoadBookDetails(bookId);
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('❌ Please login react this book');", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('❌ Please login Save Book');", true);
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('⚠️ You already saved this book!');", true);
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

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Book Save Successfully');", true);
        }



        protected void btnAddComment_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('❌ Please login');", true);
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

            if (!string.IsNullOrEmpty(comment) || videoPath != null)
            {
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
                pnlNoComments.Visible = false;
                BindComments();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('❌ Please Write Something');", true);
                return;
            }

            txtComment.Text = "";
            videoPath = null;
        }


        protected void rptComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddReply")
            {
                if (Session["UserId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('❌ Please login');", true);
                    return;
                }
                int userId = Convert.ToInt32(Session["UserId"]);
                string ProfileImagePath = Session["ProfileImage"]?.ToString();
                string Username = Session["Username"]?.ToString();
                string Role = Session["Role"]?.ToString();
                int commentId = Convert.ToInt32(e.CommandArgument);
                TextBox txtReply = (TextBox)e.Item.FindControl("txtReply");
                string videoPath = null;
                FileUpload fuReVideo = (FileUpload)e.Item.FindControl("fuReVideo");
                if (fuReVideo.HasFile)
                {
                    string folder = Server.MapPath("~/uploads/videos/");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    string filename = Guid.NewGuid() + Path.GetExtension(fuReVideo.FileName);
                    fuReVideo.SaveAs(Path.Combine(folder, filename));
                    videoPath = "~/uploads/videos/" + filename;
                }

                if (!string.IsNullOrEmpty(txtReply.Text) || videoPath != null)
                {
                    using (var con = new SqlConnection(connStr))
                    {
                        string query = @"
                    INSERT INTO Tb_Reply 
                    (CommentId, UserId, ReplyText, UserProfile, UserName, Role,Date,ReplyUserName,VideoPath,ImagePath)
                    VALUES (@CommentId, @UserId, @ReplyText, @UserProfile, @UserName, @Role,@Date,@ReplyUserName,@VideoPath,@ImagePath)";
                        var res = con.QueryFirstOrDefault<_Comment>("select UserName from Tb_Comment where CommentId=@CommentId", new _Comment { CommentId = commentId });
                        string replyusername = res.UserName;
                        if (videoPath == null)
                        {
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
                        else
                              if (videoPath.ToLower().Contains(".mp4") || videoPath.ToLower().Contains(".mov") || videoPath.ToLower().Contains(".avi") || videoPath.ToLower().Contains(".wmv") || videoPath.ToLower().Contains(".flv") || videoPath.ToLower().Contains(".mkv"))
                        {
                            con.Execute(query, new Reply
                            {
                                CommentId = commentId,
                                UserId = userId, // Current user ID
                                ReplyText = txtReply.Text,
                                UserProfile = ProfileImagePath,
                                UserName = Username,
                                VideoPath = videoPath,
                                ImagePath = null,
                                ReplyUserName = replyusername,
                                Date = DateTime.Now,
                                Role = Role
                            });
                        }
                        else
                        {
                            con.Execute(query, new Reply
                            {
                                CommentId = commentId,
                                UserId = userId, // Current user ID
                                ReplyText = txtReply.Text,
                                UserProfile = ProfileImagePath,
                                VideoPath = null,
                                ImagePath = videoPath,
                                UserName = Username,
                                ReplyUserName = replyusername,
                                Date = DateTime.Now,
                                Role = Role
                            });
                        }
                    }


                    BindComments();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('❌ Please Write Something');", true);
                    return;
                }
            }
            if (e.CommandName == "deleteComment")
            {
                int commentId = Convert.ToInt32(e.CommandArgument);
                using (var con = new SqlConnection(connStr))
                {
                    // Then delete the comment
                    string deleteCommentQuery = "DELETE FROM Tb_Comment WHERE CommentId = @CommentId";
                    string deleteRepliesQuery = "DELETE FROM Tb_Reply WHERE CommentId = @CommentId";
                    con.Execute(deleteRepliesQuery, new Reply { CommentId = commentId });
                    con.Execute(deleteCommentQuery, new _Comment { CommentId = commentId });
                    BindComments();
                }
            }
        }


        protected void rptComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Button btnUpload = (Button)e.Item.FindControl("btnReply");
            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            scriptManager.RegisterPostBackControl(btnUpload);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var dataItem = e.Item.DataItem;
                string commentUserId = DataBinder.Eval(dataItem, "UserId")?.ToString();
                string sessionUserId = Session["UserId"]?.ToString();

                Button btnDelete = (Button)e.Item.FindControl("btnCommentDelete");

                if (!string.IsNullOrEmpty(sessionUserId) && commentUserId == sessionUserId)
                {
                    btnDelete.Visible = true; // တူရင်ပြ
                }
                else
                {
                    btnDelete.Visible = false; // မတူရင်မပြ
                }
            }

        }

        protected void rptReply_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddNestedReply")
            {
                if (Session["UserId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('❌ Please login');", true);
                    return;
                }
                RepeaterItem replyItem = (RepeaterItem)e.Item;
                int userId = Convert.ToInt32(Session["UserId"]);
                string ProfileImagePath = Session["ProfileImage"]?.ToString();
                string Username = Session["Username"]?.ToString();
                string Role = Session["Role"]?.ToString();
                int replyId = Convert.ToInt32(e.CommandArgument);
                HiddenField hdnCommentId = (HiddenField)replyItem.FindControl("hdnCommentId");
                string commentId = hdnCommentId.Value;
                TextBox txtReply = (TextBox)e.Item.FindControl("txtNesReply");
                string videoPath = null;
                FileUpload fuNesVideo = (FileUpload)e.Item.FindControl("fuNesVideo");
                if (fuNesVideo.HasFile)
                {
                    string folder = Server.MapPath("~/uploads/videos/");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    string filename = Guid.NewGuid() + Path.GetExtension(fuNesVideo.FileName);
                    fuNesVideo.SaveAs(Path.Combine(folder, filename));
                    videoPath = "~/uploads/videos/" + filename;
                }
                if (!string.IsNullOrEmpty(txtReply.Text) || videoPath != null)
                {
                    using (var con = new SqlConnection(connStr))
                    {
                        string query = @"
                    INSERT INTO Tb_Reply 
                    (CommentId, UserId, ReplyText, UserProfile, UserName, Role,Date,ReplyUserName,VideoPath,ImagePath)
                    VALUES (@CommentId, @UserId, @ReplyText, @UserProfile, @UserName, @Role,@Date,@ReplyUserName,@VideoPath,@ImagePath)";
                        var res = con.QueryFirstOrDefault<Reply>("select UserName from Tb_Reply where ReplyId=@ReplyId", new Reply { ReplyId = replyId });
                        string replyusername = res.UserName;
                        if (videoPath == null)
                        {
                            con.Execute(query, new Reply
                            {
                                CommentId = Convert.ToInt32(commentId),
                                UserId = userId, // Current user ID
                                ReplyText = txtReply.Text,
                                UserProfile = ProfileImagePath,
                                UserName = Username,
                                ReplyUserName = replyusername,
                                Date = DateTime.Now,
                                Role = Role
                            });
                        }
                        else
                              if (videoPath.ToLower().Contains(".mp4") || videoPath.ToLower().Contains(".mov") || videoPath.ToLower().Contains(".avi") || videoPath.ToLower().Contains(".wmv") || videoPath.ToLower().Contains(".flv") || videoPath.ToLower().Contains(".mkv"))
                        {
                            con.Execute(query, new Reply
                            {
                                CommentId = Convert.ToInt32(commentId),
                                UserId = userId, // Current user ID
                                ReplyText = txtReply.Text,
                                UserProfile = ProfileImagePath,
                                UserName = Username,
                                VideoPath = videoPath,
                                ImagePath = null,
                                ReplyUserName = replyusername,
                                Date = DateTime.Now,
                                Role = Role
                            });
                        }
                        else
                        {
                            con.Execute(query, new Reply
                            {
                                CommentId = Convert.ToInt32(commentId),
                                UserId = userId, // Current user ID
                                ReplyText = txtReply.Text,
                                UserProfile = ProfileImagePath,
                                VideoPath = null,
                                ImagePath = videoPath,
                                UserName = Username,
                                ReplyUserName = replyusername,
                                Date = DateTime.Now,
                                Role = Role
                            });
                        }

                        BindComments();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('❌ Please Write Something');", true);
                    return;
                }
            }
            if (e.CommandName == "deleteReply")
            {
                int replyId = Convert.ToInt32(e.CommandArgument);
                using (var con = new SqlConnection(connStr))
                {
                    // Then delete the comment
                    string deleteCommentQuery = "DELETE FROM Tb_Reply WHERE ReplyId = @ReplyId";
                    con.Execute(deleteCommentQuery, new Reply { ReplyId = replyId });
                    BindComments();
                }
            }

        }

        protected void rptReply_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button btnUpload = (Button)e.Item.FindControl("btnNesReply");
                ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
                scriptManager.RegisterPostBackControl(btnUpload);
                var dataItem = e.Item.DataItem;
                string replyUserId = DataBinder.Eval(dataItem, "UserId")?.ToString();
                string sessionUserId = Session["UserId"]?.ToString();
                Button btnDelete = (Button)e.Item.FindControl("btnReplyDelete");
                if (!string.IsNullOrEmpty(sessionUserId) && replyUserId == sessionUserId)
                {
                    btnDelete.Visible = true; // တူရင်ပြ
                }
                else
                {
                    btnDelete.Visible = false; // မတူရင်မပြ
                }
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
                if (comments.Count == 0)
                {
                    pnlNoComments.Visible = true;
                }
                else
                {
                    pnlNoComments.Visible = false;
                }
                foreach (var comment in comments)
                {
                    // Load replies for each comment
                    var replies = con.Query<Reply>(
                        "select * from Tb_Reply where CommentId = @CommentId order by ReplyId asc", new Reply
                        {
                            CommentId = comment.CommentId
                        }).ToList();
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

    public class React
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public long HeartCount { get; set; } = 0;
    }

    public class Save
    {
        public int SaveId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string BookImagePath { get; set; }
        public string BookTitle { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
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

        public string VideoPath { get; set; }

        public string ImagePath { get; set; }
        public string ReplyUserName { get; set; }
        public DateTime Date { get; set; }
    }

}