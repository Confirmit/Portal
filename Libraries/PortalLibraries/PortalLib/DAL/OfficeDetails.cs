using System.Diagnostics;

namespace ConfirmIt.PortalLib.DAL
{
	/// <summary>
	/// Class of office data from database.
	/// </summary>
	public class OfficeDetails : BaseRecord
	{
		#region Fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_OfficeName;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string m_StatusesServiceURL;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string m_StatusesServiceUserName;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string m_StatusesServicePassword;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string m_MeteoInformer;
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
	}
}
