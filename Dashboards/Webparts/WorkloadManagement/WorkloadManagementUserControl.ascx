﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
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
      <asp:TableCell>
        <asp:Panel ID="Panel1" runat="server" BorderColor="ActiveCaptionText" GroupingText="Select day">
          <asp:Calendar ID="m_Calendar" runat="server" SelectionMode="Day" BackColor="White" BorderColor="#91cdf2"
            CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black"
            Height="62px" OnSelectionChanged="m_Calendar_SelectionChanged" Width="131px">
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
      <asp:TableCell>
        <asp:Panel runat="server" BorderColor="ActiveCaptionText" GroupingText="Workload">
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
                <asp:Label runat="server" ID="m_TaskLabel" Text="Task" CssClass="Label"></asp:Label>
              </asp:TableCell>
              <asp:TableCell>
                <asp:DropDownList runat="server" ID="m_TaskDropDown" CssClass="DropDown">
                </asp:DropDownList>
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
                <asp:Label runat="server" ID="m_WorkloadHoursLabel" Text="Hours" CssClass="Label"></asp:Label>
              </asp:TableCell>
              <asp:TableCell>
                <asp:TextBox runat="server" ID="m_WorkloadHoursTextBox" CssClass="TextBoxShort"></asp:TextBox>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
              <asp:TableCell>
                <asp:Label runat="server" ID="m_WorkloadMinutesLabel" Text="Minutes" CssClass="Label"></asp:Label>
              </asp:TableCell>
              <asp:TableCell>
                <asp:DropDownList runat="server" ID="m_WorkloadMinutesDropDown" CssClass="DropDownShort">
                </asp:DropDownList>
              </asp:TableCell>
            </asp:TableRow>
          </asp:Table>
        </asp:Panel>
      </asp:TableCell>
      <asp:TableCell>
        <asp:Panel runat="server" BorderColor="ActiveCaptionText" GroupingText="Actions">
          <asp:Button runat="server" ID="m_ButtonAddNew" Text="Add new" CssClass="Button" /><br />
          <asp:Button runat="server" ID="m_ButtonEdit" Text="Edit" CssClass="Button" /><br />
          <asp:Button runat="server" ID="m_ButtonSave" Text="Save" CssClass="Button" /><br />
          <asp:Button runat="server" ID="m_ButtonDelete" Text="Delete" CssClass="Button" /><br />
          <asp:Button runat="server" ID="m_ButtonCancel" Text="Cancel" CssClass="Button" /><br />
        </asp:Panel>
      </asp:TableCell>
    </asp:TableRow>
  </asp:Table>
</asp:Panel>
<asp:Panel ID="m_PanelSelect" runat="server" BorderColor="ActiveCaptionText" GroupingText="Current workload">
  <asp:GridView ID="m_GridView" runat="server" AutoGenerateColumns="False" BackColor="White"
    BorderColor="#FFFFFF" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataKeyNames="ID"
    ForeColor="Black" GridLines="Vertical" OnSelectedIndexChanged="m_GridView_SelectedIndexChanged" AutoGenerateSelectButton=true>
    <%--              <Columns>
                <asp:CommandField ButtonType="Button" SelectText="Zaznacz" ShowCancelButton="False"
                  ShowSelectButton="True" />
                <asp:TemplateField>
                  <ItemTemplate>
                    <asp:Button ID="Button_modify" runat="server" CommandArgument='<%# Eval("ID") %>'
                      CommandName="Update" OnClientClick="return confirm('Na pewno zmodyfikować?');"
                      Text="Zmodyfikuj" />
                  </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="NAZWA_KROTKA" HeaderText="Projekt" SortExpression="NAZWA_KROTKA" />
                <asp:BoundField DataField="LICZBA_GODZIN" HeaderText="Godziny" SortExpression="LICZBA_GODZIN" />
                <asp:BoundField DataField="OPIS" HeaderText="Opis" SortExpression="OPIS" />
                <asp:BoundField DataField="DATA_WPISU" HeaderText="Data" SortExpression="DATA_WPISU"
                  DataFormatString="{0:d}" HtmlEncode="False" />
                <asp:BoundField DataField="NAZWAPRACY" HeaderText="Rodzaj pracy" SortExpression="NAZWAPRACY" />
                <asp:TemplateField>
                  <ItemTemplate>
                    <asp:Button ID="Button_delete" runat="server" CommandArgument='<%# Eval("ID") %>'
                      CommandName="Delete" OnClientClick="return confirm('Na pewno usunąć?');" Text="Usuń" />
                  </ItemTemplate>
                </asp:TemplateField>
              </Columns>
    --%>
    <FooterStyle BackColor="#CCCC99" />
    <RowStyle BackColor="#F7F7DE" />
    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
    <AlternatingRowStyle BackColor="White" />
  </asp:GridView>
</asp:Panel>
