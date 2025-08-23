<%@ Page Title="Edit Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="SchoolPrj.BookWebsite.EditProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit Profile | MyBookShelf
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="min-h-screen flex items-center justify-center bg-gradient-to-br from-indigo-50 to-purple-100 py-8 px-4">
        <div class="max-w-md w-full bg-white rounded-2xl shadow-xl overflow-hidden">
            <!-- Header Section -->
            <div class="bg-gradient-to-r from-indigo-600 to-purple-700 py-6 px-8 text-center">
                <h1 class="text-3xl font-bold text-white mb-2">Edit Profile</h1>
                <p class="text-indigo-100">Update your personal information</p>
            </div>

            <div class="py-8 px-8">


                <!-- Profile Image Upload -->
                <div class="mb-6 text-center">
                    <div class="relative inline-block">
                        <asp:Image ID="imgProfile" runat="server"
                            CssClass="w-24 h-24 rounded-full border-4 border-white shadow-lg object-cover mx-auto"
                            ImageUrl="data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 24 24' stroke='%239C92AC'%3E%3Cpath stroke-linecap='round' stroke-linejoin='round' stroke-width='1' d='M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z'%3E%3C/path%3E%3C/svg%3E" />

                        <!-- label အကြောင့် FileInput ကို click လုပ်လို့ရပြီ၊ JS click မလိုတော့ -->
                        <label for="<%= fileUploadProfile.ClientID %>" class="absolute bottom-0 right-0 bg-indigo-600 text-white p-2 rounded-full shadow-md cursor-pointer hover:bg-indigo-700 transition-all">
                            <i class="fas fa-camera text-sm"></i>
                        </label>

                        <asp:FileUpload ID="fileUploadProfile" runat="server"
                            CssClass="hidden" onchange="previewRegisterImage(this)" />
                    </div>
                    <p class="text-sm text-gray-500 mt-2">Click the camera to upload a profile photo</p>
                </div>

                <!-- Success / Error Message -->
                <asp:Label ID="lblMessage" runat="server" CssClass="bg-red-50 text-red-700 p-4 rounded-xl mb-6 block text-center" Visible="false"></asp:Label>
                <!-- Full Name -->
                <div class="mb-5">
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

                <!-- Email -->
                <div class="mb-5">
                    <label class="block text-sm font-medium text-gray-700 mb-2">Email</label>
                    <div class="relative">
                        <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                            <i class="fas fa-envelope text-gray-400"></i>
                        </div>
                        <asp:TextBox ID="txtEmail" ReadOnly runat="server"
                            CssClass="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl bg-gray-100 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                            placeholder="Your email address" />
                    </div>
                </div>

                <!-- Password (Optional) -->
                <asp:Panel ID="Pw" CssClass="mb-5" runat="server">
                    <label class="block text-sm font-medium text-gray-700 mb-2">New Password (leave blank if not changing)</label>
                    <div class="relative">
                        <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                            <i class="fas fa-lock text-gray-400"></i>
                        </div>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                            CssClass="w-full pl-10 pr-12 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                            placeholder="Enter new password" />
                        <!-- Eye icon for password visibility toggle -->
                        <div class="absolute inset-y-0 right-0 pr-3 flex items-center">
                            <button type="button" id="togglePassword" class="text-gray-400 hover:text-gray-500 focus:outline-none">
                                <i class="fas fa-eye" id="eyeIcon"></i>
                            </button>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Save Button -->
                <div class="flex flex-col sm:flex-row justify-between gap-3 mt-8">
                    <asp:HyperLink ID="btnCancel" runat="server" NavigateUrl="~/Profile.aspx"
                        CssClass="px-6 py-3 bg-gray-300 text-gray-700 rounded-xl hover:bg-gray-400 transition-all font-medium text-center order-2 sm:order-1">
                        <i class="fas fa-times mr-2"></i> Cancel
                    </asp:HyperLink>
                    <asp:Button ID="btnSave" runat="server" Text="Save Changes" OnClick="btnSave_Click"
                        CssClass="px-6 py-3 bg-indigo-600 text-white rounded-xl hover:bg-indigo-700 transition-all font-medium shadow-md hover:shadow-lg order-1 sm:order-2" />
                </div>
            </div>
        </div>
    </div>

    <!-- JavaScript -->
    <script type="text/javascript">
        function hideMessageAfterDelay() {
            const message = document.getElementById('<%= lblMessage.ClientID %>');
            if (message && message.innerText.trim() !== '') {
                setTimeout(function () {
                    message.classList.add('hidden');
                }, 5000); // Hide after 5 seconds
            }
        }
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
                    var img = document.getElementById("<%= imgProfile.ClientID %>");
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
