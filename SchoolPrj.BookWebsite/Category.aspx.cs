using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolPrj.BookWebsite
{
    public partial class _Category : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public List<Book> Books { get; set; }

        private int PageSize = 8;
        private int CurrentPage
        {
            get { return (int)(ViewState["CurrentPage"] ?? 1); }
            set { ViewState["CurrentPage"] = value; }
        }

        private int TotalPages
        {
            get { return (int)(ViewState["TotalPages"] ?? 0); }
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
                LoadBooks();
                LoadAuthor();
                LoadCategory();
               
            }
        }

        protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var lblLikeCount = (Label)e.Item.FindControl("lblLikeCount");
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

        private void LoadCategory()
        {
            using (var con = new SqlConnection(connStr))
            {
                var categories = con.Query<Category>("SELECT DISTINCT Category From Tb_Category").ToList();

                ddlCategory.Items.Clear();
                ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Category --", ""));

                foreach (var cat in categories)
                {
                    ddlCategory.Items.Add(new ListItem(cat.category));
                }
            }
        }

        protected void LoadAuthor()
        {
            using (var con = new SqlConnection(connStr))
            {
                var author = con.Query<Book>("SELECT DISTINCT Author From Tb_Book").ToList();

                ddlAuthor.Items.Clear();
                ddlAuthor.Items.Add(new ListItem("-- Select Author --", ""));

                foreach (var item in author)
                {
                    ddlAuthor.Items.Add(new ListItem(item.Author));
                }
            }
        }


        private void LoadBooks(string author = "", string category = "")
        {
            int startRow = ((CurrentPage - 1) * PageSize) + 1;
            int endRow = CurrentPage * PageSize;

            using (var connection = new SqlConnection(connStr))
            {
                // COUNT total records for pagination
                string countSql = "SELECT COUNT(*) FROM Tb_Book WHERE 1=1";
                if (!string.IsNullOrEmpty(author) && string.IsNullOrEmpty(category))
                    countSql += " AND Author = @Author";
                if (!string.IsNullOrEmpty(category) && string.IsNullOrEmpty(author))
                    countSql += " AND Category LIKE '%' + @Category + '%'";
                if (!string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(category))
                    countSql += " AND Author = @Author AND Category LIKE '%' + @Category + '%'";

                int totalRecords = connection.ExecuteScalar<int>(countSql, new { Author = author, Category = category });
                TotalPages = (int)Math.Ceiling((double)totalRecords / PageSize);

                // Actual query with pagination
                string sql = @"
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
                WHERE 1=1";

                if (!string.IsNullOrEmpty(author))
                    sql += " AND Author = @Author";
                if (!string.IsNullOrEmpty(category))
                    sql += " AND Category LIKE '%' + @Category + '%'";

                sql += @"
                    ) AS Result
                    WHERE RowNum BETWEEN @StartRow AND @EndRow
                    ORDER BY RowNum";

                Books = connection.Query<Book>(sql, new
                {
                    Author = author,
                    Category = category,
                    StartRow = startRow,
                    EndRow = endRow
                }).AsList();
            }

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
            repBooks.DataSource = Books;
            repBooks.DataBind();

            lblPageInfo.Text = $"Page {CurrentPage} of {TotalPages}";

            btnPrev.Enabled = (CurrentPage > 1);
            btnNext.Enabled = (CurrentPage < TotalPages);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            string selectedAuthor = ddlAuthor.SelectedValue;
            string selectedCategory = ddlCategory.SelectedValue;
            LoadBooks(selectedAuthor, selectedCategory);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadBooks(ddlAuthor.SelectedValue, ddlCategory.SelectedValue);
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadBooks(ddlAuthor.SelectedValue, ddlCategory.SelectedValue);
            }
        }

        protected void btnUnderstand_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            ddlAuthor.SelectedIndex = 0;
            ddlCategory.SelectedIndex = 0;
            LoadBooks();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int bookId = Convert.ToInt32(btn.CommandArgument);

            DeleteBook(bookId);
            LoadBooks(ddlAuthor.SelectedValue, ddlCategory.SelectedValue);
        }

        protected void DeleteBook(int bookId)
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql = "DELETE FROM Tb_Book WHERE BookId = @BookId";
                connection.Execute(sql, new { BookId = bookId });
            }
        }
    }
}
