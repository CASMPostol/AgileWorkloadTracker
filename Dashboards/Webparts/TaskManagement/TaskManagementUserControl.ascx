﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskManagementUserControl.ascx.cs"
    Inherits="CAS.AgileWorkloadTracker.Dashboards.Webparts.TaskManagement.TaskManagementUserControl" %>
<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/AWT_Dashboards/CAS_AWT.css" />
<asp:Panel ID="m_PanelAddEdit" runat="server" BorderColor="ActiveCaptionText" GroupingText="Add/Edit task">
    <asp:Table ID="m_TableMain" runat="server" CssClass="Table">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="m_ProjectLabel" runat="server" CssClass="Label" Text="Project:" />
            </asp:TableCell>
            <asp:TableCell ColumnSpan="3">
                <asp:Label ID="m_DisplaySelectedProject" runat="server" CssClass="Label" Text="" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="m_TaskLabel" runat="server" CssClass="Label" Text="Summary" />
            </asp:TableCell>
            <asp:TableCell ColumnSpan="3">
                <asp:TextBox ID="m_TaskTextBox" runat="server" CssClass="TextBoxLong" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="m_RequirementLabel" runat="server" CssClass="Label" Text="Requirement"></asp:Label>
            </asp:TableCell>
            <asp:TableCell ColumnSpan="3">
                <asp:DropDownList ID="m_RequirementDropDown" runat="server" CssClass="DropDownShort"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="m_VersionLabel" runat="server" CssClass="Label" Text="Version" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="m_VersionDropDown" runat="server" CssClass="DropDownShort">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="m_CategoryLabel" runat="server" CssClass="Label" Text="Category" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="m_CategoryDropDown" runat="server" CssClass="DropDownShort">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="m_TypeLabel" runat="server" CssClass="Label" Text="Type" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="m_TypeDropDown" runat="server" CssClass="DropDownShort">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="m_MilestoneLabel" runat="server" CssClass="Label" Text="Milestone" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="m_MilestoneDropDown" runat="server" CssClass="DropDownShort">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="m_PriorityLabel" runat="server" CssClass="Label" Text="Priority" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="m_PriorityDropDown" runat="server" CssClass="DropDownShort">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="m_AsinegdToLabel" runat="server" CssClass="Label" Text="Asigned to" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="m_AsignedToDropDown" runat="server" CssClass="DropDownShort">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="m_StatusLabel" runat="server" CssClass="Label" Text="Status" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="m_StatusDropDown" runat="server" CssClass="DropDownShort">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="m_ResolutionLabel" runat="server" CssClass="Label" Text="Resolution" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="m_ResolutionDropDown" runat="server" CssClass="DropDownShort">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="m_EstimationLabel" runat="server" CssClass="Label" Text="Estimation" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="m_EstimationDropDown" runat="server" CssClass="DropDownShort">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="m_DueDateLabel" runat="server" CssClass="Label" Text="DueDate" />
            </asp:TableCell>
            <asp:TableCell>
                <SharePoint:DateTimeControl ID="m_DueDateDateTimeControl" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="4">
                <asp:TextBox ID="m_CommentsTextBox" runat="server" TextMode="Multiline" Height="100"
                    Width="750" CssClass="TextBox"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="4">
                <asp:Table ID="m_Actions" runat="server" CssClass="Table">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button runat="server" ID="m_ButtonAddNew" Text="Add new" CssClass="Button" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button runat="server" ID="m_ButtonEdit" Text="Edit" CssClass="Button" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button runat="server" ID="m_ButtonSave" Text="Save" CssClass="Button" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button runat="server" ID="m_ButtonDelete" Text="Delete" CssClass="Button" OnClientClick="return confirm('Na pewno zmodyfikować?');" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button runat="server" ID="m_ButtonCancel" Text="Cancel" CssClass="Button" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:TableCell></asp:TableRow>
    </asp:Table>
</asp:Panel>
