<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_UsersList" CodeBehind="UsersList.ascx.cs" %>

<asp:GridView ID="GridUsersList" runat="server" CssClass="griduserlist" AllowSorting="true"
    AutoGenerateColumns="False" OnSorting="SortingCommand_Click"
    OnRowDataBound="OnUserInfoBound" Width="90%">
    <Columns>
        <asp:BoundField DataField="UserID" HeaderText="User ID:" SortExpression="ID" meta:resourcekey="hUserID">
            <ItemStyle BorderStyle="Solid" BorderWidth="1" />
        </asp:BoundField>
        <asp:HyperLinkField DataTextField="UserName" DataNavigateUrlFields="UserID" DataNavigateUrlFormatString="../../UsersList/UsersInfo.aspx?UserID={0}" HeaderText="User Name:"
            SortExpression="LastName" meta:resourcekey="hUser" />
        <asp:BoundField DataField="Status" HeaderText="Status:" meta:resourcekey="hStatus">
            <ItemStyle BorderStyle="Solid" BorderWidth="1" />
        </asp:BoundField>
        <asp:TemplateField meta:resourcekey="hOperations">
            <ItemTemplate>
                <asp:ImageButton ID="btnIll" runat="server"
                    meta:resourcekey="btnIll"
                    OnClick="OnSetIll"
                    ImageUrl="~/Images/Ill.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="btnTrustIll" runat="server"
                    meta:resourcekey="btnTrustIll"
                    OnClick="OnSetTrustIll"
                    ImageUrl="~/Images/TrustIll.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="btnBusinessTrip" runat="server"
                    meta:resourcekey="btnBusinessTrip"
                    OnClick="OnSetBusinessTrip"
                    ImageUrl="~/Images/BusinessTrip.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="btnVacation" runat="server"
                    meta:resourcekey="btnVacation"
                    OnClick="OnSetVacation"
                    ImageUrl="~/Images/Vacation.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="btnLesson" runat="server"
                    meta:resourcekey="btnLesson"
                    OnClick="OnSetLesson"
                    ImageUrl="~/Images/lesson.png"
                    Height="17" Width="17" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <RowStyle HorizontalAlign="Center" BorderWidth="1px" BorderStyle="Solid" />
</asp:GridView>
<asp:Label runat="server" ID="lblException" CssClass="control-errorlabel" Visible="false" />