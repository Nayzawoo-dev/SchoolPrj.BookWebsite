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
    }
}