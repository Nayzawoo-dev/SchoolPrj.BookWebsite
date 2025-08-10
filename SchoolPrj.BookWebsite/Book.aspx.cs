using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using static SchoolPrj.BookWebsite.Book;

namespace SchoolPrj.BookWebsite
{
    public partial class _Book : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public List<Book> Books { get; set; }
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
                string sql = "SELECT * FROM Tb_Book";
                Books = connection.Query<Book>(sql).AsList();
            }

            rptBooks.DataSource = Books;
            rptBooks.DataBind();
        }

        private void LoadBooks(string keyword = null)
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql = "select * from Tb_Book where BookTitle like @keyword or Author like @keyword";
                Books = connection.Query<Book>(sql, new Book { keyword = keyword }).AsList();
            }
            rptBooks.DataSource = Books;
            rptBooks.DataBind();

            pnlNoBooks.Visible = (Books.Count == 0);

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
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
    }
}