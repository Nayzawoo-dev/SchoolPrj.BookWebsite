<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerifyEmail.aspx.cs" Inherits="SchoolPrj.BookWebsite.VerifyEmail" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Email Verification - MyBookShelf</title>
    <script src="https://cdn.tailwindcss.com"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css">
</head>
<body class="bg-gradient-to-br from-indigo-50 to-purple-100 min-h-screen flex items-center justify-center">
    <form id="form1" runat="server">
        <div class="max-w-md w-full bg-white rounded-2xl shadow-xl overflow-hidden">
            <div class="bg-gradient-to-r from-indigo-600 to-purple-700 py-6 px-8 text-center">
                <h1 class="text-3xl font-bold text-white mb-2 flex items-center justify-center">
                    <i class="fas fa-book-reader mr-3"></i>Phat Sa Yar
                </h1>
                <p class="text-indigo-100">Email Verification</p>
            </div>
            
            <div class="py-8 px-8 text-center">
                <asp:Label ID="lblMessage" runat="server" CssClass="text-lg font-medium"></asp:Label>
                <div class="mt-6">
                    <a href="Login.aspx" id="btnLogin" runat="server" class="bg-indigo-600 text-white px-6 py-3 rounded-xl hover:bg-indigo-700 transition-all font-medium">
                        Go to Login <i class="fas fa-arrow-right ml-2"></i>
                    </a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>