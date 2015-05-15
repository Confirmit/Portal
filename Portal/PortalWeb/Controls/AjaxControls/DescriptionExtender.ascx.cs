using System;
using System.Diagnostics;
using System.Web.UI;

public partial class DescriptionExtender : UserControl
{
    #region Fields

    private int m_moveHorizontal = 40;
    private int m_moveVertical = 10;

    #endregion

    protected override void  OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        Control control = Parent.FindControl(OpenAnimation.TargetControlID);
        if (control == null)
            Debug.Assert(false, "Could not found target control.");

        OpenAnimation.Animations = OpenAnimation.Animations.Replace("_ControlClientID_", control.ClientID);
        OpenAnimation.Animations = OpenAnimation.Animations.Replace("_FlyoutClientID_", div_descrFlyout.ClientID);
        OpenAnimation.Animations = OpenAnimation.Animations.Replace("_InfoClientID_", div_descrInfo.ClientID);
        OpenAnimation.Animations = OpenAnimation.Animations.Replace("_BtnCloseClientID_", btn_CloseParent.ClientID);

        OpenAnimation.Animations = OpenAnimation.Animations.Replace("_MoveHorizontal_", m_moveHorizontal.ToString());
        OpenAnimation.Animations = OpenAnimation.Animations.Replace("_MoveVertical_", m_moveVertical.ToString());

        CloseAnimation.Animations = CloseAnimation.Animations.Replace("_ControlClientID_", control.ClientID);
        CloseAnimation.Animations = CloseAnimation.Animations.Replace("_FlyoutClientID_", div_descrFlyout.ClientID);
        CloseAnimation.Animations = CloseAnimation.Animations.Replace("_InfoClientID_", div_descrInfo.ClientID);
        CloseAnimation.Animations = CloseAnimation.Animations.Replace("_BtnCloseClientID_", btn_CloseParent.ClientID);
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (!(Page.ClientScript.IsClientScriptIncludeRegistered("DescriptionExtender")))
            Page.ClientScript.RegisterClientScriptInclude(GetType(), "DescriptionExtender"
                                                          , "../Controls/AjaxControls/DescriptionExtender.js");
    }

    #region Properties 

    /// <summary>
    /// Pixels to move frame description in horizontal direction.
    /// </summary>
    public int MoveHorizontal
    {
        set { m_moveHorizontal = value; }
    }

    /// <summary>
    /// Pixels to move frame description in vertical direction.
    /// </summary>
    public int MoveVertical
    {
        set { m_moveVertical = value; }
    }

    /// <summary>
    /// Target control ID for animation.
    /// </summary>
    public String TargetControlID
    {
        set { OpenAnimation.TargetControlID = value; }
    }
    
    /// <summary>
    /// Text to show
    /// </summary>
    public String DesriptionText
    {
        set { lblDescript.Text = value; }
    }

    #endregion
}
