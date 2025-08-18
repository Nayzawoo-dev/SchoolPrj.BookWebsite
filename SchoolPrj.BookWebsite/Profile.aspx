<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="SchoolPrj.BookWebsite._Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="max-w-xl mx-auto bg-white shadow-lg rounded-lg p-6">
        <div class="mt-4">
            <asp:Label ID="lblMessage" runat="server" CssClass="text-sm"></asp:Label>
        </div>
        <div class="flex items-center space-x-4">
            <asp:Image ID="imgProfile" runat="server" CssClass="w-20 h-20 rounded-full border-2 border-indigo-500" />
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
</asp:Content>
