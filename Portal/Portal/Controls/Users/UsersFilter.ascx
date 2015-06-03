<%@ Control Language="C#" AutoEventWireup="true" Inherits="UsersFilter" Codebehind="UsersFilter.ascx.cs" %>

<div style="width: 100%;" align="left" class="control">
    <asp:Panel ID="pnlFilterHeader" runat="server" Width="100%"> 
        <div class="control-header">
            <div class="control-header-title">
                <asp:Label ID="lblUserSearch" runat="server" meta:resourcekey="UserSearch"/>
            </div>
            <div class="control-header-buttons">
                <asp:ImageButton ID="imgCollapsedStatus" runat="server" ImageUrl="~/Images/expand.png" AlternateText="Show"/>
            </div>            
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlFilterContent" runat="server">
        <table border="0">
            <tr>
                <td width="100">
                    <asp:Label ID="lblLastName" runat="server" meta:resourcekey="LastName" CssClass="control-label" /> 
                </td>
                <td>
                    <asp:TextBox ID="tbxLastName" runat="server" CssClass="control-textbox" Width="200" EnableTheming="false" />
                </td>
                <td width="10"></td>
                <td width="100">
                    <asp:Label ID="lblEvents" runat="server" meta:resourcekey="Events" CssClass="control-label" />  
                </td>
                <td rowspan="5">
                    <div class="control-border" style="float: left; height: 150px; width: 200px; overflow:  auto;">
                        <asp:CheckBoxList ID="cblEvents" runat="server" 
                            CssClass="control-label"  
                            DataTextField="Name" DataValueField="ID" />
                    </div>
                </td>
            </tr>
            <tr>     
                <td>
                    <asp:Label ID="lblFirstName" runat="server" meta:resourcekey="FirstName" CssClass="control-label" />
                </td>
                <td>
                    <asp:TextBox ID="tbxFirstName" runat="server" CssClass="control-textbox" Width="200" EnableTheming="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblOfficeName" runat="server" meta:resourcekey="Office" CssClass="control-label" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlOfficeName" runat="server" DataValueField="OfficeName" 
                        CssClass="control-dropdownlist" Width="200" >
                        <asp:ListItem Value="0" meta:resourcekey="Anyone"/>
                        <asp:ListItem Value="1" meta:resourcekey="InYaroslavl"/>
                        <asp:ListItem Value="2" meta:resourcekey="InMoscow"/>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRole" runat="server" meta:resourcekey="Role" CssClass="control-label" /> 
                </td>
                <td>
                    <asp:DropDownList ID="ddlRole" runat="server" AppendDataBoundItems="True" 
                        DataTextField="Name" DataValueField="ID" 
                        Width="200" CssClass="control-dropdownlist"
                        >
                        <asp:ListItem Value="-1" meta:resourcekey="Anyone"/> 
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblProject" runat="server" meta:resourcekey="Project" CssClass="control-label" Visible="false" />  
                </td>
                <td>
                    <asp:DropDownList ID="ddlProject" runat="server" 
                        AppendDataBoundItems="True"
                        CssClass="control-dropdownlist" 
                        DataTextField="Name" DataValueField="ID" Width="200"
                        Visible="false"
                        >
                            <asp:ListItem Value="-1" meta:resourcekey="Anyone"/>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4"></td>
                <td align="right" >
                    <div style="float: left; margin: 5px;">
                        <asp:Button ID="btnResetFilter" runat="server" 
                            Width="100" Text="Reset"
                            CssClass="control-button" />
                    </div>
                    <div style="float: left; margin: 5px;">
                        <asp:Button ID="btnSearch" runat="server" 
                            Width="100" meta:resourcekey="Search" 
                            CssClass="control-button" />
                    </div>
                </td>
                <td></td>
            </tr>
         </table>
        <div class="control-line-between"></div>
    </asp:Panel>
</div> 

<ajaxToolkit:CollapsiblePanelExtender ID="cpeFilter" runat="server" 
    TargetControlID="pnlFilterContent"
    ExpandControlID="pnlFilterHeader" 
    CollapseControlID="pnlFilterHeader" 
    ImageControlID="imgCollapsedStatus" 
    Collapsed="true"
    ExpandedImage="~/Images/collapse.png" 
    CollapsedImage="~/Images/expand.png"
    SuppressPostBack="true" /> 