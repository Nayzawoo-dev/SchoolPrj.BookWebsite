using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;

namespace SchoolPrj.BookWebsite
{
    public partial class ReactList : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private int pageSize = 10;
        private int currentPage = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is admin
                if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
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

                LoadFilterData();
                LoadStats();
                LoadReactions();
            }
        }

        private void LoadFilterData()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                // Load users for filter - Manual query without JOIN
                var users = connection.Query<dynamic>(
                    "SELECT UserId, Username FROM Users WHERE UserId IN (SELECT DISTINCT UserId FROM Tb_React) ORDER BY Username");

                ddlUserFilter.Items.Clear();
                ddlUserFilter.Items.Add(new ListItem("All Users", ""));
                foreach (var user in users)
                {
                    ddlUserFilter.Items.Add(new ListItem(user.Username, user.UserId.ToString()));
                }

                // Load books for filter - Manual query without JOIN
                var books = connection.Query<dynamic>(
                    "SELECT BookId, BookTitle FROM Tb_Book WHERE BookId IN (SELECT DISTINCT BookId FROM Tb_React) ORDER BY BookTitle");

                ddlBookFilter.Items.Clear();
                ddlBookFilter.Items.Add(new ListItem("All Books", ""));
                foreach (var book in books)
                {
                    ddlBookFilter.Items.Add(new ListItem(book.BookTitle, book.BookId.ToString()));
                }
            }
        }

        private void LoadStats()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                // Total reactions count
                var totalReactions = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Tb_React");
                totalReactionsCount.InnerText = totalReactions.ToString();

                // Unique users count
                var uniqueUsers = connection.ExecuteScalar<int>("SELECT COUNT(DISTINCT UserId) FROM Tb_React");
                uniqueUsersCount.InnerText = uniqueUsers.ToString();

                // Unique books count
                var uniqueBooks = connection.ExecuteScalar<int>("SELECT COUNT(DISTINCT BookId) FROM Tb_React");
                uniqueBooksCount.InnerText = uniqueBooks.ToString();
            }
        }

        private void LoadReactions()
        {
            try
            {
                // Get all reactions first
                List<dynamic> allReactions = GetAllReactions();

                // Apply filters manually
                List<dynamic> filteredReactions = ApplyFilters(allReactions);

                // Apply pagination
                List<dynamic> pagedReactions = ApplyPagination(filteredReactions);

                // Get user details for each reaction
                List<ReactionViewModel> reactionViewModels = GetReactionViewModels(pagedReactions);

                gvReactions.DataSource = reactionViewModels;
                gvReactions.DataBind();

            }
            catch (Exception ex)
            {
                ShowMessage($"Error loading reactions: {ex.Message}", "error");
            }
        }

        private List<dynamic> GetAllReactions()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<dynamic>("SELECT UserId, BookId, HeartCount FROM Tb_React").ToList();
            }
        }

        private List<dynamic> ApplyFilters(List<dynamic> reactions)
        {
            var filtered = reactions;

            // Filter by User
            if (!string.IsNullOrEmpty(ddlUserFilter.SelectedValue))
            {
                int userId = int.Parse(ddlUserFilter.SelectedValue);
                filtered = filtered.Where(r => r.UserId == userId).ToList();
            }

            // Filter by Book
            if (!string.IsNullOrEmpty(ddlBookFilter.SelectedValue))
            {
                int bookId = int.Parse(ddlBookFilter.SelectedValue);
                filtered = filtered.Where(r => r.BookId == bookId).ToList();
            }

            return filtered;
        }

        private List<dynamic> ApplyPagination(List<dynamic> reactions)
        {
            int skip = (currentPage - 1) * pageSize;
            return reactions.Skip(skip).Take(pageSize).ToList();
        }

        private List<ReactionViewModel> GetReactionViewModels(List<dynamic> reactions)
        {
            var viewModels = new List<ReactionViewModel>();

            using (var connection = new SqlConnection(connectionString))
            {
                // Get all unique user IDs from reactions
                var userIds = reactions.Select(r => (int)r.UserId).Distinct().ToList();
                // Get all unique book IDs from reactions
                var bookIds = reactions.Select(r => (int)r.BookId).Distinct().ToList();

                // Get user details - Manual query without JOIN
                Dictionary<int, dynamic> usersDict = new Dictionary<int, dynamic>();
                if (userIds.Any())
                {
                    string userQuery = $"SELECT UserId, Username, Email, ProfileImagePath FROM Users WHERE UserId IN ({string.Join(",", userIds)})";
                    var users = connection.Query<dynamic>(userQuery);
                    usersDict = users.ToDictionary(u => (int)u.UserId, u => u);
                }

                // Get book details - Manual query without JOIN
                Dictionary<int, dynamic> booksDict = new Dictionary<int, dynamic>();
                if (bookIds.Any())
                {
                    string bookQuery = $"SELECT BookId, BookTitle, Author, Category, ImagePath FROM Tb_Book WHERE BookId IN ({string.Join(",", bookIds)})";
                    var books = connection.Query<dynamic>(bookQuery);
                    booksDict = books.ToDictionary(b => (int)b.BookId, b => b);
                }

                // Combine data manually
                foreach (var reaction in reactions)
                {
                    var viewModel = new ReactionViewModel
                    {
                        UserId = reaction.UserId,
                        BookId = reaction.BookId,
                        HeartCount = reaction.HeartCount
                    };

                    // Add user information if available
                    if (usersDict.ContainsKey(reaction.UserId))
                    {
                        var user = usersDict[reaction.UserId];
                        viewModel.Username = user.Username ?? "Unknown User";
                        viewModel.Email = user.Email ?? "No Email";
                        viewModel.ProfileImagePath = user.ProfileImagePath ?? "/uploads/images/default.png";
                    }
                    else
                    {
                        viewModel.Username = "User Not Found";
                        viewModel.Email = "N/A";
                        viewModel.ProfileImagePath = "/uploads/images/default.png";
                    }

                    // Add book information if available
                    if (booksDict.ContainsKey(reaction.BookId))
                    {
                        var book = booksDict[reaction.BookId];
                        viewModel.BookTitle = book.BookTitle ?? "Unknown Book";
                        viewModel.Author = book.Author ?? "Unknown Author";
                        viewModel.Category = book.Category ?? "Uncategorized";
                        viewModel.ImagePath = book.ImagePath ?? "/uploads/images/default-book.png";
                    }
                    else
                    {
                        viewModel.BookTitle = "Book Not Found";
                        viewModel.Author = "N/A";
                        viewModel.Category = "N/A";
                        viewModel.ImagePath = "/uploads/images/default-book.png";
                    }

                    viewModels.Add(viewModel);
                }
            }

            return viewModels;
        }



        protected void ddlUserFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadReactions();
        }

        protected void ddlBookFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadReactions();
        }

        protected void ddlHeartFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadReactions();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadReactions();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadReactions();
        }

        private void ShowMessage(string msg, string type = "info")
        {
            lblMessage.Text = msg;
            lblMessage.Visible = true;

            switch (type.ToLower())
            {
                case "success":
                    lblMessage.CssClass = "text-green-700 mb-4 block text-sm bg-green-50 p-4 rounded-lg text-center border border-green-200";
                    break;
                case "error":
                    lblMessage.CssClass = "text-red-700 mb-4 block text-sm bg-red-50 p-4 rounded-lg text-center border border-red-200";
                    break;
                case "warning":
                    lblMessage.CssClass = "text-yellow-700 mb-4 block text-sm bg-yellow-50 p-4 rounded-lg text-center border border-yellow-200";
                    break;
                default:
                    lblMessage.CssClass = "text-blue-700 mb-4 block text-sm bg-blue-50 p-4 rounded-lg text-center border border-blue-200";
                    break;
            }
        }

        // ViewModel class to hold combined data
        public class ReactionViewModel
        {
            public int UserId { get; set; }
            public int BookId { get; set; }
            public long HeartCount { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string ProfileImagePath { get; set; }
            public string BookTitle { get; set; }
            public string Author { get; set; }
            public string Category { get; set; }
            public string ImagePath { get; set; }
        }
    }
}