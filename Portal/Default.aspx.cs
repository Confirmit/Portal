using System;
using System.Web.UI;
using ConfirmIt.PortalLib.BAL;

/// <summary>
/// Class of main page.
/// </summary>
public partial class Main : BaseWebPage
{
	#region Event handlers

	/// <summary>
	/// Handles of page load.
	/// </summary>
	protected void Page_Load(object sender, EventArgs e)
	{
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
    
	/// <summary>
	/// Handles finishing of work.
	/// </summary>
	protected void OnWorkFinish( object sender, EventArgs e )
	{
		// SendReport();
	}

	#endregion

	#region Methods

	protected bool IsUsingSilverlightControl()
	{
		return CookiesHelper.IsUsingSLControl;
	}

	private void OnChangeSkin_Click(object sender, ImageClickEventArgs e)
	{
		CookiesHelper.IsUsingSLControl = !CookiesHelper.IsUsingSLControl;
		RedirectToMySelf();
	}

	#endregion
}