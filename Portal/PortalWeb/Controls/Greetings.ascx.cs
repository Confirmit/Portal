using System;

public partial class Controls_Greetings : BaseUserControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if( Page.CurrentUser != null )
		{
			if( string.IsNullOrEmpty( Page.CurrentUser.FirstName.ToString() ) )
				locName.Text = Page.CurrentUser.LastName.ToString();
			else
				locName.Text = Page.CurrentUser.FirstName.ToString() + " " + Page.CurrentUser.LastName.ToString();
		}
	}
}
