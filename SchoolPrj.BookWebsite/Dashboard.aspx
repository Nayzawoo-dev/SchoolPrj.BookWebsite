<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SchoolPrj.BookWebsite.Dashboard" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    User Management | MyBookShelf
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <style>
        /* Custom Styles for Dashboard */
        .dashboard-container {
            min-height: calc(100vh - 200px);
        }
        
        .dashboard-header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border-radius: 12px;
            padding: 2rem;
            margin-bottom: 2rem;
            color: white;
            box-shadow: 0 4px 20px rgba(102, 126, 234, 0.15);
        }
        
        .dashboard-header h1 {
            font-size: 2rem;
            font-weight: 700;
            margin-bottom: 0.5rem;
        }
        
        .dashboard-header p {
            opacity: 0.9;
            font-size: 1.1rem;
        }
        
        .search-section {
            background: white;
            border-radius: 12px;
            padding: 1.5rem;
            margin-bottom: 2rem;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
            border: 1px solid #e2e8f0;
        }
        
        .search-container {
            display: flex;
            flex-direction: column;
            gap: 1rem;
        }
        
        @media (min-width: 640px) {
            .search-container {
                flex-direction: row;
                align-items: center;
            }
        }
        
        .search-input-container {
            position: relative;
            flex-grow: 1;
        }
        
        .search-icon {
            position: absolute;
            left: 1rem;
            top: 50%;
            transform: translateY(-50%);
            color: #9ca3af;
        }
        
        .search-input {
            width: 100%;
            padding: 0.75rem 1rem 0.75rem 3rem;
            border: 1px solid #d1d5db;
            border-radius: 8px;
            font-size: 1rem;
            transition: all 0.2s;
        }
        
        .search-input:focus {
            outline: none;
            border-color: #667eea;
            box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
        }
        
        .search-button {
            background: #667eea;
            color: white;
            border: none;
            border-radius: 8px;
            padding: 0.75rem 1.5rem;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s;
            white-space: nowrap;
        }
        
        .search-button:hover {
            background: #5a6fd8;
            transform: translateY(-1px);
            box-shadow: 0 4px 8px rgba(102, 126, 234, 0.2);
        }
        
        .table-container {
            background: white;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
            border: 1px solid #e2e8f0;
        }
        
        .table-responsive {
            overflow-x: auto;
        }
        
        .users-table {
            width: 100%;
            border-collapse: collapse;
            min-width: 600px;
        }
        
        .users-table th {
            background: #f8fafc;
            padding: 1rem 1.5rem;
            text-align: left;
            font-weight: 600;
            color: #374151;
            border-bottom: 1px solid #e5e7eb;
            font-size: 0.875rem;
            text-transform: uppercase;
            letter-spacing: 0.05em;
        }
        
        .users-table td {
            padding: 1rem 1.5rem;
            border-bottom: 1px solid #f3f4f6;
            color: #4b5563;
        }
        
        .users-table tr:last-child td {
            border-bottom: none;
        }
        
        .users-table tr:hover {
            background: #f9fafb;
        }
        
        .ban-button, .unban-button {
            border: none;
            border-radius: 6px;
            padding: 0.5rem 1rem;
            font-size: 0.875rem;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s;
        }
        
        .ban-button {
            background: #ef4444;
            color: white;
        }
        
        .ban-button:hover {
            background: #dc2626;
            transform: translateY(-1px);
            box-shadow: 0 2px 4px rgba(239, 68, 68, 0.2);
        }
        
        .unban-button {
            background: #10b981;
            color: white;
        }
        
        .unban-button:hover {
            background: #059669;
            transform: translateY(-1px);
            box-shadow: 0 2px 4px rgba(16, 185, 129, 0.2);
        }
        
        .no-users-message {
            text-align: center;
            padding: 3rem 2rem;
        }
        
        .no-users-icon {
            font-size: 4rem;
            color: #9ca3af;
            margin-bottom: 1.5rem;
        }
        
        .no-users-message h3 {
            font-size: 1.5rem;
            font-weight: 600;
            color: #374151;
            margin-bottom: 0.5rem;
        }
        
        .no-users-message p {
            color: #6b7280;
            margin-bottom: 1.5rem;
        }
        
        .browse-users-button {
            background: #667eea;
            color: white;
            border: none;
            border-radius: 8px;
            padding: 0.75rem 1.5rem;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s;
        }
        
        .browse-users-button:hover {
            background: #5a6fd8;
            transform: translateY(-1px);
            box-shadow: 0 4px 8px rgba(102, 126, 234, 0.2);
        }
        
        /* Stats Cards */
        .stats-container {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
            gap: 1.5rem;
            margin-bottom: 2rem;
        }
        
        .stat-card {
            background: white;
            border-radius: 12px;
            padding: 1.5rem;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
            border: 1px solid #e2e8f0;
            transition: transform 0.2s;
        }
        
        .stat-card:hover {
            transform: translateY(-2px);
        }
        
        .stat-card-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 1rem;
        }
        
        .stat-card-icon {
            width: 3rem;
            height: 3rem;
            border-radius: 10px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.5rem;
        }
        
        .stat-card-title {
            font-size: 0.875rem;
            color: #6b7280;
            font-weight: 500;
        }
        
        .stat-card-value {
            font-size: 1.875rem;
            font-weight: 700;
            color: #1f2937;
        }
        
        .stat-card-change {
            font-size: 0.875rem;
            display: flex;
            align-items: center;
            margin-top: 0.5rem;
        }
        
        .stat-card-change.positive {
            color: #10b981;
        }
        
        .stat-card-change.negative {
            color: #ef4444;
        }
        
        /* Filter Section */
        .filter-section {
            background: white;
            border-radius: 12px;
            padding: 1.5rem;
            margin-bottom: 2rem;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
            border: 1px solid #e2e8f0;
        }
        
        .filter-container {
            display: flex;
            flex-direction: column;
            gap: 1rem;
        }
        
        @media (min-width: 768px) {
            .filter-container {
                flex-direction: row;
                align-items: center;
            }
        }
        
        .filter-group {
            display: flex;
            flex-direction: column;
            gap: 0.5rem;
            flex-grow: 1;
        }
        
        .filter-label {
            font-size: 0.875rem;
            font-weight: 500;
            color: #374151;
        }
        
        .filter-select {
            padding: 0.75rem 1rem;
            border: 1px solid #d1d5db;
            border-radius: 8px;
            font-size: 1rem;
            background: white;
            transition: all 0.2s;
        }
        
        .filter-select:focus {
            outline: none;
            border-color: #667eea;
            box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
        }
        
        .filter-button {
            background: #667eea;
            color: white;
            border: none;
            border-radius: 8px;
            padding: 0.75rem 1.5rem;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s;
            white-space: nowrap;
            margin-top: 1.5rem;
        }
        
        @media (min-width: 768px) {
            .filter-button {
                margin-top: 1.75rem;
            }
        }
        
        .filter-button:hover {
            background: #5a6fd8;
            transform: translateY(-1px);
            box-shadow: 0 4px 8px rgba(102, 126, 234, 0.2);
        }
        
        /* Loading indicator */
        .loading-overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(255, 255, 255, 0.8);
            display: flex;
            align-items: center;
            justify-content: center;
            z-index: 9999;
            opacity: 0;
            visibility: hidden;
            transition: all 0.3s;
        }
        
        .loading-overlay.active {
            opacity: 1;
            visibility: visible;
        }
        
        .loading-spinner {
            width: 50px;
            height: 50px;
            border: 5px solid #f3f4f6;
            border-top: 5px solid #667eea;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }
        
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
    </style>

    <asp:UpdatePanel ID="upUserManagement" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="dashboard-container">
                <!-- Loading Overlay -->
                <div class="loading-overlay" id="loadingOverlay">
                    <div class="loading-spinner"></div>
                </div>

                <!-- Header -->
                <div class="dashboard-header">
                    <h1>User Management</h1>
                    <p>Manage user accounts and permissions for Phat Sa Yar Book Community</p>
                </div>

                <!-- Stats Cards -->
                <div class="stats-container">
                    <div class="stat-card">
                        <div class="stat-card-header">
                            <div>
                                <div class="stat-card-title">Total Users</div>
                                <asp:Label ID="lblTotalUsers" runat="server" CssClass="stat-card-value"></asp:Label>
                            </div>
                            <div class="stat-card-icon" style="background: rgba(102, 126, 234, 0.1); color: #667eea;">
                                <i class="fas fa-users"></i>
                            </div>
                        </div>
                    </div>
                    
                    <div class="stat-card">
                        <div class="stat-card-header">
                            <div>
                                <div class="stat-card-title">TotalBook</div>
                                <asp:Label ID="lblTotalBook" runat="server" CssClass="stat-card-value"></asp:Label>
                            </div>
                            <div class="stat-card-icon" style="background: rgba(16, 185, 129, 0.1); color: #10b981;">
                               <i class="fas fa-book mr-2"></i>
                            </div>
                        </div>
                    </div>
                    
                
                </div>

                <!-- Search Section -->
                <div class="search-section">
                    <div class="search-container">
                        <div class="search-input-container">
                            <div class="search-icon">
                                <i class="fas fa-search"></i>
                            </div>
                            <asp:TextBox ID="txtSearch" runat="server"
                                CssClass="search-input"
                                placeholder="Search by username, email, or user ID"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnSearch" runat="server" Text="Search Users" OnClick="btnSearch_Click"
                            CssClass="search-button" />
                    </div>
                </div>

                <!-- User List Table -->
                <div class="table-container mb-16">
                    <div class="table-responsive">
                        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="UserId"
                            CssClass="users-table"
                            OnRowCommand="gvUsers_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="Username" HeaderText="Username" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <span class='status-badge <%# Convert.ToInt32(Eval("BanStatus")) == 0 ? "status-active" : "status-banned" %>'>
                                            <%# Convert.ToInt32(Eval("BanStatus")) == 0 ? "Active" : "Banned" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <!-- Ban Button -->
                                        <asp:Button ID="btnBan" runat="server" Text="Ban User"
                                            CommandName="Ban"
                                            CommandArgument='<%# Eval("UserId") %>'
                                            CssClass="ban-button"
                                            Visible='<%# Convert.ToInt32(Eval("BanStatus")) == 0 %>'
                                            OnClientClick="showLoading(); return confirm('Are you sure you want to ban this user?');" />

                                        <!-- Unban Button -->
                                        <asp:Button ID="btnUnban" runat="server" Text="Unban User"
                                            CommandName="Unban"
                                            CommandArgument='<%# Eval("UserId") %>'
                                            CssClass="unban-button"
                                            Visible='<%# Convert.ToInt32(Eval("BanStatus")) == 1 %>'
                                            OnClientClick="showLoading(); return confirm('Are you sure you want to unban this user?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>

                    <asp:Panel ID="pnlNoUsers" runat="server" Visible="false" CssClass="no-users-message">
                        <div class="no-users-icon">
                            <i class="fas fa-users-slash"></i>
                        </div>
                        <h3>No users found</h3>
                        <p>Try adjusting your search criteria or filters</p>
                        <asp:Button ID="btnUnderstand" runat="server" Text="Browse All Users"
                            CssClass="browse-users-button"
                            OnClick="btnUnderstand_Click" />
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <script>
        // Show loading indicator
        function showLoading() {
            document.getElementById('loadingOverlay').classList.add('active');
        }
        
        // Hide loading indicator when page loads or update panel refreshes
        document.addEventListener('DOMContentLoaded', function() {
            document.getElementById('loadingOverlay').classList.remove('active');
        });
        
        // Handle update panel events to hide loading indicator
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function() {
            document.getElementById('loadingOverlay').classList.remove('active');
        });
        
        // Add status badge styles
        document.addEventListener('DOMContentLoaded', function() {
            const style = document.createElement('style');
            style.textContent = `
                .status-badge {
                    padding: 0.25rem 0.75rem;
                    border-radius: 9999px;
                    font-size: 0.75rem;
                    font-weight: 600;
                    text-transform: uppercase;
                    letter-spacing: 0.05em;
                }
                .status-active {
                    background: rgba(16, 185, 129, 0.1);
                    color: #059669;
                }
                .status-banned {
                    background: rgba(239, 68, 68, 0.1);
                    color: #dc2626;
                }
                .view-profile-button {
                    display: inline-flex;
                    align-items: center;
                    justify-content: center;
                    width: 2.5rem;
                    height: 2.5rem;
                    background: #f3f4f6;
                    color: #4b5563;
                    border-radius: 6px;
                    margin-left: 0.5rem;
                    transition: all 0.2s;
                }
                .view-profile-button:hover {
                    background: #667eea;
                    color: white;
                    transform: translateY(-1px);
                }
            `;
            document.head.appendChild(style);
        });
    </script>
</asp:Content>