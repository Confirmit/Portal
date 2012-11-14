using System;
using System.Collections;

public partial class Controls_TopMenuItem : BaseUserControl
{
	#region Properties
	/// <summary>
	/// Target URL.
	/// </summary>
	public string NavigateUrl
	{
		get { return hlLink.NavigateUrl; }
		set { hlLink.NavigateUrl = value; }
	}

	/// <summary>
	/// Tooltip text.
	/// </summary>
	public string Tooltip
	{
		get { return hlLink.ToolTip; }
		set { hlLink.ToolTip = value; }
	}

	/// <summary>
	/// Text of item.
	/// </summary>
	public string Text
	{
		get { return hlLink.Text; }
		set { hlLink.Text = value; }
	}

	/// <summary>
	/// Is this item for admins only.
	/// </summary>
	public IList AllowedRoles
	{
		get
		{
			if (ViewState["AllowedRoles"] == null)
			{ return null; }
			else
			{ return (IList)ViewState["AllowedRoles"]; }
		}
		set
		{
			ViewState["AllowedRoles"] = value;

			// Correct visibility of control.
			if( Page.CurrentUser == null )
			{ pnlMenuItem.Visible = false; }
			else if( (AllowedRoles == null) || (AllowedRoles.Count == 0) )
			{ pnlMenuItem.Visible = true; }
			else
			{
				pnlMenuItem.Visible = false;
				foreach (object allowedRole in AllowedRoles)
				{
					if (Page.CurrentUser.IsInRole(allowedRole.ToString()))
					{
						pnlMenuItem.Visible = true;
						break;
					}
				}
			}
		}
	}
	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{
	}
}
