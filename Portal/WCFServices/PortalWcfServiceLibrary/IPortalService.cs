using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.Portal.WcfServiceLibrary
{
	/// <summary>
	/// Interface of Portal services.
	/// </summary>
	[ServiceContract]
	public interface IPortalService
	{
		/// <summary>
		/// Get office name.
		/// </summary>
		/// <returns>Office name.</returns>
		[OperationContract]
		string GetOfficeName();

		/// <summary>
		/// Get list of users statuses.
		/// </summary>
		/// <returns>User statuses list.</returns>
		[OperationContract]
		IEnumerable<XMLSerializableUserStatusInfo> GetUserStatuses();
	}
}