using System.Collections.Generic;
using System.ServiceModel;

namespace ConfirmIt.Portal.WcfServiceLibrary
{
	/// <summary>
	/// Client proxy for portal service.
	/// </summary>
	public class PortalServiceProxy : ClientBase<IPortalService>, IPortalService
	{
		#region Fields

		private readonly string m_UserName;
		private readonly string m_Password;

		#endregion

		#region Constructors
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="url">URL of Web service.</param>
		/// <param name="userName">User name.</param>
		/// <param name="password">Password.</param>
		public PortalServiceProxy(string url, string userName, string password) 
			: base( 
				new WSHttpBinding(), 
				new EndpointAddress(url) )
		{
			m_UserName = userName;
			m_Password = password;
		}
		#endregion

		#region IPortalService Members

		/// <summary>
		/// Get office name.
		/// </summary>
		/// <returns>Office name.</returns>
		public string GetOfficeName()
		{
			AuthHeader authHeader = new AuthHeader(m_UserName, m_Password);
			MessageHeader<AuthHeader> header = new MessageHeader<AuthHeader>(authHeader);

			using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
			{
				OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
				return Channel.GetOfficeName();
			}
		}

		/// <summary>
		/// Get list of users statuses.
		/// </summary>
		/// <returns>User statuses list.</returns>
		public IEnumerable<UlterSystems.PortalLib.BusinessObjects.XMLSerializableUserStatusInfo> GetUserStatuses()
		{
			AuthHeader authHeader = new AuthHeader(m_UserName, m_Password);
			MessageHeader<AuthHeader> header = new MessageHeader<AuthHeader>(authHeader);

			using (OperationContextScope scope = new OperationContextScope(this.InnerChannel))
			{
				OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("AuthHeader", "ConfirmIt.Portal.WcfServiceLibrary"));
				return this.Channel.GetUserStatuses();
			}
		}

		#endregion
	}
}
