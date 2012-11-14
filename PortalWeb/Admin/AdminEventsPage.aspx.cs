using System;

public partial class Admin_AdminPage : BaseWebPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			Calendar.SelectedDate = DateTime.Today;
			ReportDateChanged(this, EventArgs.Empty);
		}
	}

	protected void ReportDateChanged(object sender, EventArgs e)
	{
		usersList.Date = Calendar.SelectedDate;
	}
}
