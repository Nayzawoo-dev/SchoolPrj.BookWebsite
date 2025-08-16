<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SchoolPrj.BookWebsite.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="max-w-md mx-auto bg-white p-6 rounded-lg shadow-md">
        <h2 class="text-2xl font-bold text-center mb-4 text-indigo-600">📝 Register</h2>

        <!-- Error / Success Message -->


        <!-- Profile Image Upload + Preview -->
        <div class="mb-3 flex-column ">
            <asp:Image ID="imgPreview" runat="server" CssClass="w-24 h-24 mx-auto mb-2 rounded-full border-2 border-indigo-400 object-cover" />
            <asp:FileUpload ID="fuProfile" runat="server" CssClass="w-full" onchange="previewRegisterImage(this)" />
        </div>

        <!-- Full Name -->
        <asp:Label ID="lblNull" runat="server" CssClass="block mt-3 text-red-500"></asp:Label>
        <asp:TextBox ID="txtFullName" runat="server" CssClass="w-full p-2 border rounded mb-3" placeholder="Full Name"></asp:TextBox>

        <!-- Username -->
        <asp:Label ID="lblMessage" runat="server" CssClass="block mt-3 text-red-500"></asp:Label>
        <asp:TextBox ID="txtUsername" runat="server" CssClass="w-full p-2 border rounded mb-3" placeholder="Username"></asp:TextBox>

        <!-- Email -->
        <asp:Label ID="Labelemail" runat="server" CssClass="block mt-3 text-red-500"></asp:Label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="w-full p-2 border rounded mb-3" placeholder="Email"></asp:TextBox>

        <!-- Password -->
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="w-full p-2 border rounded mb-3" placeholder="Password"></asp:TextBox>


        <!-- Register Button -->
        <asp:Button ID="btnRegister" runat="server" Text="Register"
            CssClass="w-full bg-indigo-600 text-white py-2 rounded hover:bg-indigo-700 transition"
            OnClick="btnRegister_Click" />
    </div>

    <!-- JS for Image Preview -->
    <script type="text/javascript">
        function previewRegisterImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    var img = document.getElementById("<%= imgPreview.ClientID %>");
                    img.src = e.target.result;
                };

                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
</asp:Content>
