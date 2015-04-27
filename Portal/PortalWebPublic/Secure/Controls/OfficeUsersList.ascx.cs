using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using ConfirmIt.Portal.WcfServiceLibrary;
using ConfirmIt.PortalLib.BAL;

using UlterSystems.PortalLib;

using WorkEventType=ConfirmIt.PortalLib.BAL.WorkEventType;
using XMLSerializableUserStatusInfo=UlterSystems.PortalLib.BusinessObjects.XMLSerializableUserStatusInfo;

public partial class Secure_Controls_OfficeUsersList : System.Web.UI.UserControl
{
	#region Classes
	protected enum UsersListGridColumns
	{
		USLName,
		User,
		Begin,
		Status,
		END
	};
	#endregion

	#region Properties
	/// <summary>
	/// Reference to user page.
	/// </summary>
	public string NavigateURL
	{
		get 
        {
            return ViewState["NavigateURL"] == null
                       ? null
                       : (string) ViewState["NavigateURL"];
        }
	    set { ViewState["NavigateURL"] = value; }
	}

	/// <summary>
	/// Name of office.
	/// </summary>
	public string OfficeName
	{
		get
		{
		    return ViewState["OfficeName"] == null
		               ? null
		               : (string) ViewState["OfficeName"];
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
		    return ViewState["ServiceURL"] == null
		               ? null
		               : (string) ViewState["ServiceURL"];
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
		    return ViewState["ServiceUserName"] == null
		               ? null
		               : (string) ViewState["ServiceUserName"];
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
		    return ViewState["ServicePassword"] == null
		               ? null
		               : (string) ViewState["ServicePassword"];
		}
	    set { ViewState["ServicePassword"] = value; }
	}

   /// <summary>
   /// Meteo Informer's string.
   /// </summary>
   public string MeteoInformer
   {
      get
      {
          return ViewState["MeteoInformer"] == null
                     ? null
                     : (string) ViewState["MeteoInformer"];
      }
       set { ViewState["MeteoInformer"] = value; }
   }

   /// <summary>
   /// Clock Informer's string.
   /// </summary>
   public string ClockInformer
   {
       get
       {
           return ViewState["ClockInformer"] == null
                      ? null
                      : (string) ViewState["ClockInformer"];
       }
       set { ViewState["ClockInformer"] = value; }
   }

   /// <summary>
   /// Digital clock Informer's string.
   /// </summary>
    public string DigitalClockInformer
   {
       get
       {
           return ViewState["DigitalClockInformer"] == null
                      ? null
                      : (string) ViewState["DigitalClockInformer"];
       }
       set { ViewState["DigitalClockInformer"] = value; }
   }

	#endregion

	#region Event handlers

	/// <summary>
	/// Handler of control loading.
	/// </summary>
	protected void Page_Load(object sender, EventArgs e)
	{
		string script = @"Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
				function EndRequestHandler(sender, args)
				{
					showAllControls('" + btnUpdate.ID + @"', 'input');
				}";
		Page.ClientScript.RegisterStartupScript(GetType(), "showAllUpdateButtonsScript", script, true);

		btnUpdate.Attributes["onclick"] = String.Format("hideAllControls('{0}', 'input');", btnUpdate.ID);
	}

	/// <summary>
	/// Binding of user data to controls.
	/// </summary>
	protected void OnUserInfoBound(object sender, DataGridItemEventArgs e)
	{
		if (e.Item.DataItem == null)
			return;
		if (!(e.Item.DataItem is XMLSerializableUserStatusInfo))
			return;

		// Получить информацию о статусе пользователя.
		XMLSerializableUserStatusInfo usInfo = (XMLSerializableUserStatusInfo)e.Item.DataItem;
		// Найти гиперссылку
		HyperLink hl = (HyperLink)e.Item.FindControl("hlUserName");
		if (hl == null)
			return;

		// Установить параметры ссылки
	    hl.NavigateUrl = !string.IsNullOrEmpty(NavigateURL)
	                         ? NavigateURL + (@"?UserID=" + usInfo.UserID.ToString())
	                         : string.Empty;

	    // Status cell is filling
		switch ((int)usInfo.EventType)
		{
			case (int)WorkEventType.OfficeOut:
			case (int) WorkEventType.TimeOff:
				e.Item.Cells[(int)UsersListGridColumns.Status].BackColor = System.Drawing.Color.LightGray;
				break;

			case (int) WorkEventType.MainWork:
				e.Item.Cells[(int)UsersListGridColumns.Status].BackColor = System.Drawing.Color.LightSkyBlue;
				break;

			case (int) WorkEventType.LanchTime:
				e.Item.Cells[(int)UsersListGridColumns.Status].BackColor = System.Drawing.Color.LimeGreen;
				break;

			case (int) WorkEventType.Ill:
			case (int) WorkEventType.TrustIll:
				e.Item.Cells[(int)UsersListGridColumns.Status].BackColor = System.Drawing.Color.LightPink;
				break;
			case (int) WorkEventType.BusinessTrip:
				e.Item.Cells[(int)UsersListGridColumns.Status].BackColor = System.Drawing.Color.LightSlateGray;
				break;

			case (int) WorkEventType.Vacation:
				e.Item.Cells[(int)UsersListGridColumns.Status].BackColor = System.Drawing.Color.LightYellow;
				break;
		}
	}

