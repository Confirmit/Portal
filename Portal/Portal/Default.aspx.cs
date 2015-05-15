using System;
using System.Web.UI;
using ConfirmIt.PortalLib.BAL;

/// <summary>
/// Class of main page.
/// </summary>
public partial class Main : BaseWebPage
{
	/// <summary>
	/// Handles of page load.
	/// </summary>
	protected void Page_Load(object sender, EventArgs e)
	{
		// Set visibility of time management control.
		if (CurrentUser == null)
		{
			localizeNotRegistered.Visible = true;
			localizzeNotEmployee.Visible = false;
		}
		else
		{
			if (!CurrentUser.IsInRole(RolesEnum.Employee))
			{
				localizeNotRegistered.Visible = false;
				localizzeNotEmployee.Visible = true;
			}
			else
			{
				localizeNotRegistered.Visible = false;
				localizzeNotEmployee.Visible = false;
			}
		}
	}
	
}