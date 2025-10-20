<%@ Page Title="Edit Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="SchoolPrj.BookWebsite.EditProfile" MaintainScrollPositionOnPostback="true" %>

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

                        <label for="<%= fileUploadProfile.ClientID %>" class="absolute bottom-0 right-0 bg-indigo-600 text-white px-2 py-1 rounded-full shadow-md cursor-pointer hover:bg-indigo-700 transition-all">
                            <i class="fas fa-camera text-sm"></i>
                        </label>

                        <asp:FileUpload ID="fileUploadProfile" runat="server"
                            CssClass="hidden" onchange="previewRegisterImage(this)" />
                    </div>
                    <p class="text-sm text-gray-500 mt-2">Click the camera to upload a profile photo</p>
                </div>

                <!-- Success / Error Message -->
                <asp:Label ID="lblMessage" runat="server" CssClass="mb-4 block text-sm p-3 rounded-lg text-center" Visible="false"></asp:Label>

                <!-- Full Name -->
                <div class="mb-5">
                    <label class="block text-sm font-medium text-gray-700 mb-2">Username</label>
                    <p class="text-sm text-gray-600 mb-4">(Leave blank if you don't want to change Username)</p>
                    <div class="relative">
                        <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                            <i class="fas fa-user text-gray-400"></i>
                        </div>
                        <asp:TextBox ID="txtFullName" runat="server"
                            CssClass="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                            placeholder="Enter your user name" />
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

                <asp:Panel ID="pwchange" runat="server">
                    <!-- Password Change Section -->
                    <div class="mb-5 p-4 bg-gray-50 rounded-xl border border-gray-200">
                        <h3 class="text-lg font-medium text-gray-800 mb-3 flex items-center">
                            <i class="fas fa-key mr-2 text-indigo-600"></i>Change Password
                        </h3>
                        <p class="text-sm text-gray-600 mb-4">Leave blank if you don't want to change password</p>

                        <!-- Old Password -->
                        <asp:Panel ID="Opw" CssClass="mb-4" runat="server">
                            <label class="block text-sm font-medium text-gray-700 mb-2">Old Password <span class="text-red-500">*</span></label>
                            <div class="relative">
                                <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                    <i class="fas fa-lock text-gray-400"></i>
                                </div>
                                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password"
                                    CssClass="w-full pl-10 pr-12 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                    placeholder="Enter your current password" />
                                <!-- Eye icon for password visibility toggle -->
                                <div class="absolute inset-y-0 right-0 pr-3 flex items-center">
                                    <button type="button" id="toggleOldPassword" class="text-gray-400 hover:text-gray-500 focus:outline-none">
                                        <i class="fas fa-eye" id="eyeIconOld"></i>
                                    </button>
                                </div>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvOldPassword" runat="server" ControlToValidate="txtOldPassword"
                                ErrorMessage="*Old password is required to change password" CssClass="text-red-500 text-xs mt-1 block"
                                Display="Dynamic" Enabled="false"></asp:RequiredFieldValidator>
                        </asp:Panel>

                        <!-- New Password -->
                        <asp:Panel ID="Pw" CssClass="mb-4" runat="server">
                            <label class="block text-sm font-medium text-gray-700 mb-2">New Password</label>
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
                            <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword"
                                ErrorMessage="*Password must be at least 6 characters" ValidationExpression=".{6,}"
                                CssClass="text-red-500 text-xs mt-1 block" Display="Dynamic"></asp:RegularExpressionValidator>
                        </asp:Panel>

                        <!-- Confirm New Password -->
                        <asp:Panel ID="ConfirmPw" CssClass="mb-2" runat="server">
                            <label class="block text-sm font-medium text-gray-700 mb-2">Confirm New Password</label>
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
                            <asp:CompareValidator ID="cvPasswords" runat="server" ControlToValidate="txtConfirmPassword"
                                ControlToCompare="txtPassword" Operator="Equal" Type="String"
                                ErrorMessage="*Passwords do not match" CssClass="text-red-500 text-xs mt-1 block" Display="Dynamic"></asp:CompareValidator>
                        </asp:Panel>
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
                    message.style.display = 'none';
                }, 8000); // Hide after 8 seconds
            }
        }

        function setupPasswordToggles() {
            // Old Password Toggle
            const toggleOldPassword = document.getElementById('toggleOldPassword');
            const oldPasswordInput = document.getElementById('<%= txtOldPassword.ClientID %>');
            const eyeIconOld = document.getElementById('eyeIconOld');

            if (toggleOldPassword && oldPasswordInput && eyeIconOld) {
                toggleOldPassword.addEventListener('click', function () {
                    const type = oldPasswordInput.getAttribute('type') === 'password' ? 'text' : 'password';
                    oldPasswordInput.setAttribute('type', type);
                    eyeIconOld.className = type === 'text' ? 'fas fa-eye-slash' : 'fas fa-eye';
                });
            }

            // New Password Toggle
            const togglePassword = document.getElementById('togglePassword');
            const passwordInput = document.getElementById('<%= txtPassword.ClientID %>');
            const eyeIcon = document.getElementById('eyeIcon');

            if (togglePassword && passwordInput && eyeIcon) {
                togglePassword.addEventListener('click', function () {
                    const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
                    passwordInput.setAttribute('type', type);
                    eyeIcon.className = type === 'text' ? 'fas fa-eye-slash' : 'fas fa-eye';
                });
            }

            // Confirm Password Toggle
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

        // Enable old password validation when new password is entered
        function toggleOldPasswordRequirement() {
            const newPassword = document.getElementById('<%= txtPassword.ClientID %>');
            const oldPasswordValidator = document.getElementById('<%= rfvOldPassword.ClientID %>');

            if (newPassword && oldPasswordValidator) {
                newPassword.addEventListener('input', function () {
                    if (this.value.trim() !== '') {
                        oldPasswordValidator.enabled = true;
                    } else {
                        oldPasswordValidator.enabled = false;
                    }
                });
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            hideMessageAfterDelay();
            setupPasswordToggles();
            toggleOldPasswordRequirement();
        });
    </script>
</asp:Content>
