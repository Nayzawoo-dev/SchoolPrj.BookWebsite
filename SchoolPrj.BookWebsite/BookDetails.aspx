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

        <!-- Comment Section -->
        <div class="mt-8 bg-white shadow rounded-xl p-6">
            <h2 class="text-xl font-bold mb-4">💬 Comments</h2>

            <!-- Comment Form -->
            <asp:Panel ID="pnlAddComment" runat="server" Visible="true">
                <asp:TextBox ID="txtComment" runat="server"
                    CssClass="w-full p-3 border rounded-lg mb-3"
                    TextMode="MultiLine" Rows="3" placeholder="Write a comment..."></asp:TextBox>
                <asp:FileUpload ID="fuVideo" runat="server" CssClass="mb-3" />
                <asp:Button ID="btnAddComment" runat="server" Text="Post Comment"
                    CssClass="bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600"
                    OnClick="btnAddComment_Click" />
            </asp:Panel>
        </div>

        <!-- Comments List -->
        <asp:Repeater ID="rptComments" runat="server">
            <ItemTemplate>
                <div class="flex items-start gap-3 border-b border-gray-200 p-4 bg-white mt-4 rounded-xl">
                    <img src='<%# ResolveUrl(Eval("UserProfile").ToString()) %>'
                        class="w-10 h-10 rounded-full object-cover" />
                    <div>
                        <span class="px-1 py-1 text-sm bg-indigo-100 text-indigo-700 rounded-full">
                            <%# Eval("Role") %>
                        </span>
                        <p class="font-bold text-gray-800"><%# Eval("UserName") %></p>
                        <p class="text-sm text-gray-400 mt-1"><%# Eval("Date", "{0:yyyy-MM-dd}") %></p>
                        <p class="text-gray-700"><%# Eval("Comment") %></p>
                        <asp:PlaceHolder ID="phVideo" runat="server"
                            Visible='<%# Eval("VideoPath") != DBNull.Value && Eval("VideoPath") != null %>'>
                            <video src='<%# Eval("VideoPath") == null ? "#" : ResolveUrl(Eval("VideoPath").ToString()) %>' controls class="mt-2 w-60 rounded border border-gray-200"></video>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phImage" runat="server"
                            Visible='<%# Eval("ImagePath") != DBNull.Value && Eval("ImagePath") != null %>'>
                            <img src='<%# Eval("ImagePath") == null ? "#" : ResolveUrl(Eval("ImagePath").ToString()) %>' class="mt-2 w-60 rounded border border-gray-200">
                        </asp:PlaceHolder>
                        <p class="text-sm text-gray-400 mt-1"><%# Eval("Date", "{0:hhhh}") %>hr ago</p>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

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
