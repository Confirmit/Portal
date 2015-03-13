<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_UsersList" CodeBehind="UsersList.ascx.cs" %>

<asp:ObjectDataSource ID="UserNamesAndStatusesObjectDataSource" runat="server"
            TypeName="Controls_UsersList" SelectMethod="GetUsersStatusInfo" />

<asp:GridView ID="grdUsersList" runat="server" AllowSorting="true" 
    DataSourceID="UserNamesAndStatusesObjectDataSource" AutoGenerateColumns="True">
    <Columns>
         <asp:BoundField DataField="UserName" HeaderText="User Name:"/>
         <asp:BoundField DataField="Status" HeaderText="Status:"/>
         <asp:TemplateField HeaderText="Userprofile">
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
                <asp:LinkButton ID="lbtnEdit" runat="server"
                    CssClass="control-hyperlink-big"
                    meta:resourcekey="lbtnEdit" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <AlternatingRowStyle CssClass="gridview-alternatingrow" />
    <RowStyle HorizontalAlign="Center" />
</asp:GridView>
<asp:Label runat="server" ID="lblException" CssClass="control-errorlabel" Visible="false" />