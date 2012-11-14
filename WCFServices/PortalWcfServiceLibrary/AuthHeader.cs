using System.Runtime.Serialization;

namespace ConfirmIt.Portal.WcfServiceLibrary
{
	/// <summary>
	/// Class of authentication header.
	/// </summary>
	[DataContract]
	public class AuthHeader
	{
		#region Properties

		/// <summary>
		/// User name.
		/// </summary>
		[DataMember]
		public string UserName { get; set; }

		/// <summary>
		/// User password.
		/// </summary>
		[DataMember]
		public string Password { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public AuthHeader()
		{ }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="userName">User name.</param>
		/// <param name="password">Password.</param>
		public AuthHeader(string userName, string password)
		{
			UserName = userName;
			Password = password;
		}

		#endregion
	}
}
