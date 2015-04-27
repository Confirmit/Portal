using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.Arrangements;
using ConfirmIt.PortalLib.BAL;
using Office = UlterSystems.PortalLib.BusinessObjects.Office;


public partial class Arrangements_AddEditArrangement : BaseWebPage
{
	#region Fields
	private XMLSerializableArrangement m_currentArrangement;
	#endregion

	#region Properties
	/// <summary>
	/// Current arrangement
	/// </summary>
	virtual public XMLSerializableArrangement CurrentArrangement
	{
		get
		{
			if (m_currentArrangement == null)
			{
				m_currentArrangement = new XMLSerializableArrangement();
				if (ArrangementID.HasValue)
				{
					try
					{
						// Create service wrapper.
						Office of = new Office();
						of.Load(OfficeID.Value);
						//using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", CurrentUser.ID))
						//{
						//    m_currentArrangement = c.GetArrangement(ArrangementID.Value);
						//}
					}
					catch (Exception ex)
					{ 
						ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
					}
				}

			}
			return m_currentArrangement;
		}
		set
		{
			m_currentArrangement = value;
		}
	}
	/// <summary>
	/// ID of current arrangement.
	/// </summary>
	virtual public int? ArrangementID
	{
		get
		{
			int value;
			if (Int32.TryParse(Request.Params["id"], out value))
			{
				return value;
			}
			else
			{

				return null;
			}
		}
	}
	/// <summary>
	/// ID of current office.
	/// </summary>
	virtual public int? OfficeID
	{
		get
		{
			int value;
			if (Int32.TryParse(Request.Params["officeid"], out value))
			{
				return value;
			}
			else
			{
				return null;
			}
		}
	}
	/// <summary>
	/// Is arrangement in preview mode
	/// </summary>
	public bool IsPreview
	{
		get
		{
			if (ArrangementID.HasValue)
			{
				if (!(CurrentUser.IsInRole(RolesEnum.OfficeArrangementsEditor) || CurrentUser.IsInRole(RolesEnum.GeneralArrangementsEditor)))
					return true;
				//if ((CurrentUser.IsInRole(RolesEnum.OfficeArrangementsEditor)) && (CurrentArrangement.OfficeID != Int32.Parse(ConfigurationManager.AppSettings["OfficeID"])))
				//    return true;
			}
			return false;
		}
	}
	/// <summary>
	/// Current arrangements date
	/// </summary>
	virtual public DateTime? CurrentArrangementDate
	{
		get
		{
			string value = Request.Params["date"];
			if (value != null)
			{
				int year, month, day;

				Int32.TryParse(value.Substring(4, 4), out year);
				Int32.TryParse(value.Substring(2, 2), out month);
				Int32.TryParse(value.Substring(0, 2), out day);

				return new DateTime(year, month, day);
			}
			else
			{
				return null;
			}
		}
	}
	#endregion

