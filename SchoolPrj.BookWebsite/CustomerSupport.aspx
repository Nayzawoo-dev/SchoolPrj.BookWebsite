<%@ Page Title="Customer Support" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerSupport.aspx.cs" Inherits="SchoolPrj.BookWebsite.CustomerSupport" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Customer Support | MyBookShelf
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="min-h-screen bg-gradient-to-br from-indigo-50 to-purple-100 py-8 px-4">
        <div class="max-w-4xl mx-auto">
            <!-- Header Section -->
            <div class="text-center mb-8">
                <h1 class="text-4xl font-bold text-gray-800 mb-4">Customer Support</h1>
                <p class="text-lg text-gray-600 max-w-2xl mx-auto">
                    We're here to help! Contact our support team for any issues with your account, 
                    technical problems, or general inquiries about MyBookShelf.
                </p>
            </div>

            <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
                <!-- Contact Information -->
                <div class="lg:col-span-1">
                    <div class="bg-white rounded-2xl shadow-lg p-6 sticky top-6">
                        <h2 class="text-2xl font-bold text-gray-800 mb-6 flex items-center">
                            <i class="fas fa-headset mr-3 text-indigo-600"></i>Get in Touch
                        </h2>
                        
                        <!-- Support Methods -->
                        <div class="space-y-4">
                            <!-- Email -->
                            <div class="flex items-start">
                                <div class="bg-indigo-100 p-3 rounded-lg mr-4">
                                    <i class="fas fa-envelope text-indigo-600 text-lg"></i>
                                </div>
                                <div>
                                    <h3 class="font-semibold text-gray-800">Email Support</h3>
                                    <p class="text-gray-600 text-sm">nayzawoo.ace@gmail.com</p>
                                    <p class="text-gray-500 text-xs mt-1">Typically replies within 24 hours</p>
                                </div>
                            </div>

                            <!-- Response Time -->
                            <div class="flex items-start">
                                <div class="bg-green-100 p-3 rounded-lg mr-4">
                                    <i class="fas fa-clock text-green-600 text-lg"></i>
                                </div>
                                <div>
                                    <h3 class="font-semibold text-gray-800">Response Time</h3>
                                    <p class="text-gray-600 text-sm">24-48 hours</p>
                                    <p class="text-gray-500 text-xs mt-1">During business days</p>
                                </div>
                            </div>

                            <!-- Common Issues -->
                            <div class="flex items-start">
                                <div class="bg-blue-100 p-3 rounded-lg mr-4">
                                    <i class="fas fa-lightbulb text-blue-600 text-lg"></i>
                                </div>
                                <div>
                                    <h3 class="font-semibold text-gray-800">Quick Help</h3>
                                    <p class="text-gray-600 text-sm">Check our FAQ section</p>
                                    <asp:HyperLink ID="lnkFAQ" runat="server" NavigateUrl="~/FAQ.aspx" 
                                        CssClass="text-indigo-600 hover:text-indigo-500 text-xs">View FAQs</asp:HyperLink>
                                </div>
                            </div>
                        </div>

                        <!-- Support Hours -->
                        <div class="mt-6 p-4 bg-gray-50 rounded-lg">
                            <h4 class="font-semibold text-gray-800 mb-2">Support Hours</h4>
                            <div class="text-sm text-gray-600 space-y-1">
                                <div class="flex justify-between">
                                    <span>Monday - Friday:</span>
                                    <span class="font-medium">9:00 AM - 6:00 PM</span>
                                </div>
                                <div class="flex justify-between">
                                    <span>Saturday:</span>
                                    <span class="font-medium">10:00 AM - 4:00 PM</span>
                                </div>
                                <div class="flex justify-between">
                                    <span>Sunday:</span>
                                    <span class="font-medium">Closed</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Contact Form -->
                <div class="lg:col-span-2">
                    <div class="bg-white rounded-2xl shadow-lg p-8">
                        <h2 class="text-2xl font-bold text-gray-800 mb-6">Send us a Message</h2>

                        <!-- Success/Error Message -->
                        <asp:Label ID="lblMessage" runat="server" CssClass="mb-6 block text-sm p-4 rounded-lg text-center" Visible="false"></asp:Label>

                        <div class="space-y-6">
                            <!-- User Info (Auto-filled if logged in) -->
                            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                                <!-- Full Name -->
                                <div>
                                    <label class="block text-sm font-medium text-gray-700 mb-2">Full Name <span class="text-red-500">*</span></label>
                                    <div class="relative">
                                        <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                            <i class="fas fa-user text-gray-400"></i>
                                        </div>
                                        <asp:TextBox ID="txtFullName" runat="server"
                                            CssClass="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                            placeholder="Enter your full name" />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                                        ErrorMessage="*Full name is required" CssClass="text-red-500 text-xs mt-1 block" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>

                                <!-- Email -->
                                <div>
                                    <label class="block text-sm font-medium text-gray-700 mb-2">Email Address <span class="text-red-500">*</span></label>
                                    <div class="relative">
                                        <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                            <i class="fas fa-envelope text-gray-400"></i>
                                        </div>
                                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email"
                                            CssClass="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                            placeholder="Enter your email address" />
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="*Email is required" CssClass="text-red-500 text-xs mt-1 block" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="*Please enter a valid email address" ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                                        CssClass="text-red-500 text-xs mt-1 block" Display="Dynamic"></asp:RegularExpressionValidator>
                                </div>
                            </div>

                            <!-- Issue Type -->
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-2">Issue Type <span class="text-red-500">*</span></label>
                                <div class="relative">
                                    <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                        <i class="fas fa-tag text-gray-400"></i>
                                    </div>
                                    <asp:DropDownList ID="ddlIssueType" runat="server"
                                        CssClass="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all appearance-none bg-white">
                                        <asp:ListItem Value="" Text="Select an issue type" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="Account Issues" Text="Account Issues"></asp:ListItem>
                                        <asp:ListItem Value="Login Problems" Text="Login Problems"></asp:ListItem>
                                        <asp:ListItem Value="Password Reset" Text="Password Reset"></asp:ListItem>
                                        <asp:ListItem Value="Technical Issues" Text="Technical Issues"></asp:ListItem>
                                        <asp:ListItem Value="Book Related" Text="Book Related Issues"></asp:ListItem>
                                        <asp:ListItem Value="Payment Issues" Text="Payment Issues"></asp:ListItem>
                                        <asp:ListItem Value="Feature Request" Text="Feature Request"></asp:ListItem>
                                        <asp:ListItem Value="Bug Report" Text="Bug Report"></asp:ListItem>
                                        <asp:ListItem Value="General Inquiry" Text="General Inquiry"></asp:ListItem>
                                        <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvIssueType" runat="server" ControlToValidate="ddlIssueType" InitialValue=""
                                    ErrorMessage="*Please select an issue type" CssClass="text-red-500 text-xs mt-1 block" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>

                            <!-- Subject -->
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-2">Subject <span class="text-red-500">*</span></label>
                                <div class="relative">
                                    <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                        <i class="fas fa-pen text-gray-400"></i>
                                    </div>
                                    <asp:TextBox ID="txtSubject" runat="server"
                                        CssClass="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
                                        placeholder="Brief description of your issue" />
                                </div>
                                <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject"
                                        ErrorMessage="*Subject is required" CssClass="text-red-500 text-xs mt-1 block" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>

                            <!-- Message -->
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-2">Message <span class="text-red-500">*</span></label>
                                <div class="relative">
                                    <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="8"
                                        CssClass="w-full px-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all resize-none"
                                        placeholder="Please describe your issue in detail. Include any error messages, steps to reproduce, and what you were trying to accomplish." />
                                </div>
                                <div class="flex justify-between items-center mt-2">
                                    <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage"
                                        ErrorMessage="*Message is required" CssClass="text-red-500 text-xs" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <span id="charCount" class="text-xs text-gray-500">0/2000 characters</span>
                                </div>
                            </div>

                            <!-- Priority (Optional) -->
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-2">Priority</label>
                                <div class="flex space-x-4">
                                    <label class="flex items-center">
                                        <asp:RadioButton ID="rbLow" runat="server" GroupName="Priority" Checked="true" 
                                            CssClass="text-indigo-600 focus:ring-indigo-500" />
                                        <span class="ml-2 text-gray-700">Low</span>
                                    </label>
                                    <label class="flex items-center">
                                        <asp:RadioButton ID="rbMedium" runat="server" GroupName="Priority" 
                                            CssClass="text-indigo-600 focus:ring-indigo-500" />
                                        <span class="ml-2 text-gray-700">Medium</span>
                                    </label>
                                    <label class="flex items-center">
                                        <asp:RadioButton ID="rbHigh" runat="server" GroupName="Priority" 
                                            CssClass="text-indigo-600 focus:ring-indigo-500" />
                                        <span class="ml-2 text-gray-700">High</span>
                                    </label>
                                    <label class="flex items-center">
                                        <asp:RadioButton ID="rbUrgent" runat="server" GroupName="Priority" 
                                            CssClass="text-indigo-600 focus:ring-indigo-500" />
                                        <span class="ml-2 text-gray-700">Urgent</span>
                                    </label>
                                </div>
                            </div>

                            <!-- Submit Button -->
                            <div class="flex justify-between items-center pt-4">
                                <div class="text-sm text-gray-600">
                                    <i class="fas fa-info-circle mr-1 text-indigo-600"></i>
                                    We'll respond to your email within 24-48 hours.
                                </div>
                                <asp:Button ID="btnSubmit" runat="server" Text="Send Message" OnClick="btnSubmit_Click"
                                    CssClass="px-8 py-3 bg-indigo-600 text-white rounded-xl hover:bg-indigo-700 transition-all font-medium shadow-md hover:shadow-lg focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 flex items-center" />
                            </div>
                        </div>
                    </div>

                    <!-- FAQ Quick Links -->
                    <div class="mt-8 bg-white rounded-2xl shadow-lg p-6">
                        <h3 class="text-xl font-bold text-gray-800 mb-4">Frequently Asked Questions</h3>
                        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <div class="p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors">
                                <h4 class="font-semibold text-gray-800 mb-2">How do I reset my password?</h4>
                                <p class="text-sm text-gray-600">Go to Login page and click "Forgot Password" to reset your password via email.</p>
                            </div>
                            <div class="p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors">
                                <h4 class="font-semibold text-gray-800 mb-2">Why can't I login?</h4>
                                <p class="text-sm text-gray-600">Make sure your email is verified and you're using the correct password.</p>
                            </div>
                            <div class="p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors">
                                <h4 class="font-semibold text-gray-800 mb-2">How to contact support faster?</h4>
                                <p class="text-sm text-gray-600">Include your username and detailed description for faster resolution.</p>
                            </div>
                            <div class="p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors">
                                <h4 class="font-semibold text-gray-800 mb-2">Account recovery</h4>
                                <p class="text-sm text-gray-600">We can help recover accounts if you provide sufficient verification.</p>
                            </div>
                        </div>
                    </div>
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
                }, 10000); // Hide after 10 seconds
            }
        }

        // Character count for message
        function setupCharacterCount() {
            const messageInput = document.getElementById('<%= txtMessage.ClientID %>');
            const charCount = document.getElementById('charCount');

            if (messageInput && charCount) {
                messageInput.addEventListener('input', function () {
                    const length = this.value.length;
                    charCount.textContent = length + '/2000 characters';
                    
                    if (length > 1900) {
                        charCount.className = 'text-xs text-red-500';
                    } else if (length > 1500) {
                        charCount.className = 'text-xs text-yellow-500';
                    } else {
                        charCount.className = 'text-xs text-gray-500';
                    }
                });

                // Initialize count
                charCount.textContent = messageInput.value.length + '/2000 characters';
            }
        }

        // Auto-fill for logged-in users
        function autoFillUserInfo() {
            const fullName = document.getElementById('<%= txtFullName.ClientID %>');
            const email = document.getElementById('<%= txtEmail.ClientID %>');
            
            // If fields are empty and user is logged in, we'll let server-side handle auto-fill
            // This is just for visual enhancement
        }

        document.addEventListener('DOMContentLoaded', function () {
            hideMessageAfterDelay();
            setupCharacterCount();
            autoFillUserInfo();
        });
    </script>
</asp:Content>