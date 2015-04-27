using System.Web.Services.Protocols;

namespace UlterSystems.PortalLib.WebServiceSupport
{
	/// <summary>
	/// Class of authentication header for Web services.
	/// </summary>
	public class AuthHeader : SoapHeader
	{
		#region Fields

        private string m_userDomainName;
        private string m_userName;
        private string m_userPassword;
       // private int m_newsID = 0;
		
        #endregion

        #region Constructors

        public AuthHeader()
        {}

        public AuthHeader(string userDomainName/*, int newsID*/)
        {
            m_userDomainName = userDomainName;
           // m_newsID = newsID;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Domain name of current user for access to Web service.
		/// </summary>
        public string UserDomainName
		{
            get { return m_userDomainName; }
            set { m_userDomainName = value; }
		}

        //w
        public string UserName
        {
            get { return m_userName; }
            set { m_userName = value; }
        }

        public string Password
        {
            get { return m_userPassword; }
            set { m_userPassword = value; }
        }

     /*   /// <summary>
        /// ID of current news.
        /// </summary>
        public int NewsID
        {
            get { return m_newsID; }
            set { m_newsID = value; }
        }*/

		#endregion
	}
}
