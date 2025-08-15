<%@ Page Title="Book Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookDetails.aspx.cs" Inherits="SchoolPrj.BookWebsite._BookDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="max-w-5xl mx-auto bg-white p-6 flex flex-col lg:flex-row gap-2">
        <!-- Book Image -->
        <div class="lg:w-1/2 flex-shrink-0">
            <img id="imgBook" runat="server" class="w-full h-96 rounded-sm object-contain" />
        </div>

        <!-- Book Details -->
        <div class="lg:w-1/2 flex flex-col justify-between">
            <div>
                <h1 class="text-3xl font-bold text-indigo-600 mb-4">
                    <asp:Label ID="lblTitle" runat="server" /></h1>
                <p class="text-sm text-gray-500 mb-2">
                    📚
                    <asp:Label ID="lblCategory" runat="server" />
                    | ✍️
                    <asp:Label ID="lblAuthor" runat="server" />
                </p>
                <p class="text-gray-700 mb-2">
                    <asp:Label ID="lblDescription" runat="server" /></p>
            </div>

            <div class="flex flex-col sm:flex-row gap-4">
                <a id="lnkPdf" runat="server" target="_blank"
                    class="inline-block py-2 px-5 bg-indigo-500 text-white rounded-lg hover:bg-indigo-600 transition text-center">📖 Read Book
                </a>

                <a id="lnkDownload" runat="server"
                    class="inline-block py-2 px-5 bg-green-500 text-white rounded-lg hover:bg-green-600 transition text-center"
                    download>⬇️ Download
                </a>
            </div>
        </div>
    </div>
</asp:Content>
