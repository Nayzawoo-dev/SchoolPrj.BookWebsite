<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="SchoolPrj.BookWebsite._Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="max-w-xl mx-auto bg-white shadow-lg rounded-lg p-6">
        <div class="mt-4">
            <asp:Label ID="lblMessage" runat="server" CssClass="text-sm"></asp:Label>
        </div>
        <div class="flex items-center space-x-4">
            <asp:Image ID="imgProfile" runat="server" CssClass="w-24 h-24 rounded-full border-4 border-white shadow-lg object-cover mx-auto" />
            <div>
                <h2 class="text-2xl font-bold text-gray-800">
                    <asp:Label ID="lblFullName" runat="server" />
                </h2>
                <p class="text-gray-500">
                    <asp:Label ID="lblEmail" runat="server" />
                </p>
                <span class="inline-block mt-1 px-3 py-1 text-sm bg-indigo-100 text-indigo-700 rounded-full">
                    <asp:Label ID="lblRole" runat="server" />
                </span>
            </div>
        </div>

        <div class="mt-6 space-x-4 ">
            <asp:HyperLink ID="btnEdit" runat="server" NavigateUrl="~/EditProfile.aspx"
                CssClass="px-4 py-2 bg-indigo-500 text-white rounded-lg shadow hover:bg-indigo-600 transition">
                ✏️ Edit Profile
            </asp:HyperLink>

            <asp:Button ID="btnLogout" runat="server" Text="🚪 Logout"
                CssClass="px-4 py-2 bg-red-500 text-white rounded-lg shadow hover:bg-red-600 transition"
                OnClick="btnLogout_Click" />
        </div>
    </div>

    <!-- Saved Books Section -->

    <!-- Save Section -->
    <asp:Panel ID="pnlSaveSection" runat="server" Visible="false" CssClass="flex flex-col sm:flex-row gap-4">
        <div class="max-w-4xl mx-auto mt-10 bg-white shadow-lg rounded-lg p-6">
            <h2 class="text-2xl font-bold text-gray-800 mb-4">📚 My Saved Books</h2>
            <asp:Repeater ID="rptSavedBooks" runat="server">
                <ItemTemplate>
                    <div class="flex items-center justify-between border-b py-3">
                        <div class="flex items-center space-x-4">
                            <img src='<%# ResolveUrl(Eval("BookImagePath").ToString()) %>' alt="Book Cover" class="w-16 h-20 object-cover rounded" />
                            <div>
                                <h3 class="text-lg font-semibold text-indigo-600"><%# Eval("BookTitle") %></h3>
                                <p class="text-sm text-gray-500">✍️ <%# Eval("Author") %> | 📚 <%# Eval("Category") %></p>
                            </div>
                        </div>

                        <a href='BookDetails.aspx?BookId=<%# Eval("BookId") %>'
                            class="inline-block px-4 py-2 bg-indigo-500 text-white rounded-lg hover:bg-indigo-600 transition">📖 View Book  
                        </a>

                        <asp:Button ID="btnDelete" runat="server" Text="🗑" CommandArgument='<%# Eval("SaveId") %>' OnClick="btnDelete_Click"
                            OnClientClick="return confirm('Are you sure want to delete?');"
                            CssClass="btn inline-block px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:Panel>

    <script>
        function hideMessageAfterDelay() {
            const message = document.getElementById('<%= lblMessage.ClientID %>');
              if (message && message.innerText.trim() !== '') {
                  setTimeout(function () {
                      message.classList.add('hidden');
                  }, 5000); // Hide after 5 seconds
              }
          }
    </script>
</asp:Content>
