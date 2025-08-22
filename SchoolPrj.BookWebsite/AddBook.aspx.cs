using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;

namespace SchoolPrj.BookWebsite
{
    public partial class AddBook : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            string title = txtTitle.Text.Trim();
            string category = txtCategory.Text.Trim();
            string author = txtAuthor.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                lblMessage.Text = "⚠ Please enter a book title.";
                lblMessage.Visible=true;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                return;
            }

            using (var conn = new SqlConnection(connStr))
            {
                string sql = @"select * from Tb_Book where BookTitle = @BookTitle";
               var res = conn.QueryFirstOrDefault(sql, new
                {
                    BookTitle = title
                });

                if(res != null)
                {
                    lblMessage.Text = "⚠ This Book Is Already Exits.";
                    lblMessage.Visible = true;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                    return;
                }
            }


            if (string.IsNullOrEmpty(category))
            {
                lblMessage.Text = "⚠ Please enter a book category.";
                lblMessage.Visible = true;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                return;
            }

            if (string.IsNullOrEmpty(author))
            {
                lblMessage.Text = "⚠ Please enter a book author.";
                lblMessage.Visible = true;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                return;
            }

            if (string.IsNullOrEmpty(description))
            {
                lblMessage.Text = "⚠ Please enter a book description.";
                lblMessage.Visible = true;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                return;
            }

            // 1️⃣ Upload Image
            string imagePath = null;
            if (fuImage.HasFile == false) 
            {
                lblMessage.Text = "⚠ Please enter a book image.";
                lblMessage.Visible = true;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                return;
            }
            if (fuImage.HasFile)
            {
                string ext = Path.GetExtension(fuImage.FileName).ToLower();
                if (ext != ".jpg" && ext != ".png" && ext != ".jpeg")
                {
                    lblMessage.Text = "⚠ Only JPG, JPEG, PNG allowed for images.";
                    lblMessage.Visible = true;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                    return;
                }

                string imageFolder = Server.MapPath("~/uploads/images/");
                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);

                string imageFileName = Guid.NewGuid() + ext;
                fuImage.SaveAs(Path.Combine(imageFolder, imageFileName));
                imagePath = "~/uploads/images/" + imageFileName;
            }

            // 2️⃣ Upload PDF
            string pdfPath = null;
            if (fuPdf.HasFile == false)
            {
                lblMessage.Text = "⚠ Please enter a book file.";
                lblMessage.Visible = true;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                return;
            }
            if (fuPdf.HasFile)
            {
                string ext = Path.GetExtension(fuPdf.FileName).ToLower();
                if (ext != ".pdf")
                {
                    lblMessage.Text = "⚠ Only PDF files are allowed.";
                    lblMessage.Visible = true;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
                    return;
                }

                string pdfFolder = Server.MapPath("~/uploads/pdfs/");
                if (!Directory.Exists(pdfFolder))
                    Directory.CreateDirectory(pdfFolder);

                string pdfFileName = Guid.NewGuid() + ext;
                fuPdf.SaveAs(Path.Combine(pdfFolder, pdfFileName));
                pdfPath = "~/uploads/pdfs/" + pdfFileName;
            }

            // 3️⃣ Save to Database
            using (var conn = new SqlConnection(connStr))
            {
                string sql = @"INSERT INTO Tb_Book (BookTitle, Category, Author, Description, ImagePath, PdfPath) 
                               VALUES (@BookTitle, @Category, @Author, @Description, @ImagePath, @PdfPath)";
                conn.Execute(sql, new Book
                {
                    BookTitle = title,
                    Category = category,
                    Author = author,
                    Description = description,
                    ImagePath = imagePath,
                    PdfPath = pdfPath
                });
            }

            // Success message
            lblMessage.CssClass = "bg-red-50 text-green-700 p-4 rounded-xl mb-6 block text-center";
            lblMessage.Text = "✅ Book added successfully!";
            lblMessage.Visible = true;
            txtTitle.Text = string.Empty;
            txtAuthor.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtCategory.Text = string.Empty;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMsg", "hideMessageAfterDelay();", true);
            // Redirect to Books Page
        }
    }
}
