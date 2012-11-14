using System;
using ConfirmIt.PortalLib.BAL;

/// <summary>
/// Page for managing of work time.
/// </summary>
public partial class Admin_AdminCalendarPage : BaseWebPage
{
	#region Properties

	/// <summary>
	/// Calendar item for selected date.
	/// </summary>
	public CalendarItem CalendarItem
	{
		get 
		{
			return
				ConfirmIt.PortalLib.BAL.CalendarItem.GetCalendarItem(
					calendar.SelectedDate.Date );
		}
	}

	#endregion

	#region Event handlers

	/// <summary>
	/// Handles page loading.
	/// </summary>
	protected void Page_Load( object sender, EventArgs e )
	{
		if( !IsPostBack )
		{
			calendar.SelectedDate = DateTime.Today;
			ShowCalendarItem( calendar, EventArgs.Empty );
		}
	}

	/// <summary>
	/// Shows calendar work time information.
	/// </summary>
	protected void ShowCalendarItem( object sender, EventArgs args )
	{
		tbxWorkTime.Text = CalendarItem.WorkTime.ToString();
		tbxComment.Text = CalendarItem.Comment;
		btnDelete.Enabled = CalendarItem.IsSaved;
	}

	/// <summary>
	/// Saves information about work time to database.
	/// </summary>
	protected void ApplyCalendarItem( object sender, EventArgs e )
	{
		TimeSpan workTime;
		if( TimeSpan.TryParse( tbxWorkTime.Text, out workTime ) )
		{
			CalendarItem.WorkTime = workTime;
			CalendarItem.Comment = tbxComment.Text;
			CalendarItem.Save();
			ShowCalendarItem( calendar, EventArgs.Empty );
		}
	}

	/// <summary>
	/// Deletes calendar item.
	/// </summary>
	protected void DeleteCalendarItem( object sender, EventArgs e )
	{
		if( CalendarItem.IsSaved )
		{
			CalendarItem.Delete();
			ShowCalendarItem( calendar, EventArgs.Empty );
		}
	}

	#endregion
}
