<%@ Page Title="Book Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookDetails.aspx.cs" Inherits="SchoolPrj.BookWebsite._BookDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="max-w-6xl mx-auto p-4 sm:p-6 lg:p-8">
        <div class="bg-white shadow-lg rounded-2xl overflow-hidden border border-gray-200">

            <!-- Book Content Wrapper -->
            <div class="flex flex-col lg:flex-row">

                <!-- Book Image -->
                <div class="lg:w-1/2 bg-gray-50 flex items-center justify-center p-6">
                    <img id="imgBook" runat="server"
                        class="max-h-[26rem] object-contain rounded-xl shadow-md transition-transform duration-300 hover:scale-105" />
                </div>

                <!-- Book Details -->
                <div class="lg:w-1/2 p-6 flex flex-col justify-between">

                    <!-- Book Info -->
                    <div>
                        <h1 class="text-3xl md:text-4xl font-bold text-indigo-700 mb-3 leading-snug">
                            <asp:Label ID="lblTitle" runat="server" />
                        </h1>
                        <p class="text-sm text-gray-500 mb-4">
                            📚
                            <asp:Label ID="lblCategory" runat="server" />
                            <span class="mx-2">|</span> ✍️ 
                            <asp:Label ID="lblAuthor" runat="server" />
                        </p>
                        <p class="text-gray-700 leading-relaxed text-base mb-6">
                            <asp:Label ID="lblDescription" runat="server" />
                        </p>
                    </div>

                    <!-- Action Buttons -->
                    <div class="flex flex-wrap gap-4 mt-4">
                        <a id="lnkPdf" runat="server" target="_blank"
                            class="hs-tooltip-toggle px-5 py-3 bg-indigo-600 text-white rounded-xl shadow hover:bg-indigo-700 transition-all text-sm font-medium flex items-center justify-center">📖 Read Book
                        </a>

                        <a id="lnkDownload" runat="server" download
                            class="px-5 py-3 bg-green-500 text-white rounded-xl shadow hover:bg-green-600 transition-all text-sm font-medium flex items-center justify-center">⬇️ Download
                        </a>

                        <asp:Button ID="btnSaveBook" runat="server" Text="💾 Save Book"
                            CssClass="px-5 py-3 bg-blue-500 text-white rounded-xl shadow hover:bg-blue-600 transition-all text-sm font-medium flex items-center justify-center"
                            OnClick="btnSaveBook_Click" />

                        <!-- Heart Section -->
                        <div class="flex items-center gap-1">
                            <asp:Button ID="btnHeart" runat="server" Text="♡"
                                CssClass="heart-btn text-3xl text-gray-400 hover:text-red-500 transition-transform duration-300"
                                OnClick="btnHeart_Click" />
                                <asp:Label ID="lblHeartCount" runat="server" Text="0" CssClass="text-gray-400" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        .heart-btn {
            background: none;
            border: none;
            cursor: pointer;
        }

        .heart-active {
            color: red !important; /* Tailwind Red-500 */
            transform: scale(1);
        }
    </style>

</asp:Content>
