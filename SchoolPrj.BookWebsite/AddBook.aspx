<%@ Page Title="Add Book" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddBook.aspx.cs" Inherits="SchoolPrj.BookWebsite.AddBook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Add New Book | MyBookShelf
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="max-w-6xl mx-auto py-4 px-4">
        <!-- Header -->
        <div class="mb-4 text-center">
            <h1 class="text-3xl md:text-4xl font-bold text-indigo-800 mb-2">Add New Book</h1>
        </div>

        <!-- Message Alert -->
        <div class="bg-white rounded-2xl shadow-lg overflow-hidden p-6 md:p-8">
             <asp:Label ID="lblMessage" runat="server" CssClass="bg-red-50 text-red-700 p-4 rounded-xl mb-6 block text-center" Visible="false"></asp:Label>
            <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
                <!-- Preview Box -->
                <div class="lg:col-span-1">
                    <div class="bg-indigo-50 rounded-2xl p-4 h-full flex flex-col">
                        <h3 class="text-lg font-semibold text-indigo-800 mb-4">Book Cover Preview</h3>
                        <div class="flex-1 flex items-center justify-center border-2 border-dashed border-indigo-200 rounded-xl bg-white p-4">
                            <img id="imgPreview" class="w-full h-64 object-contain rounded-lg" 
                                 src="data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 24 24' stroke='%239C92AC'%3E%3Cpath stroke-linecap='round' stroke-linejoin='round' stroke-width='1' d='M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z'%3E%3C/path%3E%3C/svg%3E" 
                                 alt="Book cover preview" />
                        </div>
                        <p class="text-sm text-gray-500 mt-3 text-center">Image will appear here after selection</p>
                    </div>
                </div>

                <!-- Form Columns -->
                <div class="lg:col-span-2">
                    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <!-- Left Column -->
                        <div class="space-y-5">
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-2">Book Title <span class="text-red-500">*</span></label>
                                <asp:TextBox ID="txtTitle" runat="server" 
                                    CssClass="w-full px-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all" 
                                    placeholder="Enter book title" />
                            </div>
                            
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-2">Category <span class="text-red-500">*</span></label>
                                <asp:TextBox ID="txtCategory" runat="server" 
                                    CssClass="w-full px-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all" 
                                    placeholder="e.g., Fiction, Science, History" />
                            </div>
                            
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-2">Author <span class="text-red-500">*</span></label>
                                <asp:TextBox ID="txtAuthor" runat="server" 
                                    CssClass="w-full px-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all" 
                                    placeholder="Author's full name" />
                            </div>
                        </div>

                        <!-- Right Column -->
                        <div class="space-y-5">
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-2">Book Cover Image <span class="text-red-500">*</span></label>
                                <div class="relative">
                                    <asp:FileUpload ID="fuImage" runat="server"
                                        CssClass="w-full px-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-indigo-50 file:text-indigo-700 hover:file:bg-indigo-100"
                                        onchange="previewImage(event)" />
                                </div>
                            </div>
                            
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-2">PDF File</label>
                                <asp:FileUpload ID="fuPdf" runat="server" 
                                    CssClass="w-full px-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-indigo-50 file:text-indigo-700 hover:file:bg-indigo-100" />
                            </div>
                        </div>
                    </div>

                    <!-- Description Field -->
                    <div class="mt-6">
                        <label class="block text-sm font-medium text-gray-700 mb-2">Description</label>
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" 
                            CssClass="w-full px-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all" 
                            placeholder="Enter a brief description of the book..." />
                    </div>

                    <!-- Submit Button -->
                    <div class="mt-8 flex justify-end">
                        <asp:Button ID="btnSave" runat="server" Text="Add Book" OnClick="btnSave_Click"
                            CssClass="px-8 py-3.5 bg-indigo-600 text-white rounded-xl hover:bg-indigo-700 transition-all font-medium shadow-md hover:shadow-lg flex items-center" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function previewImage(event) {
            var file = event.target.files[0];
            var preview = document.getElementById("imgPreview");

            if (file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    preview.src = e.target.result;
                    preview.classList.remove("opacity-50");
                };
                reader.readAsDataURL(file);
            } else {
                preview.src = "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 24 24' stroke='%239C92AC'%3E%3Cpath stroke-linecap='round' stroke-linejoin='round' stroke-width='1' d='M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z'%3E%3C/path%3E%3C/svg%3E";
                preview.classList.add("opacity-50");
            }
        }

        function hideMessageAfterDelay() {
            setTimeout(function () {
                var msg = document.getElementById('<%= lblMessage.ClientID %>');
                if (msg) {
                    msg.style.display = 'none';
                }
            }, 5000);
        }

        // Initialize on page load
        document.addEventListener('DOMContentLoaded', function () {
            hideMessageAfterDelay();

            // Set initial preview image
            var preview = document.getElementById("imgPreview");
            if (preview && !preview.src) {
                preview.src = "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 24 24' stroke='%239C92AC'%3E%3Cpath stroke-linecap='round' stroke-linejoin='round' stroke-width='1' d='M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z'%3E%3C/path%3E%3C/svg%3E";
                preview.classList.add("opacity-50");
            }
        });
    </script>
</asp:Content>