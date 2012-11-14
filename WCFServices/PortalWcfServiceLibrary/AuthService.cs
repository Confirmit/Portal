using System.ServiceModel;
using System.Configuration;

namespace ConfirmIt.Portal.WcfServiceLibrary
{
	/// <summary>
	/// Abstract service support class with authentication support.
	/// </summary>
	public abstract class AuthService
	{
		/// <summary>
		/// Checks authentication.
		/// </summary>
		/// <returns>True if user is authenticated; false, otherwise.</returns>
		protected bool CheckAuthentication()
		{
			AuthHeader authHeader =
				OperationContext.Current.IncomingMessageHeaders.GetHeader<AuthHeader>("AuthHeader", "ConfirmIt.Portal.WcfServiceLibrary");

			if ((authHeader.UserName != ConfigurationManager.AppSettings["UserName"])
			|| (authHeader.Password != ConfigurationManager.AppSettings["Password"]))
			{
				return false;
			}

			return true;
		}

	}
}
