<%@ Page Title="Add Book" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddBook.aspx.cs" Inherits="SchoolPrj.BookWebsite.AddBook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bg-white p-6 rounded-lg shadow-lg max-w-6xl mx-auto">
        <h1 class="text-2xl font-bold text-indigo-600 mb-6">📚 Add New Book</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="block mt-4 text-red-500"></asp:Label>


        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
            <!-- Preview Box -->
            <div class="mt-2">
                <img id="imgPreview" class="w-full h-100 md:h-full object-cover rounded-lg border border-gray-200" />
            </div>
            <!-- Left column -->
            <div class="space-y-4">
                <div>
                    <label class="block text-gray-700 font-semibold mb-1">Book Title</label>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="w-full border border-gray-300 rounded-lg p-2" />
                </div>
                <div>
                    <label class="block text-gray-700 font-semibold mb-1">Category</label>
                    <asp:TextBox ID="txtCategory" runat="server" CssClass="w-full border border-gray-300 rounded-lg p-2" />
                </div>
                <div>
                    <label class="block text-gray-700 font-semibold mb-1">Author</label>
                    <asp:TextBox ID="txtAuthor" runat="server" CssClass="w-full border border-gray-300 rounded-lg p-2" />
                </div>
                <div>
                    <label class="block text-gray-700 font-semibold mb-1">Description</label>
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" CssClass="w-full border border-gray-300 rounded-lg p-2" />
                </div>
            </div>

            <!-- Right column -->
            <div class="space-y-4">
                <div>
                    <label class="block text-gray-700 font-semibold mb-1">Book Image</label>
                    <asp:FileUpload ID="fuImage" runat="server"
                        CssClass="w-full border border-gray-300 rounded-lg p-2 bg-white"
                        onchange="previewImage(event)" />


                </div>
                <div>
                    <label class="block text-gray-700 font-semibold mb-1">PDF File</label>
                    <asp:FileUpload ID="fuPdf" runat="server" CssClass="w-full border border-gray-300 rounded-lg p-2 bg-white" />
                </div>
                <div class="mt-6">
                    <asp:Button ID="btnSave" runat="server" Text="💾 Save Book" OnClick="btnSave_Click"
                        CssClass="bg-indigo-500 text-white px-5 py-2 rounded-lg hover:bg-indigo-600 transition w-full" />
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
                };
                reader.readAsDataURL(file);
            } else {
                preview.src = "";
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
    </script>
</asp:Content>
