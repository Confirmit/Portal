<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_UsersList" CodeBehind="UsersList.ascx.cs" %>

<asp:GridView ID="grdUsersList" runat="server"
    AllowSorting="true">
    <Columns>
        <asp:BoundField DataField="EmployeeID" HeaderText="Employee ID" ReadOnly="true" />
        <asp:BoundField DataField="FirstName" HeaderText="First Name" />
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

        <%--http://www.aarongoldenthal.com/post/2009/04/19/manually-databinding-a-gridview.aspx--%>
        <%--http://www.tomot.de/en-us/article/7/asp.net/gridview-overview-of-different-ways-to-bind-data-to-columns--%>
        
        

        <%--<asp:TemplateField HeaderText="Last Name" SortExpression="LastName">
             <ItemTemplate>Text='<%# (string)DataBinder.Eval(Container.DataItem, "UserName") %>'</ItemTemplate>
         </asp:TemplateField>
         <asp:TemplateField HeaderText="Last Name" SortExpression="LastName">
              <ItemTemplate>Text='<%# (string)DataBinder.Eval(Container.DataItem, "Status") %>'</ItemTemplate>
         </asp:TemplateField>
         <asp:TemplateField HeaderText="Last Name" SortExpression="LastName">
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
         </asp:TemplateField>--%>
    </Columns>
    <AlternatingRowStyle CssClass="gridview-alternatingrow" />
    <PagerStyle CssClass="gridview-row" />
</asp:GridView>

<%--<asp:DataGrid ID="grdUsersList" runat="server"
    DataKeyField="UserID"
    AutoGenerateColumns="false"
    OnItemDataBound="OnUserInfoBound"
    AllowSorting="true"
    OnSortCommand="SortingCommand_Click"
    GridLines="None">
    <ItemStyle Width="5px" />
    <Columns>
        <asp:TemplateColumn HeaderText="User" ItemStyle-HorizontalAlign="Left" SortExpression="UserNameSorting" meta:resourcekey="hUser">
            <ItemTemplate>
                <b>
                    <asp:HyperLink ID="hlUserName" runat="server"
                        CssClass="control-hyperlink-big"
                        NavigateUrl="~/Admin/AdminUserInfo.aspx"
                        Text='<%# (string)DataBinder.Eval(Container.DataItem, "UserName") %>' />
                </b>
            </ItemTemplate>
            <HeaderStyle Width="30%" HorizontalAlign="Center" Font-Bold="true" />
        </asp:TemplateColumn>

        <asp:TemplateColumn HeaderText="Status" ItemStyle-HorizontalAlign="Center" SortExpression="StatusSorting"  meta:resourcekey="hStatus">
            <ItemTemplate>
                <asp:Label ID="lUserStatus" runat="server"
                    ForeColor='<%# (System.Drawing.Color)DataBinder.Eval(Container.DataItem, "EventType.Color")%>' Text='<%# (string)DataBinder.Eval(Container.DataItem, "Status") %>' />
            </ItemTemplate>
            <HeaderStyle Width="40%" HorizontalAlign="Center" Font-Bold="true" />
        </asp:TemplateColumn>

        <asp:TemplateColumn HeaderText="Operations"
            ItemStyle-HorizontalAlign="Right"
            meta:resourcekey="hOperations">
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
            <HeaderStyle Width="30%" HorizontalAlign="Center" Font-Bold="true" />
        </asp:TemplateColumn>
    </Columns>
    <ItemStyle CssClass="gridview-row" />
    <AlternatingItemStyle CssClass="gridview-alternatingrow" />
</asp:DataGrid>--%>

<asp:Label runat="server" ID="lblException" CssClass="control-errorlabel" Visible="false" />