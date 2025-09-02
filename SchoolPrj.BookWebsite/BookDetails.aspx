<%@ Page Title="Category" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookDetails.aspx.cs" Inherits="SchoolPrj.BookWebsite._BookDetails" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <div class="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-6 md:py-8">

        <!-- Book Card Section UpdatePanel -->
        <asp:UpdatePanel ID="upBookCard" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- Book Card Section -->
                <div class="bg-white rounded-2xl shadow-md overflow-hidden mb-8">
                    <div class="flex flex-col md:flex-row">
                        <!-- Book Image -->
                        <div class="md:w-2/5 bg-gradient-to-br from-blue-50 to-purple-50 flex items-center justify-center p-4 md:p-6">
                            <img id="imgBook" runat="server"
                                class="max-h-64 md:max-h-80 w-auto object-contain rounded-lg shadow-md transition-transform duration-300 hover:scale-105" />
                        </div>

                        <!-- Book Details -->
                        <div class="md:w-3/5 p-4 md:p-6">
                            <div class="mb-4 md:mb-6">
                                <h1 class="text-xl sm:text-2xl md:text-3xl font-bold text-gray-900 mb-2">
                                    <asp:Label ID="lblTitle" runat="server" />
                                </h1>

                                <div class="flex flex-wrap items-center gap-2 text-xs sm:text-sm text-gray-600 mb-3 md:mb-4">
                                    <span class="inline-flex items-center gap-1 bg-blue-100 px-2 py-1 rounded-full">
                                        <svg class="w-3 h-3 md:w-4 md:h-4" fill="currentColor" viewBox="0 0 20 20">
                                            <path d="M7 3a1 1 0 000 2h6a1 1 0 100-2H7zM4 7a1 1 0 011-1h10a1 1 0 110 2H5a1 1 0 01-1-1zM2 11a2 2 0 012-2h12a2 2 0 012 2v4a2 2 0 01-2 2H4a2 2 0 01-2-2v-4z" />
                                        </svg>
                                        <asp:Label ID="lblCategory" runat="server" />
                                    </span>
                                    <span class="inline-flex items-center gap-1 bg-green-100 px-2 py-1 rounded-full">
                                        <svg class="w-3 h-3 md:w-4 md:h-4" fill="currentColor" viewBox="0 0 20 20">
                                            <path fill-rule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clip-rule="evenodd" />
                                        </svg>
                                        <asp:Label ID="lblAuthor" runat="server" />
                                    </span>
                                </div>

                                <div class="bg-gray-50 p-3 md:p-4 rounded-lg mb-4 md:mb-6">
                                    <h3 class="text-xs md:text-sm font-semibold text-gray-700 mb-2">Description</h3>
                                    <p class="text-gray-600 leading-relaxed text-xs sm:text-sm md:text-base">
                                        <asp:Label ID="lblDescription" runat="server" />
                                    </p>
                                </div>
                            </div>

                            <!-- Action Buttons - Smart Mobile Layout -->
                            <div class="flex flex-row sm:flex-row sm:items-center justify-between gap-3">
                                <!-- Heart Button - Always visible -->
                                <div class="flex items-center">
                                    <asp:Button ID="btnHeart" runat="server" Text="♡"
                                        CssClass="heart-btn text-2xl text-gray-400 hover:text-red-500 transition-all duration-300 focus:outline-none"
                                        OnClick="btnHeart_Click" />
                                    <asp:Label ID="lblHeartCount" runat="server" Text="0" CssClass="text-gray-600 ml-1" />
                                </div>

                                <!-- Action Buttons - Compact on mobile -->
                                <div class="flex flex-row justify-end gap-2">
                                    <a id="lnkPdf" runat="server" target="_blank"
                                        class="inline-flex items-center px-3 py-2 bg-blue-600 hover:bg-blue-700 text-white text-sm font-medium rounded-lg transition-colors shadow-md flex-shrink-0">
                                        <svg class="w-4 h-4 mr-1 md:mr-2" fill="currentColor" viewBox="0 0 20 20">
                                            <path fill-rule="evenodd" d="M4 4a2 2 0 012-2h4.586A2 2 0 0112 2.586L15.414 6A2 2 0 0116 7.414V16a2 2 0 01-2 2H6a2 2 0 01-2-2V4zm2 6a1 1 0 011-1h6a1 1 0 110 2H7a1 1 0 01-1-1zm1 3a1 1 0 100 2h6a1 1 0 100-2H7z" clip-rule="evenodd" />
                                        </svg>
                                        <span class="hidden sm:inline">Read</span>
                                    </a>

                                    <a id="lnkDownload" runat="server" download
                                        class="inline-flex items-center px-3 py-2 bg-gray-800 hover:bg-gray-900 text-white text-sm font-medium rounded-lg transition-colors shadow-md flex-shrink-0">
                                        <svg class="w-4 h-4 mr-1 md:mr-2" fill="currentColor" viewBox="0 0 20 20">
                                            <path fill-rule="evenodd" d="M3 17a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm3.293-7.707a1 1 0 011.414 0L9 10.586V3a1 1 0 112 0v7.586l1.293-1.293a1 1 0 111.414 1.414l-3 3a1 1 0 01-1.414 0l-3-3a1 1 0 010-1.414z" clip-rule="evenodd" />
                                        </svg>
                                        <span class="hidden sm:inline">Download</span>
                                    </a>

                                    <asp:Button ID="btnSaveBook" runat="server" Text="Save"
                                        CssClass="inline-flex items-center px-3 py-2 bg-green-600 hover:bg-green-700 text-white text-sm font-medium rounded-lg transition-colors shadow-md cursor-pointer flex-shrink-0"
                                        OnClick="btnSaveBook_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Comment Section UpdatePanel -->
        <asp:UpdatePanel ID="upComments" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- Comment Section -->
                <div class="mt-10">
                    <h2 class="text-xl sm:text-2xl font-bold text-gray-800 mb-6 pb-2 border-b border-gray-200 flex items-center">
                        <svg class="w-5 h-5 mr-2 text-gray-600" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                            <path d="M14 1a1 1 0 0 1 1 1v8a1 1 0 0 1-1 1h-2.5a2 2 0 0 0-1.6.8L8 14.333 6.1 11.8a2 2 0 0 0-1.6-.8H2a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h12zM2 0a2 2 0 0 0-2 2v8a2 2 0 0 0 2 2h2.5a1 1 0 0 1 .8.4l1.9 2.533a1 1 0 0 0 1.6 0l1.9-2.533a1 1 0 0 1 .8-.4H14a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2z" />
                            <path d="M3 3.5a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9a.5.5 0 0 1-.5-.5zM3 6a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9A.5.5 0 0 1 3 6zm0 2.5a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1h-5a.5.5 0 0 1-.5-.5z" />
                        </svg>
                        Comments
                        <span class="ml-2 bg-blue-100 text-blue-800 text-sm font-medium px-2.5 py-0.5 rounded-full">
                            <asp:Label ID="lblCommentCount" runat="server" />
                        </span>
                    </h2>

                    <!-- Comment Form -->
                    <asp:Panel ID="pnlAddComment" runat="server" Visible="true" CssClass="bg-white rounded-xl shadow-sm p-5 mb-8 border border-gray-200">
                        <h3 class="text-lg font-semibold text-gray-800 mb-4">Add your comment</h3>
                        <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="4"
                            CssClass="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                            placeholder="Share your thoughts about this book..."></asp:TextBox>

                        <div class="mt-4 flex flex-col sm:flex-row gap-4 items-start sm:items-center">
                            <div class="relative flex-grow">
                                <asp:FileUpload ID="fuVideo" runat="server"
                                    CssClass="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100" />
                                <p class="mt-1 text-xs text-gray-500">Upload a video (optional)</p>
                            </div>

                            <asp:Button ID="btnAddComment" runat="server" Text="Post Comment" OnClick="btnAddComment_Click"
                                CssClass="px-5 py-2.5 bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-lg text-sm transition-colors duration-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 whitespace-nowrap" />
                        </div>
                    </asp:Panel>

                    <!-- No Comments -->
                    <asp:Panel ID="pnlNoComments" runat="server" Visible="false" CssClass="text-center py-10 px-4 bg-gray-50 rounded-xl border border-gray-200">
                        <svg class="w-16 h-16 mx-auto text-gray-400 mb-4" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                            <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z" />
                            <path d="M7 5.5C7 5.224 7.224 5 7.5 5h1a.5.5 0 0 1 0 1h-1A.5.5 0 0 1 7 5.5zm0 3c0 .276.224.5.5.5h1a.5.5 0 0 0 0-1h-1a.5.5 0 0 0-.5.5zM6.5 10a.5.5 0 0 0 0 1h3a.5.5 0 0 0 0-1h-3z" />
                        </svg>
                        <h3 class="text-xl font-semibold text-gray-700 mb-2">No comments yet</h3>
                        <p class="text-gray-500 max-w-md mx-auto">Be the first to share your thoughts about this book! Your insights could help other readers.</p>
                    </asp:Panel>
                </div>

                <!-- Comments List -->
                <asp:Repeater ID="rptComments" runat="server" OnItemCommand="rptComments_ItemCommand" OnItemDataBound="rptComments_ItemDataBound">
                    <ItemTemplate>
                        <div class="flex items-start gap-4 p-5 bg-white rounded-xl border border-gray-200 mt-4 shadow-sm">
                            <img src='<%# ResolveUrl(Eval("UserProfile").ToString()) %>'
                                class="w-10 h-10 rounded-full object-cover border-2 border-white shadow"
                                alt="User profile" />
                            <div class="flex-1 min-w-0">
                                <div class="flex flex-wrap items-center gap-2 mb-2">
                                    <span class="px-2 py-1 text-xs bg-indigo-100 text-indigo-800 rounded-full font-medium">
                                        <%# Eval("Role") %>
                                    </span>
                                    <p class="font-semibold text-gray-800 text-sm"><%# Eval("UserName") %></p>
                                    <span class="text-xs text-gray-500">•</span>
                                    <p class="text-xs text-gray-500"><%# Eval("Date", "{0:MMM dd, yyyy}") %></p>
                                </div>

                                <p class="text-gray-700 mb-3 text-sm"><%# Eval("Comment") %></p>

                                <asp:PlaceHolder ID="phVideo" runat="server"
                                    Visible='<%# Eval("VideoPath") != DBNull.Value && Eval("VideoPath") != null %>'>
                                    <div class="mt-3 mb-3">
                                        <video src='<%# Eval("VideoPath") == null ? "#" : ResolveUrl(Eval("VideoPath").ToString()) %>'
                                            controls class="max-w-2xs md:max-w-xs rounded-lg border border-gray-200 shadow-sm">
                                        </video>
                                    </div>
                                </asp:PlaceHolder>

                                <asp:PlaceHolder ID="phImage" runat="server"
                                    Visible='<%# Eval("ImagePath") != DBNull.Value && Eval("ImagePath") != null %>'>
                                    <div class="mt-3 mb-3">
                                        <img src='<%# Eval("ImagePath") == null ? "#" : ResolveUrl(Eval("ImagePath").ToString()) %>'
                                            class="max-w-2xs md:max-w-xs rounded-lg border border-gray-200 shadow-sm" alt="Comment image">
                                    </div>
                                </asp:PlaceHolder>

                                <div class="flex items-center gap-3 mt-3 text-xs text-gray-500">
                                    <span><%# GetRelativeTime(Convert.ToDateTime(Eval("Date"))) %></span>
                                    <button type="button" class="text-blue-600 hover:text-blue-800 font-medium reply-trigger">Reply</button>

                                    <asp:Button ID="btnCommentDelete" runat="server" Text="Delete"
                                        CssClass="text-red-600 hover:text-red-800 font-medium ml-auto"
                                        OnClientClick="return confirm('Are you sure you want to delete this comment?');"
                                        CommandName="deleteComment" CommandArgument='<%# Eval("CommentId") %>'
                                        Visible="false" />
                                </div>

                                <!-- Reply Form -->
                                <asp:Panel ID="pnlReply" runat="server" CssClass="mt-4 pl-4 border-l-2 border-gray-200 hidden reply-form">
                                    <h4 class="text-sm font-semibold text-gray-700 mb-2">Post a reply</h4>
                                    <asp:TextBox ID="txtReply" runat="server"
                                        CssClass="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-sm"
                                        TextMode="MultiLine" Rows="2" placeholder="Write a reply..."></asp:TextBox>
                                    <div class="mt-2">
                                        <asp:FileUpload ID="fuReVideo" runat="server" CssClass="text-xs" />
                                    </div>
                                    <div class="mt-2 flex gap-2">
                                        <asp:Button ID="btnReply" runat="server" Text="Reply"
                                            CssClass="px-3 py-1.5 bg-blue-600 hover:bg-blue-700 text-white text-sm font-medium rounded-lg transition-colors duration-200"
                                            CommandName="AddReply" CommandArgument='<%# Eval("CommentId") %>' />
                                        <button type="button" class="px-3 py-1.5 bg-gray-200 hover:bg-gray-300 text-gray-800 text-sm font-medium rounded-lg transition-colors duration-200 cancel-reply">Cancel</button>
                                    </div>
                                </asp:Panel>

                                <!-- Reply List -->
                                <asp:Repeater ID="rptReplies" runat="server" DataSource='<%# Eval("Replies") %>' OnItemCommand="rptReply_ItemCommand" OnItemDataBound="rptReply_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="mt-4 pl-4 border-l-2 border-gray-200">
                                            <div class="flex items-start gap-3">
                                                <img src='<%# ResolveUrl(Eval("UserProfile").ToString()) %>'
                                                    class="w-8 h-8 rounded-full object-cover border border-white shadow"
                                                    alt="User profile" />
                                                <div class="flex-1">
                                                    <div class="flex flex-wrap items-center gap-2 mb-1">
                                                        <span class="px-1.5 py-0.5 text-xs bg-indigo-100 text-indigo-800 rounded-full">
                                                            <%# Eval("Role") %>
                                                        </span>
                                                        <span class="text-2xs font-semibold text-gray-800"><%# Eval("UserName") %></span>
                                                        <span class="text-2xs text-gray-500">•</span>
                                                        <p class="text-2xs text-gray-500"><%# Eval("Date", "{0:MMM dd, yyyy}") %></p>
                                                    </div>
                                                    <p class="text-gray-700 text-sm"><span class="text-sm text-blue-500 mr-2"><%# Eval("ReplyUserName") %></span><%# Eval("ReplyText") %></p>

                                                    <asp:PlaceHolder ID="phVideo" runat="server"
                                                        Visible='<%# Eval("VideoPath") != DBNull.Value && Eval("VideoPath") != null %>'>
                                                        <div class="mt-2 mb-2">
                                                            <video src='<%# Eval("VideoPath") == null ? "#" : ResolveUrl(Eval("VideoPath").ToString()) %>'
                                                                controls class="max-w-2xs md:max-w-xs rounded-lg border border-gray-200 shadow-sm">
                                                            </video>
                                                        </div>
                                                    </asp:PlaceHolder>

                                                    <asp:PlaceHolder ID="phImage" runat="server"
                                                        Visible='<%# Eval("ImagePath") != DBNull.Value && Eval("ImagePath") != null %>'>
                                                        <div class="mt-2 mb-2">
                                                            <img src='<%# Eval("ImagePath") == null ? "#" : ResolveUrl(Eval("ImagePath").ToString()) %>'
                                                                class="max-w-2xs md:max-w-xs rounded-lg border border-gray-200 shadow-sm" alt="Reply image">
                                                        </div>
                                                    </asp:PlaceHolder>

                                                    <div class="flex items-center gap-2 mt-2 text-xs text-gray-500">
                                                        <span><%# GetRelativeTime(Convert.ToDateTime(Eval("Date"))) %></span>
                                                        <button type="button" class="text-blue-600 hover:text-blue-800 font-medium nested-reply-trigger">Reply</button>

                                                        <asp:Button ID="btnReplyDelete" runat="server" Text="Delete"
                                                            CssClass="text-red-600 hover:text-red-800 font-medium ml-auto"
                                                            OnClientClick="return confirm('Are you sure you want to delete this reply?');"
                                                            CommandName="deleteReply" CommandArgument='<%# Eval("ReplyId") %>'
                                                            Visible="false" />
                                                    </div>

                                                    <!-- Nested Reply Form -->
                                                    <asp:Panel ID="pnlReply" runat="server" CssClass="mt-3 pl-3 border-l-2 border-gray-200 hidden nested-reply-form">
                                                        <h4 class="text-xs font-semibold text-gray-700 mb-1">Post a reply</h4>
                                                        <asp:TextBox ID="txtNesReply" runat="server"
                                                            CssClass="w-full px-3 py-1.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-sm"
                                                            TextMode="MultiLine" Rows="2" placeholder="Write a reply..."></asp:TextBox>
                                                        <div class="mt-1.5">
                                                            <asp:FileUpload ID="fuNesVideo" runat="server" CssClass="text-xs" />
                                                        </div>
                                                        <asp:HiddenField ID="hdnCommentId" runat="server"
                                                            Value='<%# Eval("CommentId") %>' />
                                                        <div class="mt-1.5 flex gap-2">
                                                            <asp:Button ID="btnNesReply" runat="server" Text="Reply"
                                                                CssClass="px-2.5 py-1 bg-blue-600 hover:bg-blue-700 text-white text-xs font-medium rounded-lg transition-colors duration-200"
                                                                CommandName="AddNestedReply" CommandArgument='<%# Eval("ReplyId") %>' />
                                                            <button type="button" class="px-2.5 py-1 bg-gray-200 hover:bg-gray-300 text-gray-800 text-xs font-medium rounded-lg transition-colors duration-200 cancel-nested-reply">Cancel</button>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnAddComment" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <style>
        .heart-btn {
            background: none;
            border: none;
            cursor: pointer;
            transition: all 0.3s ease;
        }

        .heart-active {
            color: #ef4444 !important;
            transform: scale(1.1);
            animation: pulse 0.5s;
        }

        @keyframes pulse {
            0% {
                transform: scale(1);
            }

            50% {
                transform: scale(1.2);
            }

            100% {
                transform: scale(1.1);
            }
        }
    </style>

    <script>
        // JavaScript for handling reply form toggling
        document.addEventListener('click', function (e) {
            // Main reply forms
            if (e.target.classList.contains('reply-trigger')) {
                e.preventDefault();
                const form = e.target.closest('.flex.items-center').nextElementSibling;
                form.classList.toggle('hidden');
            }

            if (e.target.classList.contains('cancel-reply')) {
                e.preventDefault();
                const form = e.target.closest('.reply-form');
                form.classList.add('hidden');
            }

            // Nested reply forms
            if (e.target.classList.contains('nested-reply-trigger')) {
                e.preventDefault();
                const form = e.target.closest('.flex.items-center').nextElementSibling;
                form.classList.toggle('hidden');
            }

            if (e.target.classList.contains('cancel-nested-reply')) {
                e.preventDefault();
                const form = e.target.closest('.nested-reply-form');
                form.classList.add('hidden');
            }
        });
    </script>
</asp:Content>
