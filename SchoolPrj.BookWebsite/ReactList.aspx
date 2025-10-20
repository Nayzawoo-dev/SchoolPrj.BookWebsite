<%@ Page Title="React List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReactList.aspx.cs" Inherits="SchoolPrj.BookWebsite.ReactList" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    React List | Admin Panel
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="min-h-screen bg-gradient-to-br from-indigo-50 to-purple-100 py-8 px-4">
        <div class="max-w-7xl mx-auto">
            <!-- Header -->
            <div class="flex justify-between items-center mb-8">
                <div>
                    <h1 class="text-3xl font-bold text-gray-800">Book Reactions</h1>
                    <p class="text-gray-600 mt-2">View all user reactions to books</p>
                </div>
                <div class="flex items-center space-x-4">
                    <!-- Stats -->
                    <div class="bg-white rounded-lg shadow-sm p-4 text-center">
                        <div class="text-2xl font-bold text-indigo-600" id="totalReactionsCount" runat="server">0</div>
                        <div class="text-sm text-gray-600">Total Reactions</div>
                    </div>
                    <div class="bg-white rounded-lg shadow-sm p-4 text-center">
                        <div class="text-2xl font-bold text-green-600" id="uniqueUsersCount" runat="server">0</div>
                        <div class="text-sm text-gray-600">Unique Users</div>
                    </div>
                    <div class="bg-white rounded-lg shadow-sm p-4 text-center">
                        <div class="text-2xl font-bold text-purple-600" id="uniqueBooksCount" runat="server">0</div>
                        <div class="text-sm text-gray-600">Unique Books</div>
                    </div>
                </div>
            </div>

            <!-- Filters -->
            <div class="bg-white rounded-2xl shadow-lg p-6 mb-6">
                <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
                    <!-- User Filter -->
                    <div>
                        <label class="block text-sm font-medium text-gray-700 mb-2">Filter by User</label>
                        <asp:DropDownList ID="ddlUserFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUserFilter_SelectedIndexChanged"
                            CssClass="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent">
                            <asp:ListItem Value="">All Users</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- Book Filter -->
                    <div>
                        <label class="block text-sm font-medium text-gray-700 mb-2">Filter by Book</label>
                        <asp:DropDownList ID="ddlBookFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBookFilter_SelectedIndexChanged"
                            CssClass="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent">
                            <asp:ListItem Value="">All Books</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>
            </div>

            <!-- Message -->
            <asp:Label ID="lblMessage" runat="server" CssClass="mb-4 block text-sm p-4 rounded-lg text-center" Visible="false"></asp:Label>

            <!-- Reactions Grid -->
            <div class="bg-white rounded-2xl shadow-lg overflow-hidden">
                <asp:GridView ID="gvReactions" runat="server" AutoGenerateColumns="false"
                    CssClass="w-full" 
                    HeaderStyle-CssClass="bg-gray-50 text-gray-700 font-semibold" 
                    RowStyle-CssClass="hover:bg-gray-50 transition-colors"
                    AlternatingRowStyle-CssClass="bg-gray-50"
                    EmptyDataText="No reactions found">
                    <Columns>
                        <asp:TemplateField HeaderText="User">
                            <ItemTemplate>
                                <div class="flex items-center space-x-3 py-4">
                                    <asp:Image ID="imgUser" runat="server" 
                                        ImageUrl='<%# Eval("ProfileImagePath") %>'
                                        CssClass="w-10 h-10 rounded-full object-cover border-2 border-gray-200" 
                                        onerror="this.src='data:image/svg+xml,%3Csvg xmlns=\'http://www.w3.org/2000/svg\' fill=\'none\' viewBox=\'0 0 24 24\' stroke=\'%239C92AC\'%3E%3Cpath stroke-linecap=\'round\' stroke-linejoin=\'round\' stroke-width=\'1\' d=\'M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z\'%3E%3C/path%3E%3C/svg%3E'" />
                                    <div>
                                        <div class="font-medium text-gray-900"><%# Eval("Username") %></div>
                                        <div class="text-sm text-gray-500"><%# Eval("Email") %></div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Book">
                            <ItemTemplate>
                                <div class="flex items-center space-x-3 py-4">
                                    <asp:Image ID="imgBook" runat="server" 
                                        ImageUrl='<%# Eval("ImagePath") %>'
                                        CssClass="w-12 h-16 object-cover rounded-lg shadow-sm border border-gray-200" 
                                        onerror="this.src='data:image/svg+xml,%3Csvg xmlns=\'http://www.w3.org/2000/svg\' fill=\'none\' viewBox=\'0 0 24 24\' stroke=\'%239C92AC\'%3E%3Cpath stroke-linecap=\'round\' stroke-linejoin=\'round\' stroke-width=\'1\' d=\'M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.746 0 3.332.477 4.5 1.253v13C19.832 18.477 18.246 18 16.5 18c-1.746 0-3.332.477-4.5 1.253\'%3E%3C/path%3E%3C/svg%3E'" />
                                    <div>
                                        <div class="font-medium text-gray-900"><%# Eval("BookTitle") %></div>
                                        <div class="text-sm text-gray-500">by <%# Eval("Author") %></div>
                                        <div class="text-xs text-gray-400 mt-1"><%# Eval("Category") %></div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Reaction">
                            <ItemTemplate>
                                <div class="text-center py-4">
                                    <div class="flex items-center justify-center space-x-1">
                                        <i class="fas fa-heart text-red-500 text-lg"></i>
                                        <span class="text-xl font-bold text-gray-800"><%# Eval("HeartCount") %></span>
                                    </div>
                                    <div class="text-xs text-gray-500 mt-1">hearts</div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Details">
                            <ItemTemplate>
                                <div class="text-center py-4">
                                    <div class="text-sm text-gray-600">User ID: <%# Eval("UserId") %></div>
                                    <div class="text-sm text-gray-600">Book ID: <%# Eval("BookId") %></div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <EmptyDataTemplate>
                        <div class="text-center py-12">
                            <i class="fas fa-heart text-gray-400 text-4xl mb-4"></i>
                            <h3 class="text-lg font-medium text-gray-900 mb-2">No reactions found</h3>
                            <p class="text-gray-500">There are no book reactions matching your criteria.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>

            
        </div>
    </div>

    <script>
        function hideMessageAfterDelay() {
            const message = document.getElementById('<%= lblMessage.ClientID %>');
            if (message && message.innerText.trim() !== '') {
                setTimeout(function () {
                    message.style.display = 'none';
                }, 5000);
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            hideMessageAfterDelay();
        });
    </script>
</asp:Content>