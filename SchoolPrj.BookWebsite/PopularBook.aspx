<%@ Page Title="Books" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PopularBook.aspx.cs" Inherits="SchoolPrj.BookWebsite._PopularBook" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Books | MyBookShelf - Explore Our Collection
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <asp:UpdatePanel ID="upBookCategoy" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- Results Count -->
            <div class="flex justify-between items-center mb-6">
                <h2 class="text-xl font-bold text-gray-800">Popular Book</h2>
            </div>

            <!-- Books Grid -->
            <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6 mb-12">
                <asp:Repeater ID="rptBook" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <!-- Book Card -->
                        <div class="bg-white rounded-xl overflow-hidden shadow-lg hover:shadow-2xl transition-all duration-300 transform hover:-translate-y-1 border border-gray-100">
                            <!-- Book Image -->
                            <div class="h-56 bg-indigo-50 flex items-center justify-center overflow-hidden">
                                <img src='<%# ResolveUrl(Eval("ImagePath").ToString()) %>'
                                    alt='<%# Eval("BookTitle") %>'
                                    class="w-full h-full object-contain p-4" />
                            </div>

                            <!-- Book Details -->
                            <div class="p-5">
                                <h3 class="text-lg font-bold text-gray-800 mb-2 truncate"><%# Eval("BookTitle") %></h3>

                                <div class="flex items-center text-sm text-gray-600 mb-3">
                                    <i class="fas fa-user-circle text-indigo-500 mr-2"></i>
                                    <span class="truncate"><%# Eval("Author") %></span>
                                </div>

                                <div class="flex items-center text-sm text-gray-600 mb-4">
                                    <i class="fas fa-tag text-indigo-500 mr-2"></i>
                                    <span class="truncate"><%# Eval("Category") %></span>
                                </div>

                                <!-- Action Buttons -->
                                <div class="flex justify-between items-center">
                                    <a href='BookDetails.aspx?BookId=<%# Eval("BookId") %>'
                                        class="px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-all text-sm font-medium flex items-center">
                                        <i class="fas fa-eye mr-2"></i>View
                                    </a>

                                    <div class="flex space-x-2">
                                        <asp:HyperLink ID="btnEditBook" runat="server"
                                            NavigateUrl='<%# "EditBook.aspx?BookId=" + Eval("BookId") %>'
                                            Visible="false"
                                            CssClass="px-3 py-2 bg-slate-400 text-white rounded-lg hover:bg-yellow-600 transition-all flex items-center">
                                        <i class="fas fa-edit"></i>
                                        </asp:HyperLink>

                                        <asp:LinkButton ID="btnDelete" runat="server" Visible="false"
                                            CommandArgument='<%# Eval("BookId") %>'
                                            OnClientClick="return confirm('Are you sure you want to delete this book?');"
                                            CssClass="px-3 py-2 bg-rose-500 text-white rounded">
                                        <i class="fas fa-trash"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
