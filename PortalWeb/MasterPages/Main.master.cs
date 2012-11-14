using System;
using System.Web.UI;

public partial class MasterPages_Main : BaseMasterPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		Page.ClientScript.RegisterClientScriptInclude(GetType(), "main", ResolveClientUrl("~/Scripts/Main.js"));
	}

    public override ScriptManager ScriptManager
    {
        get { return scriptManager; }
    }
}