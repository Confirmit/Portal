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
using System.Net;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib.WebServiceSupport;
using UlterSystems.PortalLib;
using ConfirmIt.PortalLib.Arrangements;
using UlterSystems.PortalLib.DB;

using Office = UlterSystems.PortalLib.BusinessObjects.Office;


public partial class Arrangements_Default : BaseWebPage
{
    #region Classes
    internal class OfficeDescription
    {
        #region Fields
        private int m_OfficeID=0;
        private string m_OfficeName;
        private string m_ServiceURL;
        private string m_UserName;
        private string m_Password;
        private DateTime m_SelectedDate = DateTime.Today;
        #endregion

        #region Properties
        /// <summary>
        /// ID of office.
        /// </summary>
        public int OfficeID
        {
            get { return m_OfficeID; }
            set { m_OfficeID = value; }
        }

        /// <summary>
        /// Name of office.
        /// </summary>
        public string OfficeName
        {
            get { return m_OfficeName; }
            set { m_OfficeName = value; }
        }

        /// <summary>
        /// URL of Web service for office.
        /// </summary>
        public string ServiceURL
        {
            get { return m_ServiceURL; }
            set { m_ServiceURL = value; }
        }

        /// <summary>
        /// Name of user for access to Web service.
        /// </summary>
        public string ServiceUserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }

        /// <summary>
        /// Password of user for access to Web service.
        /// </summary>
        public string ServicePassword
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        /// <summary>
        /// Date selected on the calendar.
        /// </summary>
        public DateTime SelectedDate
        {
            get
            {
                return m_SelectedDate;
            }
            set
            {
                m_SelectedDate = value;
            }
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
            Calendar.SelectedDate = DateTime.Today;
            FillOfficesList();
        }
    }
    /// <summary>
    /// Handles changing of date.
    /// </summary>
    protected void Calendar_SelectionChanged(object sender, EventArgs e)
    {
        FillOfficesList();
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

            // Create object for work with proxy.
            WebProxy proxy = null;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProxyURL"]))
            {
                proxy = new WebProxy(ConfigurationManager.AppSettings["ProxyURL"]);
                string proxyDomain = ConfigurationManager.AppSettings["ProxyUserDomain"];
                string proxyUser = ConfigurationManager.AppSettings["ProxyUserLogin"];
                string proxyPassword = ConfigurationManager.AppSettings["ProxyUserPassword"];
                if (!string.IsNullOrEmpty(proxyUser))
                {
                    NetworkCredential netCred = new NetworkCredential();
                    netCred.UserName = proxyUser;
                    if (!string.IsNullOrEmpty(proxyDomain))
                        netCred.Domain = proxyDomain;
                    if (!string.IsNullOrEmpty(proxyPassword))
                        netCred.Password = proxyPassword;
                    proxy.Credentials = netCred;
                }
            }

            List<OfficeDescription> coll = new List<OfficeDescription>();
            foreach (Office office in Office.GetOffices())
            {
                //if (!string.IsNullOrEmpty(office.StatusesServiceURL))
                {
                    try
                    {
                        OfficeDescription officeDescr = new OfficeDescription();

                        if (!string.IsNullOrEmpty(office.StatusesServiceURL))
                        {
                            UsersStatusesService service = new UsersStatusesService(office.StatusesServiceURL);
                            if (proxy != null)
                            { service.Proxy = proxy; }
                            service.Timeout = 180000;

                            officeDescr.OfficeName = service.GetOfficeName();
                        }
                        if (string.IsNullOrEmpty(officeDescr.OfficeName))
                            officeDescr.OfficeName = office.OfficeName;
                        officeDescr.OfficeID = office.ID.Value;
                        officeDescr.ServiceURL = office.StatusesServiceURL;
                        officeDescr.ServiceUserName = office.StatusesServiceUserName;
                        officeDescr.ServicePassword = office.StatusesServicePassword;
                        officeDescr.SelectedDate = Calendar.SelectedDate;
                        coll.Add(officeDescr);
                    }
                    catch (Exception ex)
                    { Logger.Log.Error(ex.Message, ex); }
                }
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
            Arrangements_Controls_ArrangementsList arrList = e.Item.FindControl("arrangementsList") as Arrangements_Controls_ArrangementsList;

            Button btn = arrList.FindControl("btnUpdate") as Button;
            if (string.IsNullOrEmpty(office.ServiceURL))
                btn.Enabled = false;
            else
                btn.Enabled = true;
        }
    }
    #endregion
}
