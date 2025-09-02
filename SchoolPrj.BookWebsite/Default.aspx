<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SchoolPrj.BookWebsite._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home | MyBookShelf - Discover Your Next Favorite Book
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Hero Section -->
    <section class="relative bg-gradient-to-r from-indigo-600 to-purple-700 text-white py-16 md:py-24 rounded-xl overflow-hidden mb-12 shadow-lg">
        <div class="absolute inset-0 bg-black opacity-10"></div>
        <div class="absolute top-0 right-0 -mt-16 mr-10 opacity-20">
            <i class="fas fa-book-open text-white text-9xl"></i>
        </div>
        <div class="container mx-auto px-4 relative z-10">
            <div class="max-w-3xl mx-auto text-center">
                <h1 class="text-4xl md:text-5xl lg:text-6xl font-bold mb-6 leading-tight">Discover Your Next <span class="text-yellow-300">Favorite Book</span></h1>
                <p class="text-xl md:text-2xl mb-8 text-indigo-100">Explore thousands of free books across all genres. No payment. No limit. Just read and enjoy.</p>
                <div class="flex flex-col sm:flex-row justify-center gap-4">
                    <a href="Book.aspx" class="bg-white text-indigo-700 hover:bg-indigo-50 font-semibold py-3 px-8 rounded-full transition duration-300 transform hover:-translate-y-1 shadow-lg flex items-center justify-center">
                        <i class="fas fa-book-open mr-2"></i>Browse Books
                    </a>
                    <asp:HyperLink ID="HyperLink1" NavigateUrl="Login.aspx" runat="server" class="bg-indigo-800 hover:bg-indigo-900 text-white font-semibold py-3 px-8 rounded-full transition duration-300 border border-indigo-500 flex items-center justify-center">
                        <i class="fas fa-key mr-2"></i>Get Started
                    </asp:HyperLink>
                </div>
            </div>
        </div>
    </section>

    <!-- Features Section -->
    <section class="mb-16">
        <div class="container mx-auto px-4">
            <h2 class="text-3xl md:text-4xl font-bold text-center text-gray-800 mb-12">Why Choose <span class="text-indigo-600">MyBookShelf</span>?</h2>

            <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
                <!-- Feature 1 -->
                <div class="bg-white p-8 rounded-xl shadow-md hover:shadow-xl transition-all duration-300 border border-indigo-100 text-center">
                    <div class="w-16 h-16 bg-indigo-100 rounded-full flex items-center justify-center mx-auto mb-6">
                        <i class="fas fa-book text-3xl text-indigo-600"></i>
                    </div>
                    <h3 class="text-xl font-semibold text-gray-800 mb-4">Extensive Collection</h3>
                    <p class="text-gray-600">Access thousands of books across all genres - from classics to contemporary bestsellers.</p>
                </div>

                <!-- Feature 2 -->
                <div class="bg-white p-8 rounded-xl shadow-md hover:shadow-xl transition-all duration-300 border border-indigo-100 text-center">
                    <div class="w-16 h-16 bg-indigo-100 rounded-full flex items-center justify-center mx-auto mb-6">
                        <i class="fas fa-tags text-3xl text-indigo-600"></i>
                    </div>
                    <h3 class="text-xl font-semibold text-gray-800 mb-4">Organized Categories</h3>
                    <p class="text-gray-600">Find exactly what you're looking for with our carefully curated categories and genres.</p>
                </div>

                <!-- Feature 3 -->
                <div class="bg-white p-8 rounded-xl shadow-md hover:shadow-xl transition-all duration-300 border border-indigo-100 text-center">
                    <div class="w-16 h-16 bg-indigo-100 rounded-full flex items-center justify-center mx-auto mb-6">
                        <i class="fas fa-user-circle text-3xl text-indigo-600"></i>
                    </div>
                    <h3 class="text-xl font-semibold text-gray-800 mb-4">Personal Profile</h3>
                    <p class="text-gray-600">Track your reading, save favorites, and get personalized recommendations.</p>
                </div>
            </div>
        </div>
    </section>

    <!-- Stats Section -->
    <section class="bg-indigo-50 py-12 rounded-xl mb-16">
        <div class="container mx-auto px-4">
            <div class="grid grid-cols-2 md:grid-cols-4 gap-6 text-center">
                <div class="p-6">
                    <div class="text-4xl md:text-5xl font-bold text-indigo-700 mb-2">10,000+</div>
                    <div class="text-gray-600">Books Available</div>
                </div>
                <div class="p-6">
                    <div class="text-4xl md:text-5xl font-bold text-indigo-700 mb-2">50+</div>
                    <div class="text-gray-600">Categories</div>
                </div>
                <div class="p-6">
                    <div class="text-4xl md:text-5xl font-bold text-indigo-700 mb-2">100%</div>
                    <div class="text-gray-600">Free Access</div>
                </div>
                <div class="p-6">
                    <div class="text-4xl md:text-5xl font-bold text-indigo-700 mb-2">24/7</div>
                    <div class="text-gray-600">Available</div>
                </div>
            </div>
        </div>
    </section>

    <!-- CTA Section -->
    <section class="bg-gradient-to-r from-purple-600 to-indigo-700 rounded-xl p-10 text-center text-white mb-12 shadow-lg">
        <h2 class="text-3xl md:text-4xl font-bold mb-6">Ready to Start Your Reading Journey?</h2>
        <p class="text-xl text-purple-100 mb-8 max-w-2xl mx-auto">Join thousands of readers who have discovered their next favorite book on MyBookShelf.</p>
        <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Register.aspx"
            CssClass="inline-block bg-white text-indigo-700 font-semibold py-3 px-8 rounded-full hover:bg-indigo-50 transition duration-300 shadow-lg"><i class="fas fa-user-plus mr-2"></i> Create Account
        </asp:HyperLink>
    </section>

    <!-- Testimonials Section -->
    <section class="mb-16">
        <h2 class="text-3xl font-bold text-center text-gray-800 mb-12">What Our <span class="text-indigo-600">Readers</span> Say</h2>

        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            <!-- Testimonial 1 -->
            <div class="bg-white p-6 rounded-xl shadow-md border border-indigo-100">
                <div class="flex items-center mb-4">
                    <div class="w-12 h-12 bg-indigo-100 rounded-full flex items-center justify-center mr-4">
                        <i class="fas fa-user text-indigo-600"></i>
                    </div>
                    <div>
                        <h4 class="font-semibold">Sarah M.</h4>
                        <div class="flex text-yellow-400">
                            <i class="fas fa-star"></i>
                            <i class="fas fa-star"></i>
                            <i class="fas fa-star"></i>
                            <i class="fas fa-star"></i>
                            <i class="fas fa-star"></i>
                        </div>
                    </div>
                </div>
                <p class="text-gray-600 italic">"I've discovered so many amazing books here that I wouldn't have found elsewhere. The categorization is fantastic!"</p>
            </div>

            <!-- Testimonial 2 -->
            <div class="bg-white p-6 rounded-xl shadow-md border border-indigo-100">
                <div class="flex items-center mb-4">
                    <div class="w-12 h-12 bg-indigo-100 rounded-full flex items-center justify-center mr-4">
                        <i class="fas fa-user text-indigo-600"></i>
                    </div>
                    <div>
                        <h4 class="font-semibold">James L.</h4>
                        <div class="flex text-yellow-400">
                            <i class="fas fa-star"></i>
                            <i class="fas fa-star"></i>
                            <i class="fas fa-star"></i>
                            <i class="fas fa-star"></i>
                            <i class="fas fa-star-half-alt"></i>
                        </div>
                    </div>
                </div>
                <p class="text-gray-600 italic">"As an avid reader on a budget, MyBookShelf has been a game-changer. So many quality books completely free!"</p>
            </div>

            <!-- Testimonial 3 -->
            <div class="bg-white p-6 rounded-xl shadow-md border border-indigo-100">
                <div class="flex items-center mb-4">
                    <div class="w-12 h-12 bg-indigo-100 rounded-full flex items-center justify-center mr-4">
                        <i class="fas fa-user text-indigo-600"></i>
                    </div>
                    <div>
                        <h4 class="font-semibold">Emma K.</h4>
                        <div class="flex text-yellow-400">
                            <i class="fas fa-star"></i>
                            <i class="fas fa-star"></i>
                            <i class="fas fa-star"></i>
                            <i class="fas fa-star"></i>
                            <i class="fas fa-star"></i>
                        </div>
                    </div>
                </div>
                <p class="text-gray-600 italic">"The user interface is beautiful and easy to navigate. I love how I can track my reading progress!"</p>
            </div>
        </div>
    </section>
</asp:Content>
