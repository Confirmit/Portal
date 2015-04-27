<%@ Control Language="C#" AutoEventWireup="true" Inherits="DescriptionExtender" Codebehind="DescriptionExtender.ascx.cs" %>

<!-- "Wire frame" div used to transition from the button to the info panel -->
<div id="div_descrFlyout" runat="server" style="display: none; overflow: hidden; z-index: 2; background-color: #FFFFFF; border: solid 1px #D0D0D0;"></div>
        
<!-- Info panel to be displayed as a flyout when the button is clicked -->
<div id="div_descrInfo" runat="server" style="display: none; width: 250px; z-index: 2; opacity: 0; font-size: 12px; border: solid 1px #CCCCCC; background-color: #FFFFFF; padding: 5px;">
    <div id="btn_CloseParent" runat="server" style="float: right; opacity: 0;" >
        <asp:LinkButton id="btnClose" runat="server" OnClientClick="return false;" Text="X" ToolTip="Close"
            Style="background-color: #666666; color: #FFFFFF; text-align: center; font-weight: bold; text-decoration: none; border: outset thin #FFFFFF; padding: 5px;" />
    </div>
    <div>
        <p>
            <asp:Label ID="lblDescript" runat="server" />
        </p>
    </div>
</div>
        
<ajaxToolkit:AnimationExtender id="OpenAnimation" runat="server" >
    <Animations>
        <OnClick>
            <Sequence>
                <%-- Disable the button so it can't be clicked again --%>
                <EnableAction Enabled="false" />
                        
                <%-- Position the wire frame on top of the button and show it --%>
                <ScriptAction Script="doCover('_ControlClientID_', '_FlyoutClientID_');" />
                <StyleAction AnimationTarget="_FlyoutClientID_" Attribute="display" Value="block"/>
                        
                <%-- Move the wire frame from the button's bounds to the info panel's bounds --%>
                <Parallel AnimationTarget="_FlyoutClientID_" Duration=".3" Fps="25">
                    <Move Horizontal="_MoveHorizontal_" Vertical="_MoveVertical_" />
                    <Resize Width="260" Height="80" />
                    <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                </Parallel>
                        
                <%-- Move the info panel on top of the wire frame, fade it in, and hide the frame --%>
                <ScriptAction Script="doCover('_FlyoutClientID_', '_InfoClientID_', true);" />
                <StyleAction AnimationTarget="_InfoClientID_" Attribute="display" Value="block"/>
                <FadeIn AnimationTarget="_InfoClientID_" Duration=".1"/>
                <StyleAction AnimationTarget="_FlyoutClientID_" Attribute="display" Value="none"/>
                                                
                <Parallel AnimationTarget="_InfoClientID_" Duration=".5">
                    <FadeIn AnimationTarget="_BtnCloseClientID_" MaximumOpacity=".9" />
                </Parallel>
            </Sequence>
        </OnClick>
    </Animations>
</ajaxToolkit:AnimationExtender>
        
<ajaxToolkit:AnimationExtender id="CloseAnimation" TargetControlID="btnClose" runat="server" >
    <Animations>
        <OnClick>
            <Sequence AnimationTarget="_InfoClientID_">
                <%--  Shrink the info panel out of view --%>
                <StyleAction Attribute="overflow" Value="hidden"/>
                <Parallel Duration=".3" Fps="15">
                    <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                    <FadeOut />
                </Parallel>
                        
                <%--  Reset the sample so it can be played again --%>
                <StyleAction Attribute="display" Value="none"/>
                <StyleAction Attribute="width" Value="250px"/>
                <StyleAction Attribute="height" Value=""/>
                <StyleAction Attribute="fontSize" Value="12px"/>
                <OpacityAction AnimationTarget="_BtnCloseClientID_" Opacity="0" />
                        
                <%--  Enable the button so it can be played again --%>
                <EnableAction AnimationTarget="_ControlClientID_" Enabled="true" />
            </Sequence>
        </OnClick>
    </Animations>
</ajaxToolkit:AnimationExtender>
