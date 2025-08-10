<%@ Page Title="Books" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Book.aspx.cs" Inherits="SchoolPrj.BookWebsite._Book" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Top Bar: Search + Add Book -->
    <div class="flex flex-col sm:flex-row items-center mb-6 gap-4">
        <!-- Search Bar -->
        <div class="relative w-full max-w-full sm:max-w-sm">
            <asp:TextBox ID="txtSearch" runat="server"
                CssClass="w-full pl-10 pr-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-indigo-500 text-sm sm:text-base"
                placeholder="Search books...">
            </asp:TextBox>
            <svg xmlns="http://www.w3.org/2000/svg"
                class="h-5 w-5 text-gray-400 absolute left-3 top-2.5"
                fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                    d="M21 21l-4.35-4.35m0 0A7.5 7.5 0 1110.5 3a7.5 7.5 0 016.15 13.65z" />
            </svg>
        </div>
        <!-- Search Button -->
        <asp:Button ID="btnSearch" runat="server" Text="Search"
            CssClass="w-full sm:w-auto px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition text-sm sm:text-base"
            OnClick="btnSearch_Click" />
        <!-- Add New Book Button -->
        <a href="AddBook.aspx" class="w-full sm:w-auto flex justify-center items-center px-4 py-2 bg-indigo-600 text-white rounded-lg shadow hover:bg-indigo-700 transition text-sm sm:text-base">Add</a>
    </div>


    <!-- Books Grid -->
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        <asp:Repeater ID="rptBooks" runat="server">
            <ItemTemplate>
                <div class="bg-white shadow-lg rounded-xl overflow-hidden hover:shadow-2xl transition-shadow duration-300">
                    <img src='<%# ResolveUrl(Eval("ImagePath").ToString()) %>'
                        alt='<%# Eval("BookTitle") %>'
                        class="w-full h-48 object-cover" />
                    <div class="p-4">
                        <h2 class="text-lg font-bold text-indigo-600"><%# Eval("BookTitle") %></h2>
                        <p class="text-sm text-gray-500 mb-2">
                            📚 <%# Eval("Category") %> | ✍ <%# Eval("Author") %>
                        </p>
                        <a href='<%# ResolveUrl(Eval("PdfPath").ToString()) %>' target="_blank"
                            class="inline-block px-4 py-2 bg-indigo-500 text-white rounded-lg hover:bg-indigo-600 transition">📖 View Book
                        </a>

                        <a href='<%# ResolveUrl(Eval("PdfPath").ToString()) %>' target="_blank"
                            class="inline-block px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition">❌ Delete Book
                        </a>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- Warning Message -->
    <asp:Panel ID="pnlNoBooks" runat="server" Visible="false"
        CssClass="mt-10 mx-auto max-w-xs sm:max-w-md p-4 bg-gray-50 border border-gray-200 rounded-lg shadow-sm text-center">
        <div class="text-gray-700 text-sm sm:text-md mb-3">
            😕 No books found matching your search.
        </div>
        <asp:Button ID="btnUnderstand" runat="server" Text="I Understand"
            CssClass="px-4 py-2 bg-gray-200 text-gray-800 rounded hover:bg-gray-300 transition text-sm sm:text-base"
            OnClick="btnUnderstand_Click" />
    </asp:Panel>
</asp:Content>

