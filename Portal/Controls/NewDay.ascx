<%@ Control Language="c#" Inherits="NewDay" Codebehind="NewDay.ascx.cs" %>

<div class="control">
    <table>
	    <tr>
		    <td align="center">
			    <asp:Button ID="btWork" runat="server" CssClass="control-button" 
			        Text="Work" Width="128px" Visible="false" 
			        OnClick="OnWork_Click" meta:resourcekey="btnWorkBegin"
			    />
		    </td>
	    </tr>
	    <tr>
		    <td align="center">
			    <asp:Button ID="btTime" runat="server" CssClass="control-button" 
			        Text="Time" Width="128px" Visible="false" 
			        OnClick="OnTime_Click" meta:resourcekey="btnTimeOn" 
			    />
		    </td>
	    </tr>
        <tr>
            <td>
                <asp:Label ID="lblTime" runat="server" 
                     CssClass="control-label" meta:resourcekey="lblTime"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblRemainToday" runat="server" 
                    CssClass="control-label" meta:resourcekey="lblRemainToday" />
                <asp:Label ID="lblEndDay" runat="server" 
                    meta:resourcekey="lblEndDay" CssClass="control-label" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblRemainWeek" runat="server" 
                    meta:resourcekey="lblRemainWeek" CssClass="control-label" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblRemainMonth" runat="server" 
                    meta:resourcekey="lblRemainMonth" CssClass="control-label" />
            </td>
        </tr>
    </table>    

    <div class="control-line-between"></div>
    <div class="control" style="width: 500px;">
        <asp:GridView ID="gridViewUserDayEvents" runat="server" 
            AutoGenerateColumns="False"
            CssClass="gridview" AllowPaging="true"
            PageSize="5" Width="100%" GridLines="None"
            EnableTheming="false"
            >
            <HeaderStyle CssClass="gridview-headerrow" HorizontalAlign="Left" />
            <RowStyle CssClass="gridview-row" Height="20" />
            <AlternatingRowStyle CssClass="gridview-alternatingrow" />
            
            <EmptyDataTemplate>
                <center>
                    No work events..
                </center>
            </EmptyDataTemplate>

            <Columns>
                <asp:TemplateField HeaderText="Begin" ItemStyle-HorizontalAlign="Center" meta:resourcekey="hBegin">
                    <ItemTemplate>
                        <asp:Label ID="lblBeginTime" runat="server" Text='<%# ((DateTime)DataBinder.Eval(Container, "DataItem.BeginTime")).ToShortTimeString() %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            
                <asp:TemplateField HeaderText="End" ItemStyle-HorizontalAlign="Center" meta:resourcekey="hEnd">
                    <ItemTemplate>
                        <asp:Label ID="lblEndTime" runat="server" Text='<%# ((DateTime)DataBinder.Eval(Container, "DataItem.EndTime")).ToShortTimeString().Trim().Replace("0:00", "") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            
                <asp:TemplateField HeaderText="Period" ItemStyle-HorizontalAlign="Center" meta:resourcekey="hPeriod">
                    <ItemTemplate>
                        <asp:Label ID="lblPeriod" runat="server" Text='<%# new DateTime(((TimeSpan)DataBinder.Eval(Container, "DataItem.Duration")).Ticks).ToLongTimeString()  %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            
                <asp:TemplateField HeaderText="Event Type"  meta:resourcekey="hEventType">
                    <ItemTemplate>
                        <asp:Label ID="lblEventType" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.EventType")).ToString() %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    
    <div class="control-line-between"></div>
</div>    
