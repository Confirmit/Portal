s<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_UsersList" CodeBehind="UsersList.ascx.cs" %>

<asp:GridView ID="GridUsersList" runat="server" AllowSorting="true"
    AutoGenerateColumns="False" OnSorting="SortingCommand_Click" Width="90%">
    <Columns>
        <asp:BoundField AccessibleHeaderText="ASD" DataField="UserID" HeaderText="User ID:" SortExpression="ID" meta:resourcekey="hUserID">
            <ItemStyle Width="30%" BorderStyle="Solid" BorderWidth="1"/>
        </asp:BoundField>
        <asp:HyperLinkField DataTextField="UserName" DataNavigateUrlFields="UserID" DataNavigateUrlFormatString="../../UsersList/UsersInfo.aspx?UserID={0}" HeaderText="User Name:" 
            SortExpression="LastName" meta:resourcekey="hUser" />
        <asp:BoundField DataField="Status" HeaderText="Status:" meta:resourcekey="hStatus">
            <ItemStyle Width="30%" BorderStyle="Solid" BorderWidth="1"/>
        </asp:BoundField>
    </Columns>
    <RowStyle HorizontalAlign="Center" BorderWidth="1px" />
</asp:GridView>
<asp:Label runat="server" ID="lblException" CssClass="control-errorlabel" Visible="false" />