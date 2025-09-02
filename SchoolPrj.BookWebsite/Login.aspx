<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SchoolPrj.BookWebsite._Login" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <asp:UpdatePanel ID="upBookCategoy" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="flex items-center justify-center bg-gradient-to-br from-indigo-50 to-purple-100 py-4 px-4 sm:px-6 lg:px-8">
                <div class="max-w-md w-full bg-white rounded-2xl shadow-xl overflow-hidden">
                    <!-- Decorative Header -->
                    <div class="bg-gradient-to-r from-indigo-600 to-purple-700 py-6 px-8 text-center">
                        <h1 class="text-3xl font-bold text-white mb-2 flex items-center justify-center">
                            <i class="fas fa-book-reader mr-3"></i>MyBookShelf
                        </h1>
                        <p class="text-indigo-100">Sign in to your reading journey</p>
                    </div>

                    <div class="py-4 px-4">
                        <!-- Error Message -->
                        <asp:Label ID="lblMessage" runat="server" CssClass="text-red-500 mb-4 block text-sm bg-red-50 p-3 rounded-lg text-center" Visible="false"></asp:Label>

                        <div class="space-y-5">
                            <!-- Username Field -->
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-2">Username or Email</label>
                                <div class="relative">
                                    <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                        <i class="fas fa-user text-gray-400"></i>
                                    </div>
                                    <asp:TextBox ID="txtUsername" runat="server"
                                        CssClass="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                        placeholder="Enter your username or email" />
                                </div>
                            </div>

                            <!-- Password Field -->
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-2">Password</label>
                                <div class="relative">
                                    <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                        <i class="fas fa-lock text-gray-400"></i>
                                    </div>
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                                        CssClass="w-full pl-10 pr-12 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                        placeholder="Enter your password" />
                                    <!-- Eye icon for password visibility toggle -->
                                    <div class="absolute inset-y-0 right-0 pr-3 flex items-center">
                                        <button type="button" id="togglePassword" class="text-gray-400 hover:text-gray-500 focus:outline-none">
                                            <i class="fas fa-eye" id="eyeIcon"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>

                            <!-- Login Button -->
                            <asp:Button ID="btnLogin" runat="server" Text="Sign In" OnClick="btnLogin_Click"
                                CssClass="w-full bg-indigo-600 text-white py-3.5 rounded-xl hover:bg-indigo-700 transition-all font-medium shadow-md hover:shadow-lg focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" />

                            <!-- Divider -->
                            <div class="relative">
                                <div class="absolute inset-0 flex items-center">
                                    <div class="w-full border-t border-gray-300"></div>
                                </div>
                                <div class="relative flex justify-center text-sm">
                                    <span class="px-2 bg-white text-gray-500">Or continue with</span>
                                </div>
                            </div>

                            <!-- Google Login Button -->
                            <asp:HyperLink ID="lnkGoogleLogin" runat="server"
                                NavigateUrl="~/GoogleLogin.aspx"
                                CssClass="w-full bg-white border border-gray-300 text-gray-700 py-3 rounded-xl hover:bg-gray-50 transition-all font-medium shadow-sm flex items-center justify-center">
                        <img src="/uploads/images/google.png" class="w-5 h-5 mr-2" alt="Google logo">
                        Sign in with Google
                            </asp:HyperLink>

                            <!-- Register Link -->
                            <div class="text-center text-sm mt-4">
                                <span class="text-gray-600">Don't have an account?</span>
                                <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Register.aspx"
                                    CssClass="font-medium text-indigo-600 hover:text-indigo-500 ml-1">Register now</asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
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

        // Password visibility toggle function
        function setupPasswordToggle() {
            const togglePassword = document.getElementById('togglePassword');
            const passwordInput = document.getElementById('<%= txtPassword.ClientID %>');
            const eyeIcon = document.getElementById('eyeIcon');

            if (togglePassword && passwordInput && eyeIcon) {
                togglePassword.addEventListener('click', function () {
                    // Toggle the password visibility
                    const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
                    passwordInput.setAttribute('type', type);

                    // Toggle the eye icon
                    if (type === 'text') {
                        eyeIcon.classList.remove('fa-eye');
                        eyeIcon.classList.add('fa-eye-slash');
                    } else {
                        eyeIcon.classList.remove('fa-eye-slash');
                        eyeIcon.classList.add('fa-eye');
                    }
                });
            }
        }

        Sys.Application.add_load(function () {
            hideMessageAfterDelay();
            setupPasswordToggle();
        });
    </script>
</asp:Content>
