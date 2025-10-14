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
    public partial class _PopularBook : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public List<Book> Books = new List<Book>();
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
                LoadBooks();
            }
        }

        private void LoadBooks()
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql = @"SELECT 
    BookId, 
    COUNT(UserId) AS Heart
FROM dbo.Tb_React
GROUP BY BookId
HAVING COUNT(UserId) >= 3
ORDER BY Heart DESC;";
                string sql2 = "select * from Tb_Book where BookId = @BookId";
                List<Book> res = connection.Query<Book>(sql).AsList();
                foreach (Book book in res)
                {
                    var books = connection.QueryFirstOrDefault<Book>(sql2, new Book
                    {
                        BookId = book.BookId,
                    });
                    Books.Add(books);
                }
                rptBook.DataSource = Books;
                rptBook.DataBind();
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
    }
}