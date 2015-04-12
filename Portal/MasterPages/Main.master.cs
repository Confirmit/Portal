using System.Web;

public partial class MasterPages_Main : MasterPages_Base
{
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
}