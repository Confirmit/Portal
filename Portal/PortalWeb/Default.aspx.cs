using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib;
using UlterSystems.PortalLib.Notification;

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
        setSilverlightInputString();
        btnChangeSkin.Click += OnChangeSkin_Click;

        // Set visibility of time management control.
        if (CurrentUser == null)
        {
            locNotRegistered.Visible = true;
            locNotEmployee.Visible = false;

            btnSend.Visible = false;
            phReportForm.Visible = false;
            phReportUnavaliable.Visible = true;
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

            // Set visibility of report controls.
            if (string.IsNullOrEmpty(CurrentUser.PrimaryEMail))
            {
                btnSend.Visible = false;
                phReportForm.Visible = false;
                phReportUnavaliable.Visible = true;
            }
            else
            {
                btnSend.Visible = true;
                phReportForm.Visible = true;
                phReportUnavaliable.Visible = false;
            }
        }
    }

    private void setSilverlightInputString()
    {
        slEventsControl.InitParameters += String.Format(",UserID={0},Culture={1}",
                                              CurrentUser.ID.Value,
                                              Thread.CurrentThread.CurrentCulture.Name);

        slDayInfo.InitParameters += String.Format(",UserID={0},Culture={1}",
                                                  CurrentUser.ID.Value,
                                                  Thread.CurrentThread.CurrentCulture.Name);
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
    	return CookiesHelper.IsUsingSLControl;
    }

    private void OnChangeSkin_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
		CookiesHelper.IsUsingSLControl = !CookiesHelper.IsUsingSLControl;
        RedirectToMySelf();
    }

	#endregion
}