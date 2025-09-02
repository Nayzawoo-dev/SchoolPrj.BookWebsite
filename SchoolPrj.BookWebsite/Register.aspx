<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SchoolPrj.BookWebsite.Register" MaintainScrollPositionOnPostback="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create Account | MyBookShelf
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="flex items-center justify-center bg-gradient-to-br from-indigo-50 to-purple-100 py-4 px-4">
        <div class="max-w-lg w-full bg-white rounded-2xl shadow-xl overflow-hidden">
            <!-- Header Section -->
            <div class="bg-gradient-to-r from-indigo-600 to-purple-700 py-6 px-8 text-center">
                <h1 class="text-3xl font-bold text-white mb-2 flex items-center justify-center">
                    <i class="fas fa-book-reader mr-3"></i>MyBookShelf
                </h1>
                <p class="text-indigo-100">Join our community of readers</p>
            </div>

            <div class="py-8 px-8">
                <h2 class="text-2xl font-bold text-center text-gray-800 mb-6">Create Your Account</h2>

                <!-- Profile Image Upload -->
                <div class="mb-6 text-center">
                    <div class="relative inline-block">
                        <asp:Image ID="imgPreview" runat="server"
                            CssClass="w-24 h-24 rounded-full border-4 border-white shadow-lg object-cover mx-auto"
                            ImageUrl="data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 24 24' stroke='%239C92AC'%3E%3Cpath stroke-linecap='round' stroke-linejoin='round' stroke-width='1' d='M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z'%3E%3C/path%3E%3C/svg%3E" />

                        <!-- label အကြောင့် FileInput ကို click လုပ်လို့ရပြီ၊ JS click မလိုတော့ -->
                        <label for="<%= fuProfile.ClientID %>" class="absolute bottom-0 right-0 bg-indigo-600 text-white px-2 py-1 rounded-full shadow-md cursor-pointer hover:bg-indigo-700 transition-all">
                            <i class="fas fa-camera text-sm"></i>
                        </label>

                        <asp:FileUpload ID="fuProfile" runat="server"
                            CssClass="hidden" onchange="previewRegisterImage(this)" />
                    </div>
                    <p class="text-sm text-gray-500 mt-2">Click the camera to upload a profile photo</p>
                </div>

                <!-- Error / Success Message -->
                <asp:Label ID="lblMessage" runat="server"
                    CssClass="bg-red-50 text-red-700 p-4 rounded-xl mb-6 block text-center" Visible="false"></asp:Label>

                <div class="space-y-5">
                    <!-- Full Name -->
                    <div>
                        <label class="block text-sm font-medium text-gray-700 mb-2">Full Name</label>
                        <div class="relative">
                            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                <i class="fas fa-user text-gray-400"></i>
                            </div>
                            <asp:TextBox ID="txtFullName" runat="server"
                                CssClass="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                placeholder="Enter your full name" />
                        </div>
                    </div>

                    <!-- Username -->
                    <div>
                        <label class="block text-sm font-medium text-gray-700 mb-2">Username</label>
                        <div class="relative">
                            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                <i class="fas fa-at text-gray-400"></i>
                            </div>
                            <asp:TextBox ID="txtUsername" runat="server"
                                CssClass="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                placeholder="Choose a username" />
                        </div>
                    </div>

                    <!-- Email -->
                    <div>
                        <label class="block text-sm font-medium text-gray-700 mb-2">Email</label>
                        <div class="relative">
                            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                <i class="fas fa-envelope text-gray-400"></i>
                            </div>
                            <asp:TextBox ID="txtEmail" runat="server"
                                CssClass="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                placeholder="Enter your email" />
                        </div>
                    </div>

                    <!-- Password -->
                    <div>
                        <label class="block text-sm font-medium text-gray-700 mb-2">Password</label>
                        <div class="relative">
                            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                <i class="fas fa-lock text-gray-400"></i>
                            </div>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                                CssClass="w-full pl-10 pr-12 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                placeholder="Create a password" />
                            <!-- Eye icon for password visibility toggle -->
                            <div class="absolute inset-y-0 right-0 pr-3 flex items-center">
                                <button type="button" id="togglePassword" class="text-gray-400 hover:text-gray-500 focus:outline-none">
                                    <i class="fas fa-eye" id="eyeIcon"></i>
                                </button>
                            </div>
                        </div>
                    </div>

                    <!-- Terms and Conditions -->
                    <div class="flex items-center">
                        <input id="terms" name="terms" type="checkbox" class="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded">
                        <label for="terms" class="ml-2 block text-sm text-gray-700">
                            I agree to the <a href="#" class="text-indigo-600 hover:text-indigo-500">Terms and Conditions</a>
                        </label>
                    </div>

                    <!-- Register Button -->
                    <asp:Button ID="btnRegister" runat="server" Text="Create Account" OnClick="btnRegister_Click"
                        CssClass="w-full bg-indigo-600 text-white py-3.5 rounded-xl hover:bg-indigo-700 transition-all font-medium shadow-md hover:shadow-lg focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" />

                    <!-- Login Link -->
                    <div class="text-center text-sm mt-4">
                        <span class="text-gray-600">Already have an account?</span>
                        <a href="Login.aspx" class="font-medium text-indigo-600 hover:text-indigo-500 ml-1">Sign in</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

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

        function previewRegisterImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = document.getElementById("<%= imgPreview.ClientID %>");
                    img.src = e.target.result;
                    img.classList.remove("opacity-50");
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            hideMessageAfterDelay();
            setupPasswordToggle();
            // ❌ No need to add click event for label anymore
        });
    </script>
</asp:Content>
