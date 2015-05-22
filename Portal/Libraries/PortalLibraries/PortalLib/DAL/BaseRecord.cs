using System.Diagnostics;

namespace ConfirmIt.PortalLib.DAL
{
	/// <summary>
	/// Base DAL object with ID.
	/// </summary>
	public class BaseRecord
	{
		#region Fields
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private int m_ID = -1;
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
		#endregion
	}
}
