using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPages_Error : BaseMasterPage
{
    #region Properties

    public new BaseWebPage Page
    {
        get { return (BaseWebPage) base.Page; }
        set { base.Page = value; }
    }

    #endregion

    #region Life Cycle

    protected override void OnInit(System.EventArgs e)
    {
        if (Page.CurrentUser != null)
        {
            if (string.IsNullOrEmpty(Page.CurrentUser.FirstName.ToString()))
                lUserName.Text = Page.CurrentUser.LastName.ToString();
            else
                lUserName.Text = Page.CurrentUser.FirstName.ToString() + " " + Page.CurrentUser.LastName.ToString();
        }

        base.OnInit(e);
    }

    #endregion

    public override ScriptManager ScriptManager
    {
        get { return scriptManager; }
    }
}

