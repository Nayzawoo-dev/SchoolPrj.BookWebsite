using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using Dapper;
using static SchoolPrj.BookWebsite._Category;



namespace SchoolPrj.BookWebsite
{
    public partial class _Category : System.Web.UI.Page
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

            repBooks.DataSource = Books;
            repBooks.DataBind();
        }

        protected void btnUnderstand_Click(object sender, EventArgs e)
        {
            LoadBooks();
            pnlNoBook.Visible = false;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string selectedAuthor = ddlAuthor.SelectedValue;
            string selectedCategory = ddlCategory.SelectedValue;
            string query = string.Empty;
            // ဒီနေရာမှာတော့ Dapper, ADO.NET, EF စသဖြင့် DB query ရိုက်လို့ရပြီ
            if (string.IsNullOrEmpty(selectedCategory))
            {
                query = "select * from Tb_Book where Author = @Author";
            }

            if (string.IsNullOrEmpty(selectedAuthor))
            {
                query = "select * from Tb_Book where Category = @Category";
            }

            if (!string.IsNullOrEmpty(selectedCategory) && !string.IsNullOrEmpty(selectedAuthor))
            {
                query = "select * from Tb_Book where Author = @Author and Category = @Category";
            }

            using (var connection = new SqlConnection(connStr))
            {
                Books = connection.Query<Book>(query, new Book
                {
                    Author = selectedAuthor,
                    Category = selectedCategory,
                }).AsList();
                if (Books.Count <= 0)
                {
                    pnlNoBook.Visible = true;
                }
                repBooks.DataSource = Books;
                repBooks.DataBind();
            }
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
