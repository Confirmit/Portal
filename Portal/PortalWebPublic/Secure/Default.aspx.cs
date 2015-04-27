using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using UlterSystems.PortalLib;
using UlterSystems.PortalLib.BusinessObjects;

using System.Diagnostics;
using ConfirmIt.Portal.WcfServiceLibrary;

public partial class Secure_Default : BaseWebPage
{
	#region Classes
	/// <summary>
	/// Description of office.
	/// </summary>
	internal class OfficeDescription
	{
		#region Fields

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_OfficeName;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_ServiceURL;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_UserName;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_Password;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_MeteoInformer;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_ClockInformer;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_DigitalClockInformer;

		#endregion

		#region Properties
		/// <summary>
		/// Name of office.
		/// </summary>
		public string OfficeName
		{
			[DebuggerStepThrough]
			get { return m_OfficeName; }
			[DebuggerStepThrough]
			set { m_OfficeName = value; }
		}

		/// <summary>
		/// URL of Web service for office.
		/// </summary>
		public string ServiceURL
		{
			[DebuggerStepThrough]
			get { return m_ServiceURL; }
			[DebuggerStepThrough]
			set { m_ServiceURL = value; }
		}

		/// <summary>
		/// Name of user for access to Web service.
		/// </summary>
		public string ServiceUserName
		{
			[DebuggerStepThrough]
			get { return m_UserName; }
			[DebuggerStepThrough]
			set { m_UserName = value; }
		}

		/// <summary>
		/// Password of user for access to Web service.
		/// </summary>
		public string ServicePassword
		{
			[DebuggerStepThrough]
			get { return m_Password; }
			[DebuggerStepThrough]
			set { m_Password = value; }
		}

		/// <summary>
		/// Meteo Informer's string .
		/// </summary>
		public string MeteoInformer
		{
			[DebuggerStepThrough]
			get { return m_MeteoInformer; }
			[DebuggerStepThrough]
			set { m_MeteoInformer = value; }
		}
		/// <summary>
		/// Clock Informer's string .
		/// </summary>
		public string ClockInformer
		{
			[DebuggerStepThrough]
			get { return m_ClockInformer; }
			[DebuggerStepThrough]
			set { m_ClockInformer = value; }
		}

		/// <summary>
		/// Digital clock Informer's string .
		/// </summary>
		public string DigitalClockInformer
		{
			[DebuggerStepThrough]
			get { return m_DigitalClockInformer; }
			[DebuggerStepThrough]
			set { m_DigitalClockInformer = value; }
		}

		#endregion
	}
	#endregion

	#region Event handlers
	/// <summary>
	/// Handles page loading.
	/// </summary>
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			FillOfficesList();
		}
	}
	#endregion

	#region Methods

	/// <summary>
	/// Creates list of offices and shows it.
	/// </summary>
	private void FillOfficesList()
	{
		try
		{
			Office[] offices = Office.GetOffices();
			if (offices == null)
				return;

			//// Create object for work with proxy.
			//WebProxy proxy = null;
			//if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProxyURL"]))
			//{
			//   proxy = new WebProxy(ConfigurationManager.AppSettings["ProxyURL"]);
			//   string proxyDomain = ConfigurationManager.AppSettings["ProxyUserDomain"];
			//   string proxyUser = ConfigurationManager.AppSettings["ProxyUserLogin"];
			//   string proxyPassword = ConfigurationManager.AppSettings["ProxyUserPassword"];

			//   if (!string.IsNullOrEmpty(proxyUser))
			//   {
			//      NetworkCredential netCred = new NetworkCredential();
			//      netCred.UserName = proxyUser;

			//      if (!string.IsNullOrEmpty(proxyDomain))
			//         netCred.Domain = proxyDomain;

			//      if (!string.IsNullOrEmpty(proxyPassword))
			//         netCred.Password = proxyPassword;

			//      proxy.Credentials = netCred;
			//   }
			//}

			List<OfficeDescription> coll = new List<OfficeDescription>();
			foreach (Office office in offices)
			{
				OfficeDescription officeDescr = new OfficeDescription();
				try
				{
					if (!String.IsNullOrEmpty(office.StatusesServiceURL))
					{
						PortalServiceProxy wcfClient = new PortalServiceProxy(
							office.StatusesServiceURL,
							office.StatusesServiceUserName,
							office.StatusesServicePassword);
						officeDescr.OfficeName = wcfClient.GetOfficeName();

						wcfClient.Close();
					}
				}
				catch (Exception ex)
				{
					Logger.Log.Error(ex.Message, ex);
				}

				if (String.IsNullOrEmpty(officeDescr.OfficeName))
					officeDescr.OfficeName = office.OfficeName;

				officeDescr.ServiceURL = office.StatusesServiceURL;
				officeDescr.ServiceUserName = office.StatusesServiceUserName;
				officeDescr.ServicePassword = office.StatusesServicePassword;
				officeDescr.MeteoInformer = office.MeteoInformer;
				officeDescr.ClockInformer = office.ClockInformer;
				officeDescr.DigitalClockInformer = office.DigitalClockInformer;

				coll.Add(officeDescr);
			}

			repOffices.DataSource = coll;
			repOffices.DataBind();
		}
		catch (Exception ex)
		{ Logger.Log.Error(ex.Message, ex); }
	}

	protected void repOffices_OnItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
		{
			OfficeDescription office = (OfficeDescription)e.Item.DataItem;

			Secure_Controls_OfficeUsersList offUsList = e.Item.FindControl("officeUsersList") as Secure_Controls_OfficeUsersList;
			Button btn = offUsList.FindControl("btnUpdate") as Button;

			btn.Enabled = String.IsNullOrEmpty(office.ServiceURL)
									? false
									: true;
		}
	}
	#endregion
}
