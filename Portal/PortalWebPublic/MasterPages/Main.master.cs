using System;

public partial class MasterPages_Main : System.Web.UI.MasterPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "main", ResolveClientUrl("~/Scripts/Main.js"));
	}
}