	/// <summary>
	/// Handles updating of users data.
	/// </summary>
	protected void OnUpdate(object sender, EventArgs e)
	{
		UpdateUsersData();
	}

	#endregion

	#region Methods

	/// <summary>
	/// Updates data of users.
	/// </summary>
	public void UpdateUsersData()
	{
        try
        {
            if (string.IsNullOrEmpty(ServiceURL))
                return;

            try
            {
                //// Create object for work with proxy.
                //WebProxy proxy = null;
                //if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProxyURL"]))
                //{
                //    proxy = new WebProxy(ConfigurationManager.AppSettings["ProxyURL"]);
                //    string proxyDomain = ConfigurationManager.AppSettings["ProxyUserDomain"];
                //    string proxyUser = ConfigurationManager.AppSettings["ProxyUserLogin"];
                //    string proxyPassword = ConfigurationManager.AppSettings["ProxyUserPassword"];
                //    if (!string.IsNullOrEmpty(proxyUser))
                //    {
                //        NetworkCredential netCred = new NetworkCredential();
                //        netCred.UserName = proxyUser;
                //        if (!string.IsNullOrEmpty(proxyDomain))
                //            netCred.Domain = proxyDomain;

                //        if (!string.IsNullOrEmpty(proxyPassword))
                //            netCred.Password = proxyPassword;

                //        proxy.Credentials = netCred;
                //    }
                //}

                // Create service wrapper.
                List<XMLSerializableUserStatusInfo> coll = new List<XMLSerializableUserStatusInfo>();

                PortalServiceProxy wcfClient = new PortalServiceProxy(ServiceURL, ServiceUserName, ServicePassword);
                IEnumerable<XMLSerializableUserStatusInfo> statuses = wcfClient.GetUserStatuses();
                
                wcfClient.Close();

                if (statuses != null)
                    coll.AddRange(statuses);

                grdUsersList.DataSource = coll.ToArray();
                grdUsersList.DataBind();
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }
        }
	    finally
		{}
	}

	/// <summary>
	/// Returns string presentation of time.
	/// </summary>
	/// <param name="time">Time.</param>
	/// <returns>String presentation of time.</returns>
	protected string GetTimePresentation(DateTime time)
	{
		try
		{
		    return time == DateTime.MinValue
		               ? string.Empty
		               : time.ToShortTimeString();
		}
		catch( Exception ex )
		{
			Logger.Log.Error( ex.Message, ex );
			return string.Empty;
		}
	}

    /// <summary>
    /// Returns string end work time.
    /// </summary>
    protected string GetEndTime(Object obj)
    {
        try
        {
            XMLSerializableUserStatusInfo info = (XMLSerializableUserStatusInfo)obj;
            UserTimeCalculator timesCalc = new UserTimeCalculator(info.UserID);

            if (info.EndWork == DateTime.MinValue)
                return String.Empty;

            // Время до окончания дня.
            TimeSpan todayRest = timesCalc.GetRateWithLunch(DateTime.Today);
            return (info.BeginWork == info.EndWork)
                       ? (info.BeginWork + todayRest).ToShortTimeString()
                       : info.EndWork.ToShortTimeString();
        }
        catch (Exception ex)
        {
            Logger.Log.Error(ex.Message, ex);
            return string.Empty;
        }
    }

    /// <summary>
    /// Returns string duration of time.
    /// </summary>
    protected string GetEventDuration(Object obj)
    {
        try
        {
            XMLSerializableUserStatusInfo info = (XMLSerializableUserStatusInfo)obj;
            UserTimeCalculator timesCalc = new UserTimeCalculator(info.UserID);

            if ((info.BeginWork == DateTime.MinValue)
                || (info.EndWork == DateTime.MinValue))
                return String.Empty;

            DateTime workedTime = (info.BeginWork == info.EndWork)
                       ? new DateTime((DateTime.Now - info.BeginWork).Ticks)
                       : new DateTime(timesCalc.GetWorkedTimeWithoutLunch(info.BeginWork.Date).Ticks);
            
            return workedTime.ToString("HH:mm");
        }
        catch (Exception ex)
        {
            Logger.Log.Error(ex.Message, ex);
            return string.Empty;
        }
    }

	#endregion

   protected string GetMeteoInformer()
   {
       return MeteoInformer;
   }

    protected string GetClockInformer()
    {
        return ClockInformer;
    }

    protected string GetDigitalClockInformer()
    {
        return DigitalClockInformer;
    }
}
