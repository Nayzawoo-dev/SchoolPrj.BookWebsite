<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SchoolPrj.BookWebsite._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="bg-indigo-100 p-8 rounded-xl shadow mb-10">
            <h1 class="text-4xl font-bold text-indigo-800 mb-4">📖 Welcome to MyBookShelf</h1>
            <p class="text-gray-700 text-lg">
                Dive into a world of books – Explore, categorize, and enjoy reading from a beautifully organized digital shelf.
            </p>
        </div>

    <div class="w-full mx-auto px-4 sm:px-6 lg:px-8 text-center">
        <!-- Hero Section -->
        <div class="bg-white p-6 sm:p-10 rounded-xl shadow-md mt-6 sm:mt-12">
            <h1 class="text-3xl sm:text-4xl md:text-5xl font-extrabold text-indigo-700 mb-4 leading-snug sm:leading-tight">
                📚 Free Books, Infinite Stories
            </h1>
            <p class="text-base sm:text-lg md:text-xl text-gray-700 mb-6">
                Discover thousands of free books across all genres. No payment. No limit. Just read and enjoy.
            </p>

            <a href="login.aspx" class="inline-block px-5 py-3 sm:px-6 sm:py-3 bg-indigo-600 text-white rounded-full text-sm sm:text-lg hover:bg-indigo-700 transition">
                🔐 Login to Start Reading
            </a>
        </div>
    </div>
</asp:Content>