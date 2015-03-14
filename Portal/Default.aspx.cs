using System;
using System.Threading;

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
		//btnChangeSkin.Click += OnChangeSkin_Click;

		// Set visibility of time management control.
		if (CurrentUser == null)
		{
			locNotRegistered.Visible = true;
			locNotEmployee.Visible = false;
		}
		else
		{
			if (!CurrentUser.IsInRole(RolesEnum.Employee))
			{
				locNotRegistered.Visible = false;
				locNotEmployee.Visible = true;
			}
			else
			{
				locNotRegistered.Visible = false;
				locNotEmployee.Visible = false;
			}
		}
	}
	
}