	#region Events
	protected void Page_Load(object sender, EventArgs e)
	{
		if (CurrentUser == null)
			Response.Redirect("~//Admin//AdminArrangements//AdminArrangementsPage.aspx");

		RegisterJavaScript();

		if (!Page.IsPostBack)                               
		{
			if (ArrangementID.HasValue)                            //edit
			{
				if (IsPreview)
				{
					lTitle.Text = GetLocalResourceObject("tPreview").ToString() + ":";
					SetControlsProp(false);
					btnDelete.Visible = false;
				}
				else
				{
					lTitle.Text = GetLocalResourceObject("tEdit").ToString() + ":";
					SetControlsProp(true);
					btnDelete.Visible = true;
				}
				LoadArrangement();
			}
			else                                            //add
			{
				lTitle.Text = GetLocalResourceObject("tNew").ToString() + ":";
				SetControlsProp(true);
				btnDelete.Visible = false;
				Office of = new Office();
				of.Load(OfficeID.Value);
				this.tbOffice.Text = of.OfficeName;
				FillConfHallList();
				this.Calendar.SelectedDate = DateTime.Today;
				this.tbTimeBegin.Text = DateTime.Now.ToShortTimeString();
				this.tbTimeEnd.Text = DateTime.Now.ToShortTimeString();
			}
		}
	}
	/// <summary>
	/// Apply button click.
	/// </summary>
	protected void btnApply_Click(object sender, EventArgs e)
	{
		cvTimeBegin.Validate();
		cvTimeEnd.Validate();
		cvCheckAdding.Validate();

		cvDayRepeatEvery.Validate();
		cvWeekRepeatEvery.Validate();
		cvEndDate.Validate();
		cvEndCount.Validate();

		if (!Page.IsValid)
			return;
		FillArrangement();
		try
		{
			// Create service wrapper.
			Office of = new Office();
			of.Load(OfficeID.Value);
			//using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", CurrentUser.ID))
			//{
			//    if (ArrangementID.HasValue)
			//    {
			//        // One of cyclic arrangement
			//        if (!cbCyclicArrangement.Checked && c.CheckCyclicArrangement(CurrentArrangement.ArrangementID))
			//        {
			//            c.DeleteOneOfCyclicArrangements(CurrentArrangement.ArrangementID, (DateTime)CurrentArrangementDate);
			//            c.AddArrangement(CurrentArrangement.Name, CurrentArrangement.Description, CurrentArrangement.ConferenceHallID, CurrentArrangement.TimeBegin, CurrentArrangement.TimeEnd, CurrentArrangement.ListOfGuests, CurrentArrangement.Equipment);
			//        }
					
			//        // All cycle ore one of the plain arrangements
			//        else
			//        {
			//            DateTime timeBegin = Calendar.SelectedDate.Date;
			//            timeBegin = timeBegin.AddHours(CurrentArrangement.TimeBegin.Hour);
			//            timeBegin = timeBegin.AddMinutes(CurrentArrangement.TimeBegin.Minute);
			//            c.EditArrangement(CurrentArrangement.ArrangementID, CurrentArrangement.Name, CurrentArrangement.Description, CurrentArrangement.ConferenceHallID, timeBegin, CurrentArrangement.TimeEnd, CurrentArrangement.ListOfGuests, CurrentArrangement.Equipment);
			//        }
			//    }
			//    else
			//    {
			//        if (!cbCyclicArrangement.Checked)
			//            c.AddArrangement(CurrentArrangement.Name, CurrentArrangement.Description, CurrentArrangement.ConferenceHallID, CurrentArrangement.TimeBegin, CurrentArrangement.TimeEnd, CurrentArrangement.ListOfGuests, CurrentArrangement.Equipment);
			//        else
			//        {
			//            int Count = 0;
			//            DateTime EndDate = new DateTime();
			//            if (rbEnd.SelectedIndex == 0)
			//                Int32.TryParse(tbCount.Text, out Count);
			//            else
			//            {
			//                int year, month, day;
			//                string[] date = tbEndDate.Text.Split('/');
			//                Int32.TryParse(date[2], out year);
			//                Int32.TryParse(date[0], out month);
			//                Int32.TryParse(date[1], out day);
			//                EndDate = new DateTime(year, month, day);
			//            }

			//            if (rbListDailyWeekly.SelectedIndex == 0)
			//            {
			//                int daysCycle = 1;
			//                if (rbDaily.SelectedIndex == 1)
			//                    Int32.TryParse(tbDayRepeatEvery.Text, out daysCycle);
			//                c.AddDailyCyclicArrangement(CurrentArrangement.Name, CurrentArrangement.Description, CurrentArrangement.ConferenceHallID, CurrentArrangement.TimeBegin, CurrentArrangement.TimeEnd, daysCycle, EndDate, Count, CurrentArrangement.ListOfGuests, CurrentArrangement.Equipment);
			//            }
			//            else
			//            {
			//                bool Mo = false, Tu = false, We = false, Th = false, Fr = false, Sa = false, Su = false;
			//                foreach (ListItem item in cbDaysOfWeek.Items)
			//                {
			//                    if (item.Selected)
			//                        switch (item.Value)
			//                        {
			//                            case "Mo":
			//                                Mo = true;
			//                                break;
			//                            case "Tu":
			//                                Tu = true;
			//                                break;
			//                            case "We":
			//                                We = true;
			//                                break;
			//                            case "Th":
			//                                Th = true;
			//                                break;
			//                            case "Fr":
			//                                Fr = true;
			//                                break;
			//                            case "Sa":
			//                                Sa = true;
			//                                break;
			//                            case "Su":
			//                                Su = true;
			//                                break;
			//                        }
			//                }
			//                int weeksCycle;
			//                Int32.TryParse(tbWeekRepeatEvery.Text, out weeksCycle);
			//                c.AddWeeklyCyclicArrangement(CurrentArrangement.Name, CurrentArrangement.Description, CurrentArrangement.ConferenceHallID, CurrentArrangement.TimeBegin, CurrentArrangement.TimeEnd, weeksCycle, Mo, Tu, We, Th, Fr, Sa, Su, EndDate, Count, CurrentArrangement.ListOfGuests, CurrentArrangement.Equipment);
			//            }
			//        }
			//    }
			//}
		}
		catch (Exception ex)
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}

