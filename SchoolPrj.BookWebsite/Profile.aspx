<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="SchoolPrj.BookWebsite._Profile" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Profile | MyBookShelf
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <asp:UpdatePanel ID="profile" runat="server">
        <ContentTemplate>


            <div class="max-w-6xl mx-auto py-8 px-4">
                <!-- Message Alert -->
                <asp:Label ID="lblMessage" runat="server" CssClass="bg-red-50 text-red-700 p-4 rounded-xl mb-6 block text-center" Visible="false"></asp:Label>

                <!-- Profile Card -->
                <div class="w-full bg-white shadow-lg rounded-2xl overflow-hidden flex flex-col sm:flex-row mb-8">

                    <!-- Left: Avatar -->
                    <div class="bg-indigo-600 flex flex-col items-center justify-center p-6 sm:w-1/3">
                        <asp:Image ID="imgProfile" runat="server"
                            CssClass="h-28 w-28 sm:h-32 sm:w-32 rounded-full object-cover border-4 border-white shadow-xl mb-4" />
                    </div>

                    <!-- Right: Details -->
                    <div class="flex-1 p-6">
                        <div class="flex justify-between items-start flex-wrap gap-2">
                            <h2 class="text-2xl font-bold text-gray-800">
                                <asp:Label ID="lblFullName" runat="server" />
                            </h2>
                            <span class="px-3 py-1 text-sm bg-indigo-100 text-indigo-700 rounded-full">
                                <asp:Label ID="lblRole" runat="server" />
                            </span>
                        </div>

                        <!-- Contact Info -->
                        <div class="mt-4 space-y-2 text-gray-700">
                            <div class="flex items-center gap-2 mb-4">
                                <img src="/uploads/images/google.png" class="w-5 h-5 mr-2" alt="Google logo">
                                <asp:Label ID="lblEmail" runat="server" />
                            </div>
                            <!-- Buttons -->
                            <div class="mt-6 flex flex-wrap gap-3">
                                <asp:HyperLink ID="btnEdit" runat="server" NavigateUrl="~/EditProfile.aspx"
                                    CssClass="inline-flex items-center justify-center gap-2 rounded-xl bg-indigo-600 px-4 py-2.5 text-white font-medium shadow hover:bg-indigo-700 focus:ring-2 focus:ring-indigo-500">
           <i class="fas fa-edit mr-2"></i>
            Edit Profile
                                </asp:HyperLink>

                                <asp:Button ID="btnLogout" runat="server" Text="Logout"
                                    CssClass="inline-flex items-center justify-center gap-2 rounded-xl bg-rose-500 px-4 py-2.5 text-white font-medium shadow hover:bg-rose-600 focus:ring-2 focus:ring-rose-500" OnClick="btnLogout_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Saved Books Section -->
                <asp:Panel ID="pnlSaveSection" runat="server" Visible="false">
                    <div class="bg-white rounded-2xl shadow-lg overflow-hidden">
                        <div class="bg-gradient-to-r from-indigo-600 to-purple-700 py-4 px-6">
                            <h2 class="text-xl font-bold text-white flex items-center">
                                <i class="fas fa-bookmark mr-2"></i>My Saved Books
                            </h2>
                        </div>

                        <div class="p-6">
                            <asp:Repeater ID="rptSavedBooks" runat="server">
                                <ItemTemplate>
                                    <div class="bg-gray-50 rounded-xl p-4 mb-4 border border-gray-200 hover:border-indigo-300 transition-all">
                                        <div class="flex flex-col md:flex-row items-start gap-4">
                                            <!-- Book Cover -->
                                            <div class="flex-shrink-0">
                                                <img src='<%# ResolveUrl(Eval("BookImagePath").ToString()) %>'
                                                    alt="Book Cover"
                                                    class="w-16 h-20 md:w-20 md:h-24 object-cover rounded-lg shadow-md" />
                                            </div>

                                            <!-- Book Details -->
                                            <div class="flex-1 min-w-0">
                                                <h3 class="text-lg font-semibold text-gray-800 truncate"><%# Eval("BookTitle") %></h3>
                                                <div class="flex items-center text-sm text-gray-600 mt-1 mb-2">
                                                    <i class="fas fa-user-circle text-indigo-500 mr-2"></i>
                                                    <span class="truncate"><%# Eval("Author") %></span>
                                                </div>
                                                <div class="flex items-center text-sm text-gray-600">
                                                    <i class="fas fa-tag text-indigo-500 mr-2"></i>
                                                    <span class="truncate"><%# Eval("Category") %></span>
                                                </div>
                                            </div>

                                            <!-- Action Buttons -->
                                            <div class="flex flex-row sm:flex-row gap-2 md:mt-0">
                                                <a href='BookDetails.aspx?BookId=<%# Eval("BookId") %>'
                                                    class="px-4 py-2 bg-indigo-600 text-white rounded-xl hover:bg-indigo-700 transition-all text-sm font-medium flex items-center justify-center shadow-md">
                                                    <i class="fas fa-eye mr-2"></i>View
                                                </a>

                                                <asp:Button ID="btnDelete" runat="server" Text="Remove"
                                                    CommandArgument='<%# Eval("SaveId") %>' OnClick="btnDelete_Click"
                                                    OnClientClick="return confirm('Are you sure you want to remove this book from your saved items?');"
                                                    CssClass="px-4 py-2 bg-red-500 text-white rounded-xl hover:bg-red-600 transition-all text-sm font-medium flex items-center justify-center shadow-md" />
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Empty State -->
                <asp:Panel ID="pnlNoSavedBooks" runat="server" Visible="false" CssClass="text-center py-10">
                    <div class="text-6xl text-gray-300 mb-4">
                        <i class="fas fa-book-open"></i>
                    </div>
                    <h3 class="text-xl font-semibold text-gray-700 mb-2">No saved books yet</h3>
                    <p class="text-gray-500">Books you save will appear here</p>
                    <a href="Book.aspx" class="inline-block mt-4 px-6 py-2 bg-indigo-600 text-white rounded-xl hover:bg-indigo-700 transition-all font-medium">Browse Books
                    </a>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        function hideMessageAfterDelay() {
            setTimeout(function () {
                var msg = document.getElementById('<%= lblMessage.ClientID %>');
                if (msg) {
                    msg.style.display = 'none';
                }
            }, 5000);
        }

        // Initialize on page load
        document.addEventListener('DOMContentLoaded', function () {
            hideMessageAfterDelay();
        });
    </script>
</asp:Content>
