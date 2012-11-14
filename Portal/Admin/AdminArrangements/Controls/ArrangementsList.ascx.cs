using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib;
using System.Net;
using Core;
using System.Collections.Generic;
using ConfirmIt.PortalLib.Arrangements;

public partial class Arrangements_Controls_ArrangementsList : BaseUserControl
{
	#region Classes

	protected class UnionColumns
	{
		public int m_First;
		public int m_Len;
	};
	#endregion

	#region Fields
	//private int NumSpanCol = 0;
	//private int NumLastCol = 0;
	#endregion

	#region Properties
	/// <summary>
	/// Reference to user page.
	/// </summary>
	public string NavigateURL
	{
		get
		{
			if (ViewState["NavigateURL"] == null)
				return null;
			else
				return (string)ViewState["NavigateURL"];
		}
		set { ViewState["NavigateURL"] = value; }
	}

	/// <summary>
	/// ID of office.
	/// </summary>
	public int OfficeID
	{
		get
		{
			if (ViewState["OfficeID"] == null)
				return 0;
			else
				return (int)ViewState["OfficeID"];
		}
		set
		{
			ViewState["OfficeID"] = value;
			hlAddArr.NavigateUrl = "~/Admin/AdminArrangements/AddEditArrangement.aspx?officeid=" + value.ToString();
			hlAddCH.NavigateUrl = "~/Admin/AdminArrangements/AddEditConferenceHall.aspx?officeid=" + value.ToString();
		}
	}

	/// <summary>
	/// Name of office.
	/// </summary>
	public string OfficeName
	{
		get
		{
			if (ViewState["OfficeName"] == null)
				return null;
			else
				return (string)ViewState["OfficeName"];
		}
		set
		{
			ViewState["OfficeName"] = value;
			lblOfficeName.Text = value;
		}
	}

	/// <summary>
	/// URL of Web service for office.
	/// </summary>
	public string ServiceURL
	{
		get
		{
			if (ViewState["ServiceURL"] == null)
				return null;
			else
				return (string)ViewState["ServiceURL"];
		}
		set { ViewState["ServiceURL"] = value; }
	}

	/// <summary>
	/// Name of user for access to Web service.
	/// </summary>
	public string ServiceUserName
	{
		get
		{
			if (ViewState["ServiceUserName"] == null)
				return null;
			else
				return (string)ViewState["ServiceUserName"];
		}
		set { ViewState["ServiceUserName"] = value; }
	}

	/// <summary>
	/// Password of user for access to Web service.
	/// </summary>
	public string ServicePassword
	{
		get
		{
			if (ViewState["ServicePassword"] == null)
				return null;
			else
				return (string)ViewState["ServicePassword"];
		}
		set { ViewState["ServicePassword"] = value; }
	}

	/// <summary>
	/// Date selected on the caendar.
	/// </summary>
	public DateTime SelectedDate
	{
		get
		{
			if (ViewState["SelectedDate"] == null)
				return DateTime.Today;
			else
				return (DateTime)ViewState["SelectedDate"];
		}
		set { ViewState["SelectedDate"] = value; }
	}
	#endregion

