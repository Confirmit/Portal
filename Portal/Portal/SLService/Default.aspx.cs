using System;
using System.Web;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib;
using UlterSystems.PortalLib.Notification;

/// <summary>
/// Class of main page.
/// </summary>
public partial class Main_SLService : BaseWebPage
{
	#region Fields

	private readonly string sl_cookie_key = "sl_cookie_key";

	#endregion

	#region Properties

	public bool IsUsingSLControl
	{
		get
		{
			if (!Request.Browser.Cookies)
				return true;

			HttpCookie cookie = Request.Cookies.Get(sl_cookie_key);
			return cookie == null
					   ? true
					   : bool.Parse(cookie.Value);
		}
		set
		{
			if (!Request.Browser.Cookies)
				return;

			HttpCookie mycook = new HttpCookie(sl_cookie_key)
									{
										Value = value.ToString()
									};
			Response.Cookies.Set(mycook);
		}
	}

	#endregion

	#region Event handlers

	/// <summary>
	/// Handles of page load.
	/// </summary>
	protected void Page_Load(object sender, EventArgs e)
	{
		locNotRegistered.Visible = CurrentUser == null;
		locNotEmployee.Visible = CurrentUser != null && !CurrentUser.IsInRole(RolesEnum.Employee);
		//slDayInfo.Visible = slEventsControl.Visible = CurrentUser != null && CurrentUser.IsInRole(RolesEnum.Employee);

		btnSend.Visible = phReportForm.Visible = CurrentUser != null && !string.IsNullOrEmpty(CurrentUser.PrimaryEMail);
		phReportUnavaliable.Visible = CurrentUser == null || string.IsNullOrEmpty(CurrentUser.PrimaryEMail);

		if (CurrentUser != null && CurrentUser.IsInRole(RolesEnum.Employee))
		{
			setSilverlightInputString();
			btnChangeSkin.Click += OnChangeSkin_Click;
		}
	}

	private void setSilverlightInputString()
	{
		//slEventsControl.InitParameters += String.Format(",UserID={0},Culture={1}",
		//                                      CurrentUser.ID.Value,
		//                                      Thread.CurrentThread.CurrentCulture.Name);

		//slDayInfo.InitParameters += String.Format(",UserID={0},Culture={1}",
		//                                          CurrentUser.ID.Value,
		//                                          Thread.CurrentThread.CurrentCulture.Name);
	}

	/// <summary>
	/// Handles sending of report.
	/// </summary>
	protected void OnSendReport(object sender, EventArgs e)
	{
		if (!IsValid)
			return;

		if (SendReport())
		{
			cpeReport.Collapsed = true;
			RedirectToMySelf();
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

	/// <summary>
	/// Sends report to PM.
	/// </summary>
	/// <returns>Was the report sended.</returns>
	protected bool SendReport()
	{
		try
		{
			if (string.IsNullOrEmpty(tbTo.Text)
				|| string.IsNullOrEmpty(tbSubject.Text)
				|| string.IsNullOrEmpty(tbMessage.Text)
				|| !Regex.IsMatch(tbTo.Text, valToRegex.ValidationExpression))
				return false;

			Debug.Assert( CurrentUser != null );
			if (CurrentUser.ID != null && !string.IsNullOrEmpty(CurrentUser.PrimaryEMail))
			{
				MailItem item = new MailItem
									{
										FromAddress = CurrentUser.PrimaryEMail,
										ToAddress = tbTo.Text,
										Subject = tbSubject.Text,
										Body = tbMessage.Text,
										MessageType = ((int) MailTypes.UserReport)
									};
				item.Save();
			}

			return true;
		}
		catch( Exception ex )
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
			return false;
		}
	}

	protected bool IsUsingSilverlightControl()
	{
		return IsUsingSLControl;
	}

	private void OnChangeSkin_Click(object sender, System.Web.UI.ImageClickEventArgs e)
	{
		IsUsingSLControl = !IsUsingSLControl;
		RedirectToMySelf();
	}

	#endregion
}