		Response.Redirect("~//Admin//AdminArrangements//AdminArrangementsPage.aspx");
	}
	/// <summary>
	/// Delete button click.
	/// </summary>
	protected void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			// Create service wrapper.
			Office of = new Office();
			of.Load(OfficeID.Value);
			//using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", CurrentUser.ID))
			//{
			//    // If the checkbox is not checked and this arrangement is cyclic
			//    if (!cbCyclicArrangement.Checked && c.CheckCyclicArrangement(CurrentArrangement.ArrangementID))
			//        c.DeleteOneOfCyclicArrangements(CurrentArrangement.ArrangementID, (DateTime)CurrentArrangementDate);
			//    else
			//        c.DeleteArrangement(CurrentArrangement.ArrangementID);
			//}
		}
		catch (Exception ex)
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}

		Response.Redirect("~//Admin//AdminArrangements//AdminArrangementsPage.aspx");
	}
	/// <summary>
	/// Validate TimeBegin string;
	/// </summary>
	protected void cvTimeBegin_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = CheckTime(args.Value);
	}
	/// <summary>
	/// Validate TimeEnd string.
	/// </summary>
	protected void cvTimeEnd_ServerValidate(object source, ServerValidateEventArgs args)
	{
		if(CheckTime(args.Value))
		{
			if(TimeSpan.Compare(TimeSpan.Parse(tbTimeEnd.Text),TimeSpan.Parse(tbTimeBegin.Text))<1)
			{
				args.IsValid = false;
				return;
			}
		}
		else
		{
			args.IsValid = false;
			return;
		}
		args.IsValid = true;
	}
	/// <summary>
	/// Validate adding.
	/// </summary>
	protected void  cvCheckAdding_ServerValidate(object source, ServerValidateEventArgs args)
	{
		int ConfHallID = Int32.Parse(ddlConferenceHalls.SelectedItem.Value);
		DateTime dBegin = Calendar.SelectedDate.Add(TimeSpan.Parse(tbTimeBegin.Text));
		DateTime dEnd = Calendar.SelectedDate.Add(TimeSpan.Parse(tbTimeEnd.Text));
		int ArrID = 0;
		if (ArrangementID.HasValue)
			ArrID = ArrangementID.Value;
		try
		{
			// Create service wrapper.
			Office of = new Office();
			of.Load(OfficeID.Value);
			//using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", CurrentUser.ID))
			//{
			//    args.IsValid = c.CheckArrangementAdding(ConfHallID, ArrID, dBegin, dEnd);
			//}
		}
		catch (Exception ex)
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
	}
	/// <summary>
	/// Check office.
	/// </summary>
	protected void cvCheckOffice_ServerValidate(object source, ServerValidateEventArgs args)
	{
		//args.IsValid = (Int32.Parse(ddlOffices.SelectedItem.Value) == Int32.Parse(ConfigurationManager.AppSettings["OfficeID"]));
	}

	protected void tbDayRepeatEvery_ServerValidate(object source, ServerValidateEventArgs e)
	{
		int daysCycle;
		if (cbCyclicArrangement.Checked && rbListDailyWeekly.SelectedIndex == 0 && rbDaily.SelectedIndex == 1 && !Int32.TryParse(tbDayRepeatEvery.Text, out daysCycle))
			e.IsValid = false;
		else
			e.IsValid = true;
	}

	protected void tbWeedRepeatEvery_ServerValidate(object source, ServerValidateEventArgs e)
	{
		if (cbCyclicArrangement.Checked && rbListDailyWeekly.SelectedIndex == 1)
		{
			bool daysValid = false;
			foreach (ListItem item in cbDaysOfWeek.Items)
			{
				if (item.Selected)
					daysValid = true;
				break;
			}
			if (daysValid)
			{
				int daysCycle;
				if (!Int32.TryParse(tbWeekRepeatEvery.Text, out daysCycle))
					e.IsValid = false;
				else
					e.IsValid = true;
			}
			else
				e.IsValid = false;
		}
		else
			e.IsValid = true;
	}

	protected void tbCount_ServerValidate(object source, ServerValidateEventArgs e)
	{
		int Count;
		if (cbCyclicArrangement.Checked && rbEnd.SelectedIndex == 0 && !Int32.TryParse(tbCount.Text, out Count))
			e.IsValid = false;
		else
			e.IsValid = true;
	}

	protected void tbEndDate_ServerValidate(object source, ServerValidateEventArgs e)
	{
			if (cbCyclicArrangement.Checked && rbEnd.SelectedIndex == 1)
			{
				DateTime EndDate = new DateTime();
				try
				{
					int year, month, day;
					string[] date = tbEndDate.Text.Split('/');
					Int32.TryParse(date[2], out year);
					Int32.TryParse(date[0], out month);
					Int32.TryParse(date[1], out day);
					EndDate = new DateTime(year, month, day);
				}
				catch
				{
					e.IsValid = false;
				}
				if (EndDate == null || EndDate.Year < 2000)
					e.IsValid = false;
				else
					e.IsValid = true;
			}
			else
				e.IsValid = true;
		
	}
	#endregion

	#region Methods
	/// <summary>
	/// Fill arrangement object.
	/// </summary>
	protected void FillArrangement()
	{
		CurrentArrangement.ConferenceHallID = Int32.Parse(ddlConferenceHalls.SelectedItem.Value);
		CurrentArrangement.Name = tbArrName.Text;
		CurrentArrangement.Description = tbDescription.Text;
		CurrentArrangement.TimeBegin = Calendar.SelectedDate.Add(TimeSpan.Parse(tbTimeBegin.Text));
		DateTime time = DateTime.Parse(tbTimeEnd.Text);
		TimeSpan ts = new TimeSpan(time.Hour,time.Minute,0);
		if ((time.Minute > 0) && (time.Minute < 15))
		{
			ts = new TimeSpan(time.Hour,15,0);
		}
		if ((time.Minute > 15) && (time.Minute < 30))
		{
			ts = new TimeSpan(time.Hour,30,0);
		}
		if ((time.Minute > 30) && (time.Minute < 45))
		{
			ts = new TimeSpan(time.Hour,45,0);
		}
		if ((time.Minute > 45) && (time.Minute < 60))
		{
			ts = new TimeSpan(time.Hour + 1, 0, 0);
		}
		CurrentArrangement.TimeEnd = Calendar.SelectedDate.Add(ts);
		CurrentArrangement.ListOfGuests = tbListOfGuests.Text;
		CurrentArrangement.Equipment = tbEquipment.Text;
	}
	/// <summary>
	/// Load arrangement object.
	/// </summary>
	protected void LoadArrangement()
	{
		tbOffice.Text = CurrentArrangement.OfficeName;
		FillConfHallList();
		tbArrName.Text = CurrentArrangement.Name;
		tbDescription.Text = CurrentArrangement.Description;
		Calendar.SelectedDate = (DateTime)CurrentArrangementDate;
		tbTimeBegin.Text = CurrentArrangement.TimeBegin.ToShortTimeString();
		tbTimeEnd.Text = CurrentArrangement.TimeEnd.ToShortTimeString();
		tbListOfGuests.Text = CurrentArrangement.ListOfGuests;
		tbEquipment.Text = CurrentArrangement.Equipment;
	}
	/// <summary>
	/// Function for validating of time string.
	/// </summary>
	protected bool CheckTime(string time)
	{
		try
		{
			TimeSpan.Parse(time);
			if ((DateTime.Parse(time).Hour < 9) || (DateTime.Parse(time).Hour > 20))
			{
				return false;
			}
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
	/// <summary>
	/// Filling drop down list of conference halls.
	/// </summary>
	protected void FillConfHallList()
	{
		try
		{
			// Create service wrapper.
			Office of = new Office();
			of.Load(OfficeID.Value);
			//using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", CurrentUser.ID))
			//{
			//    XMLSerializableConferenceHall[] chs = c.GetConferenceHallsList(OfficeID.Value);
			//    List<XMLSerializableConferenceHall> coll = new List<XMLSerializableConferenceHall>();
			//    if (chs != null)
			//    {
			//        coll.AddRange(chs);
			//    }
			//    ddlConferenceHalls.DataSource = coll.ToArray();
			//    ddlConferenceHalls.DataBind();
			//    if (ArrangementID.HasValue)
			//    {
			//        ddlConferenceHalls.SelectedIndex = ddlConferenceHalls.Items.IndexOf(ddlConferenceHalls.Items.FindByValue(CurrentArrangement.ConferenceHallID.ToString()));
			//    }
			//}
		}
		catch (Exception ex)
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
	}
	/// <summary>
	/// Set or remove ability of editing.
	/// </summary>
	protected void SetControlsProp(bool CanEdit)
	{
		tbOffice.ReadOnly = true;
		ddlConferenceHalls.Enabled = CanEdit;
		tbArrName.ReadOnly = !CanEdit;
		tbDescription.ReadOnly = !CanEdit;
		tbEquipment.ReadOnly = !CanEdit;
		tbListOfGuests.ReadOnly = !CanEdit;
		tbTimeBegin.ReadOnly = !CanEdit;
		tbTimeEnd.ReadOnly = !CanEdit;
		Calendar.Enabled = CanEdit;
		btnApply.Visible = CanEdit;
	}

	protected void RegisterJavaScript()
	{
		if (!(Page.ClientScript.IsClientScriptIncludeRegistered("cyclicArrangementControls")))
			Page.ClientScript.RegisterClientScriptInclude(GetType(), "cyclicArrangementControls"
														  , "../../Scripts/cyclicArrangementControls.js");

		string JSControllerName = ClientID + "_controller";

		StringBuilder strScript = new StringBuilder();
		strScript.AppendFormat("var {0} = new cyclicArrangementControls();", JSControllerName);

		if (!(Page.ClientScript.IsStartupScriptRegistered("InitializeUploadObject")))
			Page.ClientScript.RegisterStartupScript(GetType(), "InitializeUploadObject"
														  , strScript.ToString(), true);

		string method = JSControllerName + ".onChange()";
		rbListDailyWeekly.Attributes.Add("onclick", method);
	}
	#endregion
}