	#region Event handlers

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		btnUpdate.Click += OnUpdate;
	}

	/// <summary>
	/// Binding of data to controls.
	/// </summary>
	protected void OnCHBound(object sender, DataGridItemEventArgs e)
	{
		if (e.Item.DataItem == null)
			return;
		if (!(e.Item.DataItem is XMLSerializableConferenceHall))
			return;
		XMLSerializableConferenceHall ch = e.Item.DataItem as XMLSerializableConferenceHall;
		//NumSpanCol = 0;
		//NumLastCol = 0;
		try
		{
			// Create service wrapper.
			Office of = new Office();
			of.Load(OfficeID);
			//using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", Page.CurrentUser.ID))
			//{
			//    List<XMLSerializableArrangement> coll = new List<XMLSerializableArrangement>();
			//    XMLSerializableArrangement[] arrList = c.GetDayArragementsList(ch.ConferenceHallID, SelectedDate);
			//    if (arrList != null)
			//    {
			//        coll.AddRange(arrList);
			//    }
			//    HyperLink hlCH = e.Item.Cells[0].FindControl("hlConferenceHallName") as HyperLink;
			//    hlCH.NavigateUrl = "~/Admin/AdminArrangements/AddEditConferenceHall.aspx?officeid=" + OfficeID.ToString() + "&id=" + ch.ConferenceHallID.ToString();
			//    foreach (XMLSerializableArrangement arr in arrList)
			//    {
			//        UnionColumns uc = CountColumns(arr);
			//        if (uc.m_First - NumSpanCol == NumLastCol)
			//            uc.m_First++;
			//        e.Item.Cells[uc.m_First - NumSpanCol].ColumnSpan = uc.m_Len;
			//        string ControlID = "hl" + (uc.m_First).ToString();
			//        HyperLink hl = e.Item.Cells[uc.m_First - NumSpanCol].FindControl(ControlID) as HyperLink;
			//        hl.Text = arr.Name;
			//        hl.NavigateUrl = "~/Admin/AdminArrangements/AddEditArrangement.aspx?officeid=" + OfficeID.ToString() + "&id=" + arr.ArrangementID.ToString() + "&date=" + SelectedDate.Date.ToString("ddMMyyyy");
			//        NumLastCol = uc.m_First - NumSpanCol;
			//        for (int i = 1; i < uc.m_Len - 1; i++)
			//        {
			//            e.Item.Cells.RemoveAt(uc.m_First - NumSpanCol + i);
			//            NumSpanCol++;
			//        }
			//    }
			//}
		}
		catch (Exception ex)
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
	}

	/// <summary>
	/// Handles updating of users data.
	/// </summary>
	protected void OnUpdate(object sender, EventArgs e)
	{
		UpdateArrangementsData();
	}
	/// <summary>
	/// Handles creating item of arrangement grid.
	/// </summary>
	protected void OnCHItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
	{
		try
		{
			if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
			{
				foreach (TableCell cell in e.Item.Cells)
				{
					if (cell != e.Item.Cells[0])
					{
						cell.Attributes.Add("OnMouseOver", "this.style.backgroundColor='#EEEEEE'; this.style.cursor='pointer';");
						cell.Attributes.Add("OnMouseOut", "this.style.backgroundColor='#FFFFFF';");
					}
				}
			}
		}
		catch (Exception ex)
		{
			ConfirmIt.PortalLib.Logger.Logger.Instance.Error(ex.Message, ex);
		}
	}
	#endregion

	#region Methods
	/// <summary>
	/// Updates data of users.
	/// </summary>
	public void UpdateArrangementsData()
	{
		try
		{
			if (string.IsNullOrEmpty(ServiceURL))
				return;
			try
			{
				// Create service wrapper.
				Office of = new Office();
				of.Load(OfficeID);
				//using (ArrangementServiceProxy c = new ArrangementServiceProxy("http://localhost:59874/ArrangementService.svc", Page.CurrentUser.ID))
				//{
				//    List<XMLSerializableConferenceHall> coll = new List<XMLSerializableConferenceHall>();
				//    XMLSerializableConferenceHall[] chs = c.GetConferenceHallsList(OfficeID);
				//    if (chs != null)
				//    {
				//        coll.AddRange(chs);
				//    }
				//    grdConferenceHallsList.DataSource = coll.ToArray();
				//    grdConferenceHallsList.DataBind();
				//}
			}
			catch (Exception)
			{
			}
		}
		finally
		{ }
	}

	/// <summary>
	/// Counts number of columns for arrangement.
	/// </summary>
	/// <param name="arr">Arrangement</param>
	/// <returns>Number of columns for arrangement in grid</returns>
	protected UnionColumns CountColumns(XMLSerializableArrangement arr)
	{
		UnionColumns uc = new UnionColumns();
		int last = 0;
		uc.m_First = (arr.TimeBegin.Hour - 9) * 4 + 1;
		last = (arr.TimeEnd.Hour - 9) * 4 + 1;
		if (arr.TimeBegin.Minute >= 45)
		{
			uc.m_First += 3;
		}
		else
		{
			if (arr.TimeBegin.Minute >= 30)
			{
				uc.m_First += 2;
			}
			else
			{
				if (arr.TimeBegin.Minute >= 15)
				{
					uc.m_First++;
				}
			}
		}
		if (arr.TimeEnd.Minute > 45)
		{
			last += 3;
		}
		else
		{
			if (arr.TimeEnd.Minute > 30)
			{
				last += 2;
			}
			else
			{
				if (arr.TimeEnd.Minute > 15)
				{
					last++;
				}
				else
					if (arr.TimeEnd.Minute == 0)
						last--;
			}
		}
		uc.m_Len = last - uc.m_First + 1;
		return uc;
	}
	#endregion

}