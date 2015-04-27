using System.Configuration;
using System.Diagnostics;

using ConfirmIt.PortalLib.Configuration;

namespace ConfirmIt.PortalLib
{
	/// <summary>
	/// Global objects of Portal.
	/// </summary>
	public static class Globals
	{
		#region Fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static readonly PortalSection m_Settings = (PortalSection) ConfigurationManager.GetSection( "portal" );
		#endregion

		#region Properties
		/// <summary>
		/// Settings of Portal.
		/// </summary>
		public static PortalSection Settings
		{
			[DebuggerStepThrough]
			get { return m_Settings; }
		}
		#endregion
	}
}
