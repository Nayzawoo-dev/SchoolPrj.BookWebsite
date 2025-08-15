using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using Dapper;
using static SchoolPrj.BookWebsite._BookDetails;

namespace SchoolPrj.BookWebsite
{
    public partial class _BookDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["BookId"] != null)
                {
                    int bookId = int.Parse(Request.QueryString["BookId"]);
                    LoadBookDetails(bookId);
                }
            }
        }

        private void LoadBookDetails(int bookId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Tb_Book WHERE BookId = @BookId";
                var book = conn.QueryFirstOrDefault<Book>(sql, new Book{ BookId = bookId });

                if (book != null)
                {
                    lblTitle.Text = book.BookTitle;
                    lblCategory.Text = book.Category;
                    lblAuthor.Text = book.Author;
                    lblDescription.Text = book.Description;
                    lnkPdf.HRef = book.PdfPath;
                    imgBook.Src = book.ImagePath;
                    imgBook.Alt = book.BookTitle;
                    lnkDownload.HRef = book.PdfPath;
                }
            }
        }
    }

}
