<%@ Page Title="Category" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="SchoolPrj.BookWebsite._Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Top Bar: Search + Add Book -->
    <div class="flex flex-col sm:flex-row items-center mb-6 gap-4">
        <!-- Filter -->
        <div class="flex flex-row w-full max-w-full sm:max-w-sm gap-5">
            <asp:DropDownList ID="ddlAuthor" runat="server" CssClass="w-full pl-2 pr-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-indigo-500 text-sm sm:text-base">
                <asp:ListItem Text="Select Author" Value="" />
                <asp:ListItem Text="Thaung Naing" Value="Thaung Naing" />
                <asp:ListItem Text="Saw Chit" Value="Saw Chit" />
            </asp:DropDownList>

            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="w-full pl-2 pr-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-indigo-500 text-sm sm:text-base">
                <asp:ListItem Text="Select Category" Value="" />
                <asp:ListItem Text="History" Value="History" />
                <asp:ListItem Text="Romance" Value="Romance" />
            </asp:DropDownList>
        </div>


        <asp:Button CssClass="w-full sm:w-auto px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition text-sm sm:text-base"
            ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" />
    </div>


    <!-- Books Grid -->
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        <asp:Repeater ID="repBooks" runat="server">
            <ItemTemplate>
                <div class="bg-white shadow-lg rounded-xl overflow-hidden hover:shadow-2xl transition-shadow duration-300">
                    <img src='<%# ResolveUrl(Eval("ImagePath").ToString()) %>'
                        alt='<%# Eval("BookTitle") %>'
                        class="w-full h-48 object-contain" />
                    <div class="p-4">
                        <h2 class="text-lg font-bold text-indigo-600"><%# Eval("BookTitle") %></h2>
                        <p class="text-sm text-gray-500 mb-2">
                            📚 <%# Eval("Category") %> | ✍ <%# Eval("Author") %>
                        </p>
                        <a href='BookDetails.aspx?BookId=<%# Eval("BookId") %>' target="_blank"
                            class="inline-block px-4 py-2 bg-indigo-500 text-white rounded-lg hover:bg-indigo-600 transition">📖 View Book
                        </a>



                        <asp:Button ID="btnDelete" runat="server" Text="Delete Book"
                            CommandArgument='<%# Eval("BookId") %>'
                            OnClick="btnDelete_Click"
                            OnClientClick="return confirm('Are you sure want to delete?');"
                            CssClass="btn inline-block px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition" />

                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- Warning Message -->
    <asp:Panel ID="pnlNoBook" runat="server" Visible="false"
        CssClass="mt-10 mx-auto max-w-xs sm:max-w-md p-4 bg-gray-50 border border-gray-200 rounded-lg shadow-sm text-center">
        <div class="text-gray-700 text-sm sm:text-md mb-3">
            😕 No books found matching your search.
        </div>
        <asp:Button ID="btnUn" runat="server" Text="I Understand"
            CssClass="px-4 py-2 bg-gray-200 text-gray-800 rounded hover:bg-gray-300 transition text-sm sm:text-base"
            OnClick="btnUnderstand_Click" />
    </asp:Panel>
</asp:Content>

