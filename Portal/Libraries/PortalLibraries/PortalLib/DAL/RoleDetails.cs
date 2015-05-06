using System.Diagnostics;
using Core;

namespace ConfirmIt.PortalLib.DAL
{
	/// <summary>
	/// Class of role data from database.
	/// </summary>
	public class RoleDetails : BaseRecord
	{
		#region Fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_RoleID;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string m_Name;
		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string m_Description;
		#endregion

		#region Properties

		/// <summary>
		/// String identifier of role.
		/// </summary>
		public string RoleID
		{
			[DebuggerStepThrough]
			get { return m_RoleID; }
			[DebuggerStepThrough]
			set { m_RoleID = value; }
		}

		/// <summary>
		/// Name of role.
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			get { return m_Name; }
			[DebuggerStepThrough]
			set { m_Name = value; }
		}

		/// <summary>
		/// Description of role.
		/// </summary>
		public string Description
		{
			[DebuggerStepThrough]
			get { return m_Description; }
			[DebuggerStepThrough]
			set { m_Description = value; }
		}

		#endregion

        public override string ToString()
        {
            return RoleID;
        }
	}
}
