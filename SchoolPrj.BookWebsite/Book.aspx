<%@ Page Title="Books" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Book.aspx.cs" Inherits="SchoolPrj.BookWebsite._Book" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Books | MyBookShelf - Explore Our Collection
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <asp:UpdatePanel ID="upBookCategoy" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <!-- 🔍 Search and Action Bar -->
            <div class="bg-white rounded-xl p-4 shadow-md mb-6 border border-gray-100">
                <div class="flex flex-col md:flex-row gap-4 items-center">
                    <!-- Search Section -->
                    <div class="flex-1 flex flex-col sm:flex-row gap-4 w-full">
                        <div class="relative flex-1">
                            <asp:TextBox ID="txtSearch" runat="server"
                                CssClass="w-full pl-12 pr-4 py-3 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                placeholder="Search by title or author"></asp:TextBox>
                            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                <i class="fas fa-search text-gray-400"></i>
                            </div>
                        </div>
                        <asp:Button ID="btnSearch" runat="server" Text="Search"
                            CssClass="px-6 py-3 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-all font-medium shadow-md hover:shadow-lg"
                            OnClick="btnSearch_Click" />
                    </div>

                    <!-- Add Book Button -->
                    <asp:HyperLink ID="btnAddBook" runat="server" NavigateUrl="AddBook.aspx" Visible="false"
                        CssClass="flex items-center justify-center px-6 py-3 bg-green-500 text-white rounded-lg hover:bg-green-600 transition-all font-medium shadow-md hover:shadow-lg">
                        <i class="fas fa-plus-circle mr-2"></i> Add New Book
                    </asp:HyperLink>
                </div>
            </div>

            <!-- ➕ Add Category Panel (Responsive + Consistent with Page) -->
            <asp:Panel ID="add_cat" runat="server" Visible="false"
                CssClass="bg-white rounded-xl shadow-md p-6 mb-8 border border-gray-100 mx-auto w-full">
                <asp:Label ID="lblMessage" runat="server" CssClass="bg-red-50 text-red-700 p-4 rounded-xl mb-6 block text-center" Visible="false"></asp:Label>
                <div class="flex flex-col sm:flex-row items-start sm:items-end gap-4">
                    <!-- Label + TextBox -->
                    <div class="flex-1 w-full">
                        <asp:Label ID="Label1" runat="server"
                            Text="Enter New Category:"
                            CssClass="block text-gray-700 font-medium mb-2"></asp:Label>

                        <asp:TextBox ID="txtCategory" runat="server"
                            CssClass="w-full px-4 py-3 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                            placeholder="e.g. Science Fiction"></asp:TextBox>
                    </div>

                    <!-- Add Button -->
                    <div class="w-full sm:w-auto">
                        <asp:Button ID="btnAdd" runat="server" Text="Add Category"
                            CssClass="w-full sm:w-auto px-6 py-3 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-all font-medium shadow-md hover:shadow-lg"
                            OnClick="btnAdd_Click" />
                    </div>
                </div>
            </asp:Panel>

            <!-- 📚 Books Grid -->
            <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                <asp:Repeater ID="rptBooks" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <div class="bg-white rounded-xl overflow-hidden shadow-lg hover:shadow-2xl transition-all duration-300 transform hover:-translate-y-1 border border-gray-100">
                            <div class="h-56 bg-indigo-50 flex items-center justify-center overflow-hidden">
                                <img src='<%# ResolveUrl(Eval("ImagePath").ToString()) %>'
                                    alt='<%# Eval("BookTitle") %>'
                                    class="w-full h-full object-contain p-4" />
                            </div>

                            <div class="p-5">
                                <h3 class="text-lg font-bold text-gray-800 mb-2 truncate"><%# Eval("BookTitle") %></h3>

                                <div class="flex items-center text-sm text-gray-600 mb-3">
                                    <i class="fas fa-user-circle text-indigo-500 mr-2"></i>
                                    <span class="truncate"><%# Eval("Author") %></span>
                                </div>

                                <div class="flex items-center text-sm text-gray-600 mb-4">
                                    <i class="fas fa-tag text-indigo-500 mr-2"></i>
                                    <span class="truncate"><%# Eval("Category") %></span>
                                </div>

                                <div class="flex justify-between items-center">
                                    <a href='BookDetails.aspx?BookId=<%# Eval("BookId") %>'
                                        class="px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-all text-sm font-medium flex items-center">
                                        <i class="fas fa-eye mr-2"></i>View
                                    </a>

                                    <div class="flex items-center text-sm text-gray-600">
                                        <asp:Literal ID="lblLikeCount" runat="server" Text="" />
                                    </div>

                                    <div class="flex space-x-2">
                                        <asp:HyperLink ID="btnEditBook" runat="server"
                                            NavigateUrl='<%# "EditBook.aspx?BookId=" + Eval("BookId") %>'
                                            Visible="false"
                                            CssClass="px-3 py-2 bg-slate-400 text-white rounded-lg hover:bg-yellow-600 transition-all flex items-center">
                                            <i class="fas fa-edit"></i>
                                        </asp:HyperLink>

                                        <asp:LinkButton ID="btnDelete" runat="server" Visible="false"
                                            CommandArgument='<%# Eval("BookId") %>'
                                            OnClientClick="return confirm('Are you sure you want to delete this book?');"
                                            OnClick="btnDelete_Click"
                                            CssClass="px-3 py-2 bg-rose-500 text-white rounded-lg hover:bg-rose-600 transition-all flex items-center">
                                            <i class="fas fa-trash"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- 🔄 Pagination -->
            <div class="flex justify-between items-center mt-10 space-y-3 sm:space-y-0 sm:space-x-4">
                <asp:Button ID="btnPrev" runat="server" Text="&laquo; Previous"
                    CssClass="px-5 py-2 bg-gray-300 text-gray-700 rounded-lg hover:bg-gray-400 transition-all"
                    OnClick="btnPrev_Click" />
                <asp:Label ID="lblPageInfo" runat="server" CssClass="font-medium text-gray-700"></asp:Label>
                <asp:Button ID="btnNext" runat="server" Text="Next &raquo;"
                    CssClass="px-5 py-2 bg-gray-300 text-gray-700 rounded-lg hover:bg-gray-400 transition-all"
                    OnClick="btnNext_Click" />
            </div>

            <!-- ❌ No Books Found -->
            <asp:Panel ID="pnlNoBooks" runat="server" Visible="false"
                CssClass="mt-12 mb-24 bg-white rounded-xl p-8 shadow-md text-center max-w-2xl mx-auto border border-gray-100">
                <div class="text-6xl text-gray-300 mb-4">
                    <i class="fas fa-book-open"></i>
                </div>
                <h3 class="text-xl font-semibold text-gray-700 mb-2">No books found</h3>
                <p class="text-gray-600 mb-6">We couldn’t find any books matching your search criteria.</p>
                <asp:Button ID="btnUnderstand" runat="server" Text="Browse Books"
                    CssClass="px-6 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-all font-medium"
                    OnClick="btnUnderstand_Click" />
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
