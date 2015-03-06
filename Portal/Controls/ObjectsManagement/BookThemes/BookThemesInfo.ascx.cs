using System;

using Core;
using UIPProcess.DataBinding;
using UIPProcess.DataValidating;
using ConfirmIt.PortalLib.BAL;

public partial class BookThemesInfo : BaseUserControl
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        ActionsMenuClientId = menuActions.ClientID;
        menuActions.ScriptManager = ((BaseMasterPage)Page.MasterPage).ScriptManager;

        reqName.Visible = false; 
    }

    #region DataBinding and DataValidating

    [DataBinding("Name", null, false)]
    public MLText ThemeName
    {
        set { mltbThemeName.MultilingualText = value; }
        get { return mltbThemeName.MultilingualText; }
    }

    [DataValidating]
    public bool ValidateUserRight()
    {
        return Page.CurrentUser.IsInRole(RolesEnum.Administrator);
    }

    [DataValidating]
    public bool ValidateThemeName()
    {
        reqName.Visible = string.IsNullOrEmpty(mltbThemeName.MultilingualText.ToString());
        return !reqName.Visible;
    }

    [DataValidating(true)]
    public void ClearValidateThemeName()
    {
        reqName.Visible = false;
    }

    #endregion
}