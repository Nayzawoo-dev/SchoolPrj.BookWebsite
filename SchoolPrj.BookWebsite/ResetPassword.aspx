<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="SchoolPrj.BookWebsite.ResetPassword" MasterPageFile="~/Site.Master" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="flex items-center justify-center bg-gradient-to-br from-indigo-50 to-purple-100 min-h-screen py-4 px-4 sm:px-6 lg:px-8">
        <div class="max-w-md w-full bg-white rounded-2xl shadow-xl overflow-hidden">
            <!-- Header Section -->
            <div class="bg-gradient-to-r from-indigo-600 to-purple-700 py-6 px-8 text-center">
                <h1 class="text-3xl font-bold text-white mb-2 flex items-center justify-center">
                    <i class="fas fa-book-reader mr-3"></i>Phat Sa Yar
                </h1>
                <p class="text-indigo-100">Create New Password</p>
            </div>

            <div class="py-8 px-8">
                <h2 class="text-2xl font-bold text-center text-gray-800 mb-2">Set New Password</h2>
                <p class="text-gray-600 text-center mb-6">Enter your new password below</p>

                <!-- Message Display -->
                <asp:Label ID="lblMessage" runat="server" CssClass="mb-4 block text-sm p-3 rounded-lg text-center" Visible="false"></asp:Label>

                <asp:HiddenField ID="hdnResetToken" runat="server" />

                <div class="space-y-5">
                    <!-- New Password Field -->
                    <div>
                        <label class="block text-sm font-medium text-gray-700 mb-2">New Password</label>
                        <div class="relative">
                            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                <i class="fas fa-lock text-gray-400"></i>
                            </div>
                            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"
                                CssClass="w-full pl-10 pr-12 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                placeholder="Enter new password" />
                            <!-- Eye icon for password visibility toggle -->
                            <div class="absolute inset-y-0 right-0 pr-3 flex items-center">
                                <button type="button" id="toggleNewPassword" class="text-gray-400 hover:text-gray-500 focus:outline-none">
                                    <i class="fas fa-eye" id="eyeIconNew"></i>
                                </button>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword"
                            ErrorMessage="*New password is required" CssClass="text-red-500 text-xs mt-1 block" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtNewPassword"
                            ErrorMessage="*Password must be at least 6 characters" ValidationExpression=".{6,}"
                            CssClass="text-red-500 text-xs mt-1 block" Display="Dynamic"></asp:RegularExpressionValidator>
                    </div>

                    <!-- Confirm Password Field -->
                    <div>
                        <label class="block text-sm font-medium text-gray-700 mb-2">Confirm Password</label>
                        <div class="relative">
                            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                <i class="fas fa-lock text-gray-400"></i>
                            </div>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"
                                CssClass="w-full pl-10 pr-12 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                placeholder="Confirm new password" />
                            <!-- Eye icon for password visibility toggle -->
                            <div class="absolute inset-y-0 right-0 pr-3 flex items-center">
                                <button type="button" id="toggleConfirmPassword" class="text-gray-400 hover:text-gray-500 focus:outline-none">
                                    <i class="fas fa-eye" id="eyeIconConfirm"></i>
                                </button>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                            ErrorMessage="*Please confirm your password" CssClass="text-red-500 text-xs mt-1 block" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvPasswords" runat="server" ControlToValidate="txtConfirmPassword"
                            ControlToCompare="txtNewPassword" Operator="Equal" Type="String"
                            ErrorMessage="*Passwords do not match" CssClass="text-red-500 text-xs mt-1 block" Display="Dynamic"></asp:CompareValidator>
                    </div>

                    <!-- Reset Password Button -->
                    <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" OnClick="btnResetPassword_Click"
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

        // Password visibility toggle functions
        function setupPasswordToggles() {
            // New password toggle
            const toggleNewPassword = document.getElementById('toggleNewPassword');
            const newPasswordInput = document.getElementById('<%= txtNewPassword.ClientID %>');
            const eyeIconNew = document.getElementById('eyeIconNew');

            if (toggleNewPassword && newPasswordInput && eyeIconNew) {
                toggleNewPassword.addEventListener('click', function () {
                    const type = newPasswordInput.getAttribute('type') === 'password' ? 'text' : 'password';
                    newPasswordInput.setAttribute('type', type);
                    eyeIconNew.className = type === 'text' ? 'fas fa-eye-slash' : 'fas fa-eye';
                });
            }

            // Confirm password toggle
            const toggleConfirmPassword = document.getElementById('toggleConfirmPassword');
            const confirmPasswordInput = document.getElementById('<%= txtConfirmPassword.ClientID %>');
            const eyeIconConfirm = document.getElementById('eyeIconConfirm');

            if (toggleConfirmPassword && confirmPasswordInput && eyeIconConfirm) {
                toggleConfirmPassword.addEventListener('click', function () {
                    const type = confirmPasswordInput.getAttribute('type') === 'password' ? 'text' : 'password';
                    confirmPasswordInput.setAttribute('type', type);
                    eyeIconConfirm.className = type === 'text' ? 'fas fa-eye-slash' : 'fas fa-eye';
                });
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            hideMessageAfterDelay();
            setupPasswordToggles();
        });
    </script>
</asp:Content>