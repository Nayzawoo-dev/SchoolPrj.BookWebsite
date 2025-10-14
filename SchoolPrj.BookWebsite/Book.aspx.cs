using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using static SchoolPrj.BookWebsite._Book;

namespace SchoolPrj.BookWebsite
{
    public partial class _Book : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public List<Book> Books { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserId"] != null)
            {
                using (var connection = new SqlConnection(connStr))
                {
                    var res = connection.QueryFirstOrDefault<int>(
                        "SELECT COUNT(1) FROM Tb_BanList WHERE UserId = @UserId",
                        new { UserId = HttpContext.Current.Session["UserId"] });

                    if (res > 0)
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
                }
                LoadBooks();
            }
        }


        protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
                var btnEditBook = (HyperLink)e.Item.FindControl("btnEditBook");
                if (btnDelete != null || btnEditBook != null) // Null ဖြစ်မဖြစ် စစ်ပေး
                {
                    if (Session["Role"] != null && Session["Role"].ToString() == "Admin")
                    {
                        btnDelete.Visible = true;
                        btnEditBook.Visible = true;
                    }

                }
            }
        }


        private void LoadBooks()
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql = "select * from Tb_Book ORDER By BookId DESC";
                Books = connection.Query<Book>(sql).AsList();
            }

            rptBooks.DataSource = Books;
            rptBooks.DataBind();
        }

        private void LoadBooks(string keyword = null)
        {
            keyword = keyword.Replace(" ", "").Replace("\u200B", "");
            using (var connection = new SqlConnection(connStr))
            {
                string sql = @"
    select * 
    from Tb_Book 
    where REPLACE(REPLACE(BookTitle, ' ', ''), NCHAR(8203), '') COLLATE SQL_Latin1_General_CP1_CI_AI 
              like '%' + REPLACE(REPLACE(@keyword, ' ', ''), NCHAR(8203), '') + '%'
       or REPLACE(REPLACE(Author, ' ', ''), NCHAR(8203), '') COLLATE SQL_Latin1_General_CP1_CI_AI 
              like '%' + REPLACE(REPLACE(@keyword, ' ', ''), NCHAR(8203), '') + '%'
       or REPLACE(REPLACE(Category, ' ', ''), NCHAR(8203), '') COLLATE SQL_Latin1_General_CP1_CI_AI 
              like '%' + REPLACE(REPLACE(@keyword, ' ', ''), NCHAR(8203), '') + '%'
";


                Books = connection.Query<Book>(sql, new Book { keyword = keyword }).AsList();
            }
            rptBooks.DataSource = Books;
            rptBooks.DataBind();


            pnlNoBooks.Visible = (Books.Count == 0);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            pnlNoBooks.Visible = false;
            string keyword = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                LoadBooks(keyword);
            }
            else { LoadBooks(); }
        }

        protected void btnUnderstand_Click(object sender, EventArgs e)
        {
            LoadBooks();
            pnlNoBooks.Visible = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int bookId = Convert.ToInt32(btn.CommandArgument);

            // Delete logic here  
            DeleteBook(bookId);

            // Rebind grid or update UI  
            LoadBooks();
        }

        protected void DeleteBook(int bookId)
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql = @"DELETE FROM [dbo].[Tb_Book]  
         WHERE BookId = @BookId";
                var res = connection.Execute(sql, new Book
                {
                    BookId = bookId
                });
            }
        }
    }
}