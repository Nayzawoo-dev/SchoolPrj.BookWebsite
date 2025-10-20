using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace SchoolPrj.BookWebsite
{
    public partial class EditBook : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategory();
                string bookId = Request.QueryString["BookId"];
                if (!string.IsNullOrEmpty(bookId))
                {
                    LoadBook(bookId);
                }

            }
            if (Session["UserId"] == null || Session["Role"].ToString() != "Admin")
            {
                // Session clear
                Session.Clear();
                Session.Abandon();

                // Authentication cookie (ASP.NET Forms Auth သုံးထားရင်)
                if (Request.Cookies["UserAuth"] != null)
                {
                    Response.Cookies["UserAuth"].Expires = DateTime.Now.AddDays(-1);
                }

                // Website အစ Login Page သို့ ပြန်ပို့
                Response.Redirect("~/Login.aspx");
            }
        }

        private void LoadBook(string bookId)
        {
            using (var connection = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Tb_Book WHERE BookId=@BookId";
                var book = connection.QuerySingleOrDefault(sql, new { BookId = bookId });
                var category = connection.Query<Category>("SELECT CategoryId, Category FROM Tb_Category WHERE Category=@Category", new { category = book.Category }).ToList();

                if (book != null)
                {
                    hfBookId.Value = book.BookId.ToString();
                    txtTitle.Text = book.BookTitle;
                    txtAuthor.Text = book.Author;
                    imgPreview.ImageUrl = book.ImagePath;
                    lnkPdf.NavigateUrl = book.PdfPath;
                    txtDescription.Text = book.Description;

                    // Category ကို Select ပြုလုပ်ခြင်း
                    ddlCategory.ClearSelection();
                    var item = ddlCategory.Items.FindByText(book.Category);
                    if (item != null)
                    {
                        item.Selected = true;
                    }

                }
            }
        }

        private void LoadCategory()
        {
            using (var con = new SqlConnection(connStr))
            {
                var categories = con.Query<Category>("SELECT CategoryId, Category FROM Tb_Category ORDER BY Category").ToList();
                foreach (var cat in categories)
                {
                    ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem(cat.category, cat.CategoryId.ToString()));
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
                string savePath = "~/uploads/images/" + fileName;
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
                string savePath = "~/uploads/pdfs/" + fileName;
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
                    Category = ddlCategory.SelectedItem.Text,
                    ImagePath = imagePath,
                    PdfPath = pdfPath
                });
            }

            Response.Redirect("~/Book.aspx");
        }
    }
}
