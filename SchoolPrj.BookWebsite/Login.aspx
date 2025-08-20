<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SchoolPrj.BookWebsite._Login" MasterPageFile="~/Site.Master" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="flex justify-center items-center min-h-screen bg-gray-50">
        <div class="bg-white p-8 rounded-2xl shadow-lg w-full max-w-md">
            <h2 class="text-2xl font-bold text-center text-indigo-600 mb-6">Login</h2>

            <asp:Label ID="lblMessage" runat="server" CssClass="text-red-500 mb-4 block"></asp:Label>

            <div class="space-y-4">
                <div>
                    <label class="block text-sm font-medium text-gray-700">Username or Email</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="w-full p-2 border rounded-lg" />
                </div>

                <div>
                    <label class="block text-sm font-medium text-gray-700">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="w-full p-2 border rounded-lg" />
                </div>

                <!-- Google Login Button -->
                <asp:HyperLink ID="lnkGoogleLogin" runat="server"
                    NavigateUrl="~/GoogleLogin.aspx"
                    CssClass="w-full bg-red-600 text-white py-2 rounded-lg hover:bg-red-700 transition block text-center">
                    Login with Google
                </asp:HyperLink>


                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click"
                    CssClass="w-full bg-indigo-600 text-white py-2 rounded-lg hover:bg-indigo-700 transition" />
                <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Register.aspx"
                    CssClass="text-indigo-600 hover:text-indigo-800 block text-center mt-4">Don't have an account? Register</asp:HyperLink>
            </div>
        </div>
    </div>

    <script>
        function hideMessageAfterDelay() {
            setTimeout(function () {
                var msg = document.getElementById('<%= lblMessage.ClientID %>');
                if (msg) {
                    msg.style.display = 'none';
                }
            }, 5000);
                }
    </script>

</asp:Content>
