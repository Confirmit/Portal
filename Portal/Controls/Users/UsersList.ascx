<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_UsersList" CodeBehind="UsersList.ascx.cs" %>

<asp:GridView ID="GridUsersList" runat="server" AllowSorting="true"
    AutoGenerateColumns="False" OnSorting="SortingCommand_Click" Width="90%" OnRowDataBound="OnRowDataBoundHandler">
    <Columns>
        <asp:BoundField DataField="UserID" HeaderText="User ID:" SortExpression="ID" meta:resourcekey="hUserID">
            <ItemStyle Width="30%" BorderStyle="Solid" BorderWidth="1"/>
        </asp:BoundField>
        <asp:HyperLinkField DataTextField="UserName" DataNavigateUrlFields="UserID" DataNavigateUrlFormatString="../../UsersList/UsersInfo.aspx?UserID={0}" HeaderText="User Name:" 
            SortExpression="LastName" meta:resourcekey="hUser" />
        <asp:BoundField DataField="Status" HeaderText="Status:" meta:resourcekey="hStatus">
            <ItemStyle Width="30%" BorderStyle="Solid" BorderWidth="1"/>
        </asp:BoundField>
        <asp:TemplateField meta:resourcekey="hOperations">
            <ItemTemplate>
                <asp:ImageButton ID="IllImageButton" runat="server"
                    meta:resourcekey="btnIll"
                    OnClick="OnSetIll"
                    ImageUrl="~/Images/Ill.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="TrustIllImageButton" runat="server"
                    meta:resourcekey="btnTrustIll"
                    OnClick="OnSetTrustIll"
                    ImageUrl="~/Images/TrustIll.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="BusinessTripImageButton" runat="server"
                    meta:resourcekey="btnBusinessTrip"
                    OnClick="OnSetBusinessTrip"
                    ImageUrl="~/Images/BusinessTrip.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="VacationImageButton" runat="server"
                    meta:resourcekey="btnVacation"
                    OnClick="OnSetVacation"
                    ImageUrl="~/Images/Vacation.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="LessonImageButton" runat="server"
                    meta:resourcekey="btnLesson"
                    OnClick="OnSetLesson"
                    ImageUrl="~/Images/lesson.png"
                    Height="17" Width="17" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <RowStyle HorizontalAlign="Center" BorderWidth="1px" />
</asp:GridView>
<asp:Label runat="server" ID="ExceptionLabel" CssClass="control-errorlabel" Visible="false" />