using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Linq;
using System.Web.UI.WebControls;
using Dapper;
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
                    "SELECT * FROM Tb_React WHERE UserId = @UserId",
                    new { UserId = Session["UserId"] }
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
    }

}


