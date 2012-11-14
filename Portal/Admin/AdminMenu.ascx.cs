using System;

public partial class Admin_AdminMenul : BaseUserControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
        hlEvents.Text = (string)GetLocalResourceObject("hlEvents.Text");
        hlStatistics.Text = (string)GetLocalResourceObject("hlStatistics.Text");
        hlUsers.Text = (string)GetLocalResourceObject("hlUsers.Text");
	}
}
