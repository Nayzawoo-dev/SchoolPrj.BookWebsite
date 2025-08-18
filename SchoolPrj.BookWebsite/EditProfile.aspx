<%@ Page Title="Edit Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="SchoolPrj.BookWebsite.EditProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="max-w-xl mx-auto bg-white shadow-lg rounded-lg p-6">
        <h2 class="text-2xl font-bold text-gray-800 mb-6">✏️ Edit Profile</h2>
        <!-- Success / Error Message -->

        <!-- Profile Image -->
        <div class="flex items-center mb-4 space-x-4">
            <asp:Image ID="imgProfile" runat="server" CssClass="w-20 h-20 rounded-full border-2 border-indigo-500 object-cover" />
            <asp:FileUpload ID="fileUploadProfile" runat="server" CssClass="text-sm" onchange="previewImage(this)" />
        </div>

        <div class="mt-4">
            <asp:Label ID="lblMessage" runat="server" CssClass="text-sm"></asp:Label>
        </div>
        <!-- Full Name -->
        <div class="mb-4">
            <label class="block text-gray-700 mb-1">Full Name</label>
            <asp:TextBox ID="txtFullName" runat="server" CssClass="w-full px-4 py-2 border rounded-lg focus:ring focus:ring-indigo-300" />
        </div>

        <!-- Email -->
        <div class="mb-4">
            <label class="block text-gray-700 mb-1">Email</label>
            <asp:TextBox ID="txtEmail" ReadOnly runat="server" CssClass="w-full px-4 py-2 border rounded-lg focus:ring focus:ring-indigo-300" />
        </div>

        <!-- Password (Optional) -->
        <div class="mb-4">
            <label class="block text-gray-700 mb-1">New Password (leave blank if not changing)</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="w-full px-4 py-2 border rounded-lg focus:ring focus:ring-indigo-300" />
        </div>

        <!-- Save Button -->
        <div class="flex justify-between">
            <asp:Button ID="btnSave" runat="server" Text="💾 Save Changes"
                CssClass="px-4 py-2 bg-indigo-500 text-white rounded-lg shadow hover:bg-indigo-600 transition"
                OnClick="btnSave_Click" />
            <asp:HyperLink ID="btnCancel" runat="server" NavigateUrl="~/Profile.aspx"
                CssClass="px-4 py-2 bg-gray-300 text-gray-700 rounded-lg shadow hover:bg-gray-400 transition">
                ✖️ Cancel
            </asp:HyperLink>
        </div>


    </div>

    <!-- 🔥 JavaScript for Preview -->
    <script type="text/javascript">
        function previewImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    var img = document.getElementById("<%= imgProfile.ClientID %>");
                    img.src = e.target.result;
                };

                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>

</asp:Content>
