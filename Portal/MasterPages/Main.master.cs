using System.Web;
using System.Web.UI;

public partial class MasterPages_Main : BaseMasterPage
{
	#region Properties

	public new BaseWebPage Page
	{
		get
		{
			return (BaseWebPage)base.Page;
		}
		set
		{
			base.Page = value;
		}
	}

	#endregion
	
	#region Life Cycle

	protected override void OnInit(System.EventArgs e)
	{
		if (Page.CurrentUser != null)
		{
            if (!Page.CurrentUser.IsAuthenticated)
                throw new HttpException(423, "User not found in the database!");

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