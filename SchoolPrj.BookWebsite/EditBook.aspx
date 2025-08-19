<%@ Page Title="Edit Book" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditBook.aspx.cs" Inherits="SchoolPrj.BookWebsite.EditBook" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bg-white p-6 rounded-lg shadow-lg max-w-6xl mx-auto">
        <h1 class="text-2xl font-bold text-indigo-600 mb-6">📚 Edit Book</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="block mt-4 text-red-500"></asp:Label>

        <asp:HiddenField ID="hfBookId" runat="server" />

        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
            <!-- Preview Box -->
            <div class="mt-2">
                <asp:Image ID="imgPreview" runat="server" CssClass="w-full h-100 md:h-full object-cover rounded-lg border border-gray-200" />
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
                        onchange="previewImage(this)" />


                </div>


                <div class="mb-4">
                    <label class="block text-gray-700 font-medium">PDF File</label>
                    <asp:FileUpload ID="fuPdf" runat="server" CssClass="w-full border border-gray-300 rounded-lg p-2 bg-white" />
                    <asp:HyperLink ID="lnkPdf" runat="server" Target="_blank" CssClass="block mt-2 text-indigo-600 underline">View Current PDF</asp:HyperLink>
                </div>


                <asp:Button ID="btnUpdate" runat="server" Text="💾 Update Book"
                    CssClass="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition"
                    OnClick="btnUpdate_Click" />

            </div>
        </div>
    </div>

    <script type="text/javascript">
        function previewImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    var img = document.getElementById("<%= imgPreview.ClientID %>");
                    img.src = e.target.result;
                };

                reader.readAsDataURL(input.files[0]);
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
