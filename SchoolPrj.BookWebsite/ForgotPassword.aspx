<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="SchoolPrj.BookWebsite.ForgotPassword" MasterPageFile="~/Site.Master" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="flex items-center justify-center bg-gradient-to-br from-indigo-50 to-purple-100 min-h-screen py-4 px-4 sm:px-6 lg:px-8">
        <div class="max-w-md w-full bg-white rounded-2xl shadow-xl overflow-hidden">
            <!-- Header Section -->
            <div class="bg-gradient-to-r from-indigo-600 to-purple-700 py-6 px-8 text-center">
                <h1 class="text-3xl font-bold text-white mb-2 flex items-center justify-center">
                    <i class="fas fa-book-reader mr-3"></i>Phat Sa Yar
                </h1>
                <p class="text-indigo-100">Reset Your Password</p>
            </div>

            <div class="py-8 px-8">
                <h2 class="text-2xl font-bold text-center text-gray-800 mb-2">Forgot Password?</h2>
                <p class="text-gray-600 text-center mb-6">Enter your email and we'll send you a reset link</p>

                <!-- Message Display -->
                <asp:Label ID="lblMessage" runat="server" CssClass="mb-4 block text-sm p-3 rounded-lg text-center" Visible="false"></asp:Label>

                <div class="space-y-5">
                    <!-- Email Field -->
                    <div>
                        <label class="block text-sm font-medium text-gray-700 mb-2">Email Address</label>
                        <div class="relative">
                            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                <i class="fas fa-envelope text-gray-400"></i>
                            </div>
                            <asp:TextBox ID="txtEmail" runat="server"
                                CssClass="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                placeholder="Enter your registered email" TextMode="Email" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="*Email is required" CssClass="text-red-500 text-xs mt-1 block" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>

                    <!-- Send Reset Link Button -->
                    <asp:Button ID="btnSendResetLink" runat="server" Text="Send Reset Link" OnClick="btnSendResetLink_Click"
                        CssClass="w-full bg-indigo-600 text-white py-3.5 rounded-xl hover:bg-indigo-700 transition-all font-medium shadow-md hover:shadow-lg focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" />

                    <!-- Back to Login Link -->
                    <div class="text-center text-sm mt-4">
                        <asp:HyperLink ID="lnkBackToLogin" runat="server" NavigateUrl="~/Login.aspx"
                            CssClass="font-medium text-indigo-600 hover:text-indigo-500">
                            <i class="fas fa-arrow-left mr-1"></i>Back to Login
                        </asp:HyperLink>
                    </div>
                </div>
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
            }, 8000);
        }

        document.addEventListener('DOMContentLoaded', function () {
            hideMessageAfterDelay();
        });
    </script>
</asp:Content>