using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using Dapper;

namespace SchoolPrj.BookWebsite
{
    public partial class EditBook : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string bookId = Request.QueryString["BookId"];
                if (!string.IsNullOrEmpty(bookId))
                {
                    LoadBook(bookId);
                }
            }
        }

        private void LoadBook(string bookId)
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Tb_Book WHERE BookId=@BookId";
                var book = connection.QuerySingleOrDefault(sql, new { BookId = bookId });
                if (book != null)
                {
                    hfBookId.Value = book.BookId.ToString();
                    txtTitle.Text = book.BookTitle;
                    txtAuthor.Text = book.Author;
                    txtCategory.Text = book.Category;
                    imgPreview.ImageUrl = book.ImagePath;
                    lnkPdf.NavigateUrl = book.PdfPath;
                    txtDescription.Text = book.Description;
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string bookId = hfBookId.Value;

            // default to existing image & pdf
            string imagePath = imgPreview.ImageUrl;
            string pdfPath = lnkPdf.NavigateUrl;

            // if user uploads new image, replace it
            if (fuImage.HasFile)
            {
                string ext = Path.GetExtension(fuImage.FileName).ToLower();
                if (ext != ".jpg" && ext != ".png" && ext != ".jpeg")
                {
                    lblMessage.Text = "⚠ Only JPG, JPEG, PNG allowed for images.";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                    return;
                }

                string fileName = Guid.NewGuid() + System.IO.Path.GetExtension(fuImage.FileName);
                string savePath = "~/Uploads/Images/" + fileName;
                fuImage.SaveAs(Server.MapPath(savePath));
                imagePath = savePath;
            }

            // if user uploads new pdf, replace it
            if (fuPdf.HasFile)
            {
                string ext = Path.GetExtension(fuPdf.FileName).ToLower();
                if (ext != ".pdf")
                {
                    lblMessage.Text = "⚠ Only PDF files are allowed.";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                    return;
                }

                string fileName = Guid.NewGuid() + System.IO.Path.GetExtension(fuPdf.FileName);
                string savePath = "~/Uploads/Pdfs/" + fileName;
                fuPdf.SaveAs(Server.MapPath(savePath));
                pdfPath = savePath;
            }

            using (var connection = new SqlConnection(connStr))
            {
                string sql = @"UPDATE Tb_Book
                               SET BookTitle=@BookTitle, Author=@Author, Category=@Category, 
                                   ImagePath=@ImagePath, PdfPath=@PdfPath
                               WHERE BookId=@BookId";

                connection.Execute(sql, new
                {
                    BookId = bookId,
                    BookTitle = txtTitle.Text,
                    Author = txtAuthor.Text,
                    Category = txtCategory.Text,
                    ImagePath = imagePath,
                    PdfPath = pdfPath
                });
            }

            Response.Redirect("~/Book.aspx");
        }
    }
}
