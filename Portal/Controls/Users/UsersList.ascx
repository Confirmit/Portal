<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_UsersList" CodeBehind="UsersList.ascx.cs" %>

<asp:ObjectDataSource ID="UserNamesAndStatusesObjectDataSource" runat="server"
    TypeName="Controls_UsersList">
</asp:ObjectDataSource>

<asp:GridView ID="GridUsersList" runat="server" AllowSorting="true"
    AutoGenerateColumns="False" OnSorting="SortingCommand_Click" Width="70%">
    <PagerStyle CssClass="gridview-pagerrow" />
    <Columns>
        <asp:BoundField DataField="UserID" HeaderText="User ID:" SortExpression="ID" meta:resourcekey="hUserID"/>
        <asp:BoundField DataField="UserName" HeaderText="User Name:" SortExpression="LastName"  meta:resourcekey="hUser" />
        <asp:BoundField DataField="Status" HeaderText="Status:" meta:resourcekey="hStatus"/>
    </Columns>
    <AlternatingRowStyle CssClass="gridview-alternatingrow" />
    <RowStyle HorizontalAlign="Center" />
</asp:GridView>
<asp:Label runat="server" ID="lblException" CssClass="control-errorlabel" Visible="false" />