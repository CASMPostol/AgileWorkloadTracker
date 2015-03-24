<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkloadManagementUserControl.ascx.cs"
    Inherits="CAS.AgileWorkloadTracker.Dashboards.Webparts.WorkloadManagement.WorkloadManagementUserControl" %>
<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/AWT_Dashboards/CAS_AWT.css" />
<asp:Panel ID="m_PanelAddEdit" runat="server" BorderColor="ActiveCaptionText" GroupingText="Add/Edit workload">
    <asp:Table ID="m_Table" runat="server" CssClass="Table">
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <asp:Panel ID="Panel1" runat="server" BorderColor="ActiveCaptionText" GroupingText="Select day">
                    <asp:Calendar ID="m_Calendar" runat="server" SelectionMode="Day" BackColor="White"
                        BorderColor="#91cdf2" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana"
                        Font-Size="8pt" ForeColor="Black" Height="62px" OnSelectionChanged="m_Calendar_SelectionChanged"
                        Width="131px">
                        <SelectedDayStyle BackColor="#91cdf2" Font-Bold="True" ForeColor="White" />
                        <TodayDayStyle BackColor="#addbf7" ForeColor="Black" />
                        <SelectorStyle BackColor="#addbf7" />
                        <WeekendDayStyle BackColor="#d5edfb" />
                        <OtherMonthDayStyle ForeColor="Gray" />
                        <NextPrevStyle VerticalAlign="Bottom" />
                        <DayHeaderStyle BackColor="#d5edfb" Font-Bold="True" Font-Size="7pt" />
                        <TitleStyle BackColor="#91cdf2" BorderColor="Black" Font-Bold="True" />
                    </asp:Calendar>
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:Table runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Panel ID="Panel2" runat="server" BorderColor="ActiveCaptionText" GroupingText="Workload">
                                <asp:Table ID="m_TableWorkloadData" runat="server" CssClass="TableInside">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label runat="server" ID="m_ProjectLabel" Text="Project" CssClass="Label"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:DropDownList runat="server" ID="m_ProjectDropDown" CssClass="DropDown" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="m_MilestoneLabel" runat="server" CssClass="Label" Text="Milestone" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:DropDownList ID="m_MilestoneDropDown" runat="server" CssClass="DropDown" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label runat="server" ID="m_TaskLabel" Text="Task" CssClass="Label"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:DropDownList runat="server" ID="m_TaskDropDown" CssClass="DropDown">
                                            </asp:DropDownList>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label runat="server" ID="m_NewTaskLabel" Text="New task" CssClass="Label"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="m_NewTaskLabel2" runat="server" CssClass="Label" Text="You can't find the task on the list above ?" />
                                            <a class="ms-addnew" href="../WebPartPages/AddEditTasks.aspx" target="_self">Add new task</a>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label runat="server" ID="m_WorkloadDescriptionLabel" Text="Description" CssClass="Label"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox runat="server" ID="m_WorkloadDescriptionTextBox" CssClass="TextBox"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <<asp:Label ID="m_WorkloadStart" runat="server" CssClass="Label" Text="Workload - start" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <SharePoint:DateTimeControl ID="m_WorkloadStartDateTimeControl" DateOnly="false" runat="server" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <<asp:Label ID="m_WorkloadEnd" runat="server" CssClass="Label" Text="Workload - end" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <SharePoint:DateTimeControl ID="m_WorkloadEndDateTimeControl" DateOnly="false" runat="server" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Panel ID="Panel3" runat="server" BorderColor="ActiveCaptionText" GroupingText="Actions"
                                Enabled="true">
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
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
<asp:Panel ID="m_PanelSelect" runat="server" BorderColor="ActiveCaptionText" GroupingText="Workload - today's summary">
    <asp:GridView ID="m_GridView" runat="server" AutoGenerateColumns="False" BackColor="#91cdf2"
        BorderColor="#91cdf2" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataKeyNames="ID"
        ForeColor="Black" GridLines="Vertical" OnSelectedIndexChanged="m_GridView_SelectedIndexChanged"
        AutoGenerateSelectButton="true">
        <FooterStyle BackColor="#91cdf2" />
        <RowStyle BackColor="#FFFFFF" BorderColor="#91cdf2" />
        <SelectedRowStyle BackColor="#d5edfb" Font-Bold="True" ForeColor="#000000" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <HeaderStyle BackColor="#91cdf2" Font-Bold="True" ForeColor="#000000" />
        <AlternatingRowStyle BackColor="#FFFFFF" />
    </asp:GridView>
    <asp:Label runat="server" ID="m_HoursADayLabel"></asp:Label>
</asp:Panel>
<asp:Panel ID="m_PanelProjectummary" runat="server" BorderColor="ActiveCaptionText"
    GroupingText="Workload - project's summary">
    <asp:GridView ID="m_GridViewProjectSummary" runat="server" AutoGenerateColumns="False"
        BackColor="#91cdf2" BorderColor="#91cdf2" BorderStyle="None" BorderWidth="1px"
        CellPadding="4" DataKeyNames="ID" ForeColor="Black" GridLines="Vertical" OnSelectedIndexChanged="m_GridView_SelectedIndexChanged"
        AutoGenerateSelectButton="false">
        <FooterStyle BackColor="#91cdf2" />
        <RowStyle BackColor="#FFFFFF" BorderColor="#91cdf2" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <HeaderStyle BackColor="#91cdf2" Font-Bold="True" ForeColor="#000000" />
        <AlternatingRowStyle BackColor="#FFFFFF" />
    </asp:GridView>
</asp:Panel>
