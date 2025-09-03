<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SchoolPrj.BookWebsite.Dashboard" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    User Management | MyBookShelf
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <asp:UpdatePanel ID="upUserManagement" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="dashboard-container">
                <!-- Header -->
                <div class="dashboard-header">
                    <h1>User Management</h1>
                    <p>Manage user accounts and permissions</p>
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
                                placeholder="Search by username or email"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                            CssClass="search-button" />
                    </div>
                </div>

                <!-- User List Table -->
                <div class="table-container mb-16">
                    <div class="table-responsive">
                        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False"
                            OnRowCommand="gvUsers_RowCommand"
                            CssClass="users-table">
                            <HeaderStyle CssClass="table-header" />
                            <Columns>
                                <asp:BoundField DataField="Username" HeaderText="Username"
                                    HeaderStyle-CssClass="table-header-cell username-header"
                                    ItemStyle-CssClass="table-cell username-cell" />

                                <asp:BoundField DataField="Email" HeaderText="Email"
                                    HeaderStyle-CssClass="table-header-cell email-header"
                                    ItemStyle-CssClass="table-cell email-cell" />

                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="table-header-cell action-header" ItemStyle-CssClass="table-cell action-cell">
                                    <ItemTemplate>
                                        <asp:Button runat="server" Text="Ban" CommandName="Ban" CommandArgument='<%# Eval("Email") %>'
                                            CssClass="ban-button"
                                            OnClientClick="return confirm('Are you sure you want to ban this user?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="table-row" />
                            <AlternatingRowStyle CssClass="table-row-alternate" />
                        </asp:GridView>
                    </div>

                    <!-- No Users Message -->
                    <asp:Panel ID="pnlNoUsers" runat="server" Visible="false" CssClass="no-users-message">
                        <div class="no-users-icon">
                            <i class="fas fa-users-slash"></i>
                        </div>
                        <h3>No users found</h3>
                        <p>Try adjusting your search criteria</p>
                        <asp:Button ID="btnUnderstand" runat="server" Text="Browse Users"
                            CssClass="browse-users-button"
                            OnClick="btnUnderstand_Click" />
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <style>
        /* Base styles */
        .dashboard-container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 1rem;
        }

        .dashboard-header {
            margin-bottom: 1.5rem;
            text-align: center;
        }

            .dashboard-header h1 {
                font-size: clamp(1.5rem, 5vw, 2.25rem);
                font-weight: bold;
                color: #1f2937;
                margin-bottom: 0.5rem;
            }

            .dashboard-header p {
                color: #6b7280;
                font-size: 1rem;
            }

        /* Search section */
        .search-section {
            background-color: white;
            border-radius: 0.75rem;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
            border: 1px solid #e5e7eb;
            padding: 1.5rem;
            margin-bottom: 1.5rem;
        }

        .search-container {
            display: flex;
            flex-direction: column;
            gap: 1rem;
        }

        .search-input-container {
            position: relative;
            flex: 1;
        }

        .search-icon {
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0.75rem;
            display: flex;
            align-items: center;
            color: #9ca3af;
        }

        .search-input {
            width: 100%;
            padding: 0.625rem 0.75rem 0.625rem 2.5rem;
            font-size: 1rem;
            border: 1px solid #d1d5db;
            border-radius: 0.5rem;
            transition: border-color 0.2s, box-shadow 0.2s;
        }

            .search-input:focus {
                outline: none;
                border-color: #4f46e5;
                box-shadow: 0 0 0 3px rgba(79, 70, 229, 0.2);
            }

        .search-button {
            padding: 0.625rem 1.25rem;
            font-size: 1rem;
            font-weight: 500;
            color: white;
            background-color: #4f46e5;
            border: none;
            border-radius: 0.5rem;
            cursor: pointer;
            transition: background-color 0.2s;
            min-height: 44px;
        }

            .search-button:hover {
                background-color: #4338ca;
            }

        /* Table container */
        .table-container {
            background-color: white;
            border-radius: 0.75rem;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
            border: 1px solid #e5e7eb;
            overflow: hidden;
            margin-bottom: 1.5rem;
        }

        .table-responsive {
            overflow-x: auto;
            -webkit-overflow-scrolling: touch;
        }

        .users-table {
            width: 100%;
            min-width: 500px;
            border-collapse: collapse;
        }

        .table-header {
            background-color: #f9fafb;
        }

        .table-header-cell {
            padding: 0.75rem 1rem;
            text-align: left;
            font-size: 0.75rem;
            font-weight: 500;
            color: #6b7280;
            text-transform: uppercase;
            letter-spacing: 0.05em;
        }

        .table-row, .table-row-alternate {
            transition: background-color 0.15s;
        }

            .table-row:hover {
                background-color: #f9fafb;
            }

        .table-row-alternate {
            background-color: #f9fafb;
        }

            .table-row-alternate:hover {
                background-color: #f3f4f6;
            }

        .table-cell {
            padding: 1rem;
            font-size: 0.875rem;
            white-space: nowrap;
        }

        .username-cell {
            font-weight: 500;
            color: #1f2937;
        }

        .email-cell {
            color: #6b7280;
        }

        .action-cell {
            text-align: right;
        }

        .ban-button {
            padding: 0.5rem 1rem;
            font-size: 0.75rem;
            font-weight: 500;
            color: white;
            background-color: #dc2626;
            border: none;
            border-radius: 0.375rem;
            cursor: pointer;
            transition: background-color 0.2s;
            min-height: 36px;
            min-width: 70px;
        }

            .ban-button:hover {
                background-color: #b91c1c;
            }

        /* No users message */
        .no-users-message {
            text-align: center;
            padding: 3rem 1rem;
        }

        .no-users-icon {
            display: flex;
            align-items: center;
            justify-content: center;
            height: 3rem;
            width: 3rem;
            border-radius: 9999px;
            background-color: #f3f4f6;
            margin: 0 auto 1rem;
            color: #9ca3af;
            font-size: 1.25rem;
        }

        .no-users-message h3 {
            font-size: 1.125rem;
            font-weight: 500;
            color: #1f2937;
            margin-bottom: 0.5rem;
        }

        .no-users-message p {
            color: #6b7280;
            margin-bottom: 1.5rem;
        }

        .browse-users-button {
            padding: 0.625rem 1.25rem;
            font-size: 0.875rem;
            font-weight: 500;
            color: white;
            background-color: #4f46e5;
            border: none;
            border-radius: 0.375rem;
            cursor: pointer;
            transition: background-color 0.2s;
        }

            .browse-users-button:hover {
                background-color: #4338ca;
            }

        /* Media queries for responsive design */
        @media (min-width: 640px) {
            .search-container {
                flex-direction: row;
                align-items: center;
            }

            .search-button {
                width: auto;
            }

            .table-header-cell {
                padding: 0.75rem 1.5rem;
            }

            .table-cell {
                padding: 1rem 1.5rem;
            }
        }

        @media (min-width: 768px) {
            .dashboard-container {
                padding: 2rem;
            }

            .dashboard-header h1 {
                font-size: 2.25rem;
            }

            .search-section {
                padding: 2rem;
            }
        }

        @media (max-width: 639px) {
            .search-button {
                width: 100%;
            }

            .users-table {
                font-size: 0.8rem;
            }

            .username-header, .email-header, .action-header {
                min-width: 120px;
            }

            .action-cell {
                padding-right: 0.5rem;
            }
        }
    </style>
</asp:Content>
