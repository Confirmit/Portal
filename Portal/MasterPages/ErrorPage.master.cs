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

    public override ScriptManager ScriptManager
    {
        get { return scriptManager; }
    }
}

