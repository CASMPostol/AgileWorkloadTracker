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
<table class="masterTable" style="width: 100%; height: 100%">
  <tr>
    <td style="width: 480px; height: 340px;" valign="top">
      <asp:Label ID="m_Label1" runat="server" Text="Label"></asp:Label><br />
      &nbsp;
      <table style="width: 580px; height: 210px;">
        <tr>
          <td style="width: 279px; height: 200px;" valign="top">
            <asp:Calendar ID="m_Calendar" runat="server" BackColor="White" BorderColor="#999999"
              CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black"
              Height="62px" OnSelectionChanged="Calendar1_SelectionChanged" Width="131px">
              <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
              <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
              <SelectorStyle BackColor="#CCCCCC" />
              <WeekendDayStyle BackColor="#FFFFCC" />
              <OtherMonthDayStyle ForeColor="Gray" />
              <NextPrevStyle VerticalAlign="Bottom" />
              <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
              <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
            </asp:Calendar>
            <br />
            <asp:Label ID="Label14" runat="server"></asp:Label><br />
            <br />
          </td>
          <td style="width: 320px; height: 200px;" valign="top">
            <asp:Label ID="Label10" runat="server" Text="Plan"></asp:Label><br />
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" BackColor="White"
              BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataKeyNames="ID"
              DataSourceID="PlanDataSource" ForeColor="Black" GridLines="Vertical">
              <FooterStyle BackColor="#CCCC99" />
              <Columns>
                <asp:BoundField DataField="NAZWA_KROTKA" HeaderText="Projekt" SortExpression="NAZWA_KROTKA" />
                <asp:BoundField DataField="GODZINY" HeaderText="Godziny" SortExpression="GODZINY" />
                <asp:BoundField DataField="OPIS" HeaderText="Opis" SortExpression="OPIS" />
                <asp:BoundField DataField="DATA" HeaderText="Data" SortExpression="DATA" DataFormatString="{0:d}"
                  HtmlEncode="False" />
              </Columns>
              <RowStyle BackColor="#F7F7DE" />
              <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
              <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
              <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
              <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <asp:ObjectDataSource ID="PlanDataSource" runat="server" OldValuesParameterFormatString="original_{0}"
              SelectMethod="GetDataByPrac" TypeName="widok.DataSetTableAdapters.PLAN_PROJEKTYTableAdapter">
              <SelectParameters>
                <asp:Parameter Name="id_projektu" Type="Decimal" />
                <asp:SessionParameter Name="login" SessionField="CurrentUser" Type="String" />
              </SelectParameters>
            </asp:ObjectDataSource>
            <br />
            <asp:Label ID="Label12" runat="server" ForeColor="Transparent">Aktualnie masz 0 godzin w tym projekcie</asp:Label><br />
            <br />
            <br />
            <br />
            <asp:Label ID="Label15" runat="server"></asp:Label><br />
            <br />
            <asp:Label ID="Label8" runat="server"></asp:Label><br />
            <br />
          </td>
        </tr>
        <tr>
          <td style="width: 279px; height: 140px;" valign="top">
            <asp:Label ID="Label2" runat="server" Text="Kategoria:"></asp:Label><br />
            <asp:DropDownList ID="m_CategoryDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_Category_SelectedIndexChanged">
            </asp:DropDownList>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Podkategoria:"></asp:Label><br />
            <asp:DropDownList ID="DropDownList_SubCategory" runat="server" AutoPostBack="True"
              OnSelectedIndexChanged="DropDownList_SubCategory_SelectedIndexChanged">
            </asp:DropDownList>
            <br />
            <asp:Label ID="Label6" runat="server" Text="Status:"></asp:Label><br />
            <asp:DropDownList ID="DropDownList_State" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_State_SelectedIndexChanged">
            </asp:DropDownList>
            <br />
            <asp:Label ID="Label4" runat="server" Text="Projekt:"></asp:Label><br />
            <asp:DropDownList ID="DropDownList_Project" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_Project_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator_Project" runat="server" ControlToValidate="DropDownList_Project"
              ErrorMessage="<%$ Resources:Resources, text_sellect_project%>" InitialValue="<%$ Resources:Resources, text_sellect_project%>"></asp:RequiredFieldValidator><br />
            <asp:Label ID="Label5" runat="server" Text="Rodzaj pracy:"></asp:Label><br />
            <asp:DropDownList ID="DropDownList_TypeOfWork" runat="server" AutoPostBack="True"
              OnSelectedIndexChanged="DropDownList_TypeOfWork_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DropDownList_TypeOfWork"
              ErrorMessage="Wybierz rodzaj pracy" InitialValue="<%$ Resources:Resources, text_sellect_work_type %>"></asp:RequiredFieldValidator><br />
          </td>
          <td style="width: 320px; height: 140px;" valign="top">
            <asp:Label ID="Label7" runat="server" Text="Godziny"></asp:Label><asp:TextBox ID="TextBox1"
              runat="server" Width="44px">0</asp:TextBox><asp:Label ID="Label11" runat="server"
                Text="Minuty"></asp:Label><asp:DropDownList ID="DropDownList_minutes" runat="server">
                  <asp:ListItem Selected="True">0</asp:ListItem>
                  <asp:ListItem>15</asp:ListItem>
                  <asp:ListItem>30</asp:ListItem>
                  <asp:ListItem>45</asp:ListItem>
                </asp:DropDownList>
            <br />
            <br />
            <asp:Label ID="Label9" runat="server" Text="Opis"></asp:Label><br />
            <asp:TextBox ID="TextBox_work_description" runat="server" Height="109px" TextMode="MultiLine"
              Width="270px" Enabled="False">Opis, zakres czynności, numer projektu/oferty itp.</asp:TextBox><br />
            <br />
            <br />
            <asp:Button ID="Button_new" runat="server" OnClick="Button_new_Click" Text="Nowy rekord" />
            &nbsp; &nbsp;
            <asp:Button ID="Button_cancel" runat="server" OnClick="Button_cancel_Click" Text="Anuluj" /><br />
          </td>
        </tr>
      </table>
      &nbsp;&nbsp;<table style="width: 592px; height: 182px;">
        <tr>
          <td style="background-color: #99cccc" valign="top">
            <asp:Label ID="Label16" runat="server" BackColor="White" Text="Aktualnie masz wpisanych 0 godzin"></asp:Label><br />
            <br />
            <asp:Label ID="Label13" runat="server" Text="Godziny"></asp:Label><br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
              BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataKeyNames="ID"
              DataSourceID="ObjectDataSource" ForeColor="Black" GridLines="Vertical" OnRowCommand="GridView1_RowCommand"
              OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
              <Columns>
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
              <FooterStyle BackColor="#CCCC99" />
              <RowStyle BackColor="#F7F7DE" />
              <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
              <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
              <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
              <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <asp:ObjectDataSource ID="ObjectDataSource" runat="server" OldValuesParameterFormatString="original_{0}"
              SelectMethod="GetDataByDate" TypeName="widok.DataSetTableAdapters.GODZINY_PROJEKTYTableAdapter">
              <SelectParameters>
                <asp:ControlParameter ControlID="m_Calendar" DefaultValue="" Name="Data_projektu"
                  PropertyName="SelectedDate" Type="DateTime" />
                <asp:SessionParameter Name="login" SessionField="CurrentUser" Type="String" />
              </SelectParameters>
            </asp:ObjectDataSource>
          </td>
        </tr>
      </table>
    </td>
  </tr>
</table>
