using System.Collections.Generic;
using System.Diagnostics;
using ConfirmIt.PortalLib.DAL;

namespace ConfirmIt.PortalLib.BAL
{
    /// <summary>
    /// Class of office.
    /// </summary>
    public class Office
    {
        #region Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_ID;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_OfficeName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_StatusesServiceURL;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_StatusesServiceUserName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_StatusesServicePassword;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_MeteoInformer;
        #endregion

        #region Properties

        /// <summary>
        /// ID of record.
        /// </summary>
        public int ID
        {
            [DebuggerStepThrough]
            get { return m_ID; }
            [DebuggerStepThrough]
            set { m_ID = value; }
        }

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
        /// URL of statuses service.
        /// </summary>
        public string StatusesServiceURL
        {
            [DebuggerStepThrough]
            get { return m_StatusesServiceURL; }
            [DebuggerStepThrough]
            set { m_StatusesServiceURL = value; }
        }

        /// <summary>
        /// User name for statuses service.
        /// </summary>
        public string StatusesServiceUserName
        {
            [DebuggerStepThrough]
            get { return m_StatusesServiceUserName; }
            [DebuggerStepThrough]
            set { m_StatusesServiceUserName = value; }
        }

        /// <summary>
        /// Password for statuses service.
        /// </summary>
        public string StatusesServicePassword
        {
            [DebuggerStepThrough]
            get { return m_StatusesServicePassword; }
            [DebuggerStepThrough]
            set { m_StatusesServicePassword = value; }
        }

        /// <summary>
        /// URL of meteo informer.
        /// </summary>
        public string MeteoInformer
        {
            [DebuggerStepThrough]
            get { return m_MeteoInformer; }
            [DebuggerStepThrough]
            set { m_MeteoInformer = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Creates new office.
        /// </summary>
        /// <param name="officeName">Name of office.</param>
        /// <param name="statusesServiceURL">URL of statuses service.</param>
        /// <param name="statusesServiceUserName">User name for statuses service.</param>
        /// <param name="statusesServicePassword">Password for statuses service.</param>
        /// <param name="meteoInformer">URL of meteo informer.</param>
        /// <returns>ID of new office.</returns>
        public static int CreateOffice(
            string officeName,
            string statusesServiceURL,
            string statusesServiceUserName,
            string statusesServicePassword,
            string meteoInformer
            )
        {
            OfficeDetails details = new OfficeDetails();
            details.OfficeName = officeName;
            details.StatusesServiceURL = statusesServiceURL;
            details.StatusesServiceUserName = statusesServiceUserName;
            details.StatusesServicePassword = statusesServicePassword;
            details.MeteoInformer = meteoInformer;

            return SiteProvider.Offices.CreateOffice(details);
        }

        /// <summary>
        /// Updates office information.
        /// </summary>
        /// <param name="id">ID of office.</param>
        /// <param name="officeName">Name of office.</param>
        /// <param name="statusesServiceURL">URL of statuses service.</param>
        /// <param name="statusesServiceUserName">User name for statuses service.</param>
        /// <param name="statusesServicePassword">Password for statuses service.</param>
        /// <param name="meteoInformer">URL of meteo informer.</param>
        /// <returns>True if office was successfully updated; false, otherwise.</returns>
        public static bool UpdateOffice(
            int id,
            string officeName,
            string statusesServiceURL,
            string statusesServiceUserName,
            string statusesServicePassword,
            string meteoInformer
            )
        {
            OfficeDetails details = new OfficeDetails();
            details.ID = id;
            details.OfficeName = officeName;
            details.StatusesServiceURL = statusesServiceURL;
            details.StatusesServiceUserName = statusesServiceUserName;
            details.StatusesServicePassword = statusesServicePassword;
            details.MeteoInformer = meteoInformer;

            return SiteProvider.Offices.UpdateOffice(details);
        }

        /// <summary>
        /// Deletes office.
        /// </summary>
        /// <param name="id">ID of office.</param>
        /// <returns>True if office was successfully updated; false, otherwise.</returns>
        public static bool DeleteOffice(int id)
        {
            return SiteProvider.Offices.DeleteOffice(id);
        }

        /// <summary>
        /// Returns all offices.
        /// </summary>
        /// <returns>All offices.</returns>
        public static Office[] GetAllOffices()
        {
            OfficeDetails[] allOffices = SiteProvider.Offices.GetAllOffices();

            List<Office> offices = new List<Office>(allOffices.Length);
            foreach (OfficeDetails details in allOffices)
            {
                offices.Add(GetOfficeFromDetails(details));
            }
            return offices.ToArray();
        }

        /// <summary>
        /// Returns office with given ID.
        /// </summary>
        /// <param name="id">ID of office.</param>
        /// <returns>Office with given ID; null otherwise.</returns>
        public static Office GetOfficeByID(int id)
        {
            OfficeDetails details = SiteProvider.Offices.GetOfficeByID(id);
            if (details == null)
                return null;

            return GetOfficeFromDetails(details);
        }

        /// <summary>
        /// Returns office from details.
        /// </summary>
        /// <param name="details">Office details.</param>
        /// <returns>Office from details.</returns>
        private static Office GetOfficeFromDetails(OfficeDetails details)
        {
            Office office = new Office();

            office.ID = details.ID;
            office.OfficeName = details.OfficeName;
            office.StatusesServiceURL = details.StatusesServiceURL;
            office.StatusesServiceUserName = details.StatusesServiceUserName;
            office.StatusesServicePassword = details.StatusesServicePassword;
            office.MeteoInformer = details.MeteoInformer;

            return office;
        }
        #endregion
    }
}
