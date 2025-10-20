using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolPrj.BookWebsite
{
    public partial class _Book : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public List<Book> Books { get; set; }

        private int PageSize = 8; // how many books per page
        private int CurrentPage
        {
            get { return (int)(ViewState["CurrentPage"] ?? 1); }
            set { ViewState["CurrentPage"] = value; }
        }
        private int TotalPages
        {
            get { return (int)(ViewState["TotalPages"] ?? 1); }
            set { ViewState["TotalPages"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserId"] != null)
            {
                using (var connection = new SqlConnection(connStr))
                {
                    var res = connection.QueryFirstOrDefault<User>(
                        "select * from Users where UserId = @UserId",
                        new { UserId = HttpContext.Current.Session["UserId"] });

                    if (res != null && res.BanStatus > 0)
                    {
                        HttpContext.Current.Session.Clear();
                        HttpContext.Current.Session.Abandon();

                        if (HttpContext.Current.Request.Cookies["UserAuth"] != null)
                        {
                            HttpContext.Current.Response.Cookies["UserAuth"].Expires = DateTime.Now.AddDays(-1);
                        }

                        HttpContext.Current.Response.Redirect("~/Login.aspx?msg=banned", true);
                    }
                }
            }

            if (!IsPostBack)
            {
                if (Session["Role"] != null && Session["Role"].ToString() == "Admin")
                {
                    btnAddBook.Visible = true;
                    add_cat.Visible = true;
                }
                LoadBooks();
            }
        }

        protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var lblLikeCount = (Literal)e.Item.FindControl("lblLikeCount");
                int likeCount = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "LikeCount"));

                if (likeCount > 0)
                {
                    lblLikeCount.Text = $"People Like {likeCount}";
                    lblLikeCount.Visible = true;
                }
                else
                {
                    lblLikeCount.Visible = false; // 0 ဖြစ်ရင် မပြ
                }

                // Admin controls visibility logic
                var btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
                var btnEditBook = (HyperLink)e.Item.FindControl("btnEditBook");
                if (Session["Role"] != null && Session["Role"].ToString() == "Admin")
                {
                    btnDelete.Visible = true;
                    btnEditBook.Visible = true;
                    lblLikeCount.Visible = false;
                }
            }
        }


        private void LoadBooks(string keyword = null)
        {
            using (var connection = new SqlConnection(connStr))
            {
                string countSql = "SELECT COUNT(*) FROM Tb_Book";
                string mainSql = @"
    SELECT * FROM (
        SELECT 
            ROW_NUMBER() OVER(ORDER BY b.BookId DESC) AS RowNum, 
            b.*, 
            ISNULL(r.LikeCount, 0) AS LikeCount
        FROM Tb_Book b
        LEFT JOIN (
            SELECT BookId, COUNT(*) AS LikeCount
            FROM Tb_React
            GROUP BY BookId
        ) r ON b.BookId = r.BookId
    ) AS Books
    WHERE RowNum BETWEEN @StartRow AND @EndRow";

                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = keyword.Replace(" ", "").Replace("\u200B", "");
                    countSql += " WHERE REPLACE(REPLACE(BookTitle, ' ', ''), NCHAR(8203), '') COLLATE SQL_Latin1_General_CP1_CI_AI like '%' + REPLACE(REPLACE(@keyword, ' ', ''), NCHAR(8203), '') + '%' or REPLACE(REPLACE(Author, ' ', ''), NCHAR(8203), '') COLLATE SQL_Latin1_General_CP1_CI_AI like '%' + REPLACE(REPLACE(@keyword, ' ', ''), NCHAR(8203), '') + '%' or REPLACE(REPLACE(Category, ' ', ''), NCHAR(8203), '') COLLATE SQL_Latin1_General_CP1_CI_AI like '%' + REPLACE(REPLACE(@keyword, ' ', ''), NCHAR(8203), '') + '%'";
                    mainSql = @"SELECT * FROM (
                                    SELECT ROW_NUMBER() OVER(ORDER BY BookId DESC) AS RowNum, * 
                                    FROM Tb_Book 
                                    WHERE REPLACE(REPLACE(BookTitle, ' ', ''), NCHAR(8203), '') COLLATE SQL_Latin1_General_CP1_CI_AI like '%' + REPLACE(REPLACE(@keyword, ' ', ''), NCHAR(8203), '') + '%' or REPLACE(REPLACE(Author, ' ', ''), NCHAR(8203), '') COLLATE SQL_Latin1_General_CP1_CI_AI like '%' + REPLACE(REPLACE(@keyword, ' ', ''), NCHAR(8203), '') + '%' or REPLACE(REPLACE(Category, ' ', ''), NCHAR(8203), '') COLLATE SQL_Latin1_General_CP1_CI_AI like '%' + REPLACE(REPLACE(@keyword, ' ', ''), NCHAR(8203), '') + '%'
                                ) AS Books
                                WHERE RowNum BETWEEN @StartRow AND @EndRow";
                }

                int totalRecords = connection.ExecuteScalar<int>(countSql, new { keyword = "%" + keyword + "%" });
                TotalPages = (int)Math.Ceiling((double)totalRecords / PageSize);

                int startRow = ((CurrentPage - 1) * PageSize) + 1;
                int endRow = CurrentPage * PageSize;

                Books = connection.Query<Book>(mainSql, new { StartRow = startRow, EndRow = endRow, keyword = "%" + keyword + "%" }).AsList();
            }

            rptBooks.DataSource = Books;
            rptBooks.DataBind();

            if (Books.Count == 0)
            {
                pnlNoBooks.Visible = true;
                btnPrev.Visible = false;
                btnNext.Visible = false;
                lblPageInfo.Visible = false;
            }
            else
            {
                pnlNoBooks.Visible = false;
                btnPrev.Visible = true;
                btnNext.Visible = true;
                lblPageInfo.Visible = true;
            }
            lblPageInfo.Text = $"Page {CurrentPage} of {TotalPages}";

            btnPrev.Enabled = (CurrentPage > 1);
            btnNext.Enabled = (CurrentPage < TotalPages);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            string keyword = txtSearch.Text.Trim();
            LoadBooks(keyword);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadBooks(txtSearch.Text.Trim());
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadBooks(txtSearch.Text.Trim());
            }
        }

        protected void btnUnderstand_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            txtSearch.Text = "";
            LoadBooks();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int bookId = Convert.ToInt32(btn.CommandArgument);
            DeleteBook(bookId);
            LoadBooks(txtSearch.Text.Trim());
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var categoryName = txtCategory.Text.Trim();

            if (!string.IsNullOrEmpty(categoryName))
            {
                using (var con = new SqlConnection(connStr))
                {
                    string query = "INSERT INTO Tb_Category (Category) VALUES (@Category)";
                    string query2 = "SELECT * FROM Tb_Category WHERE Category = @Category";
                    var existingCategory = con.QueryFirstOrDefault<Category>(query2, new { category = categoryName });
                    if (existingCategory != null)
                    {
                        lblMessage.Text = "⚠️ Category already exists.";
                        lblMessage.Visible = true;
                        return;
                    }
                    con.Execute(query, new { category = categoryName });
                    lblMessage.Text = "✅ Category added successfully.";
                    lblMessage.Visible = true;
                }
                txtCategory.Text = "";
            }
        }

        protected void DeleteBook(int bookId)
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql = @"DELETE FROM [dbo].[Tb_Book] WHERE BookId = @BookId";
                connection.Execute(sql, new { BookId = bookId });
            }
        }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string category { get; set; }

    }

}
