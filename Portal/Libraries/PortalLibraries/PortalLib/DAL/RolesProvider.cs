using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;

namespace ConfirmIt.PortalLib.DAL
{
	/// <summary>
	/// Provider for roles system.
	/// </summary>
	public abstract class RolesProvider : DataAccess
	{
		#region Fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RolesProvider m_Instance;
		#endregion

		#region Properties
		/// <summary>
		/// Instance of roles provider.
		/// </summary>
		public static RolesProvider Instance
		{
			[DebuggerStepThrough]
			get
			{
				if( m_Instance == null )
					m_Instance = (RolesProvider) Activator.CreateInstance(
						Type.GetType( Globals.Settings.Roles.ProviderType ) );
				return m_Instance;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor.
		/// </summary>
		public RolesProvider()
		{
			this.ConnectionString = Globals.Settings.Roles.ConnectionString;
		}
		#endregion

		/// <summary>
		/// Adds users to roles.
		/// </summary>
		/// <param name="usersIDs">Array of user IDs.</param>
		/// <param name="roleIDs">Array of role string IDs.</param>
		public abstract void AddUsersToRoles( int[] usersIDs, string[] roleIDs );

		/// <summary>
		/// Creates new role.
		/// </summary>
		/// <param name="role">Role.</param>
		/// <returns>ID of new role.</returns>
		public abstract int CreateRole( RoleDetails role );

		/// <summary>
		/// Updates new role.
		/// </summary>
		/// <param name="role">Role.</param>
		/// <returns>True if role was successfully updated; false, otherwise.</returns>
		public abstract bool UpdateRole( RoleDetails role );

		/// <summary>
		/// Deletes role.
		/// </summary>
		/// <param name="id">ID of role.</param>
		/// <returns>True if role was successfully deleted; false, otherwise.</returns>
		public abstract bool DeleteRole( int id );

		/// <summary>
		/// Returns array of all roles.
		/// </summary>
		/// <returns>Array of all roles.</returns>
		public abstract RoleDetails[] GetAllRoles();

        /// <summary>
        /// Return list RoleDetails from data reader.
        /// </summary>
	    public abstract List<RoleDetails> GetAllRoleDetailsFromReader(IDataReader reader);

		/// <summary>
		/// Returns all roles of given user.
		/// </summary>
		/// <param name="userID">User ID.</param>
		/// <returns>Array of all roles of given user.</returns>
		public abstract RoleDetails[] GetRolesForUser( int userID );

		/// <summary>
		/// Returns IDs of all users in given role.
		/// </summary>
		/// <param name="roleID">Role string ID.</param>
		/// <returns>Array of IDs of all users in given role.</returns>
		public abstract int[] GetUsersInRole( string roleID );

		/// <summary>
		/// Is given user in given role.
		/// </summary>
		/// <param name="userID">User ID.</param>
		/// <param name="roleID">Role string ID.</param>
		/// <returns>True if user is in role; false, otherwise.</returns>
		public abstract bool IsUserInRole( int userID, string roleID );

		/// <summary>
		/// Removes users from roles.
		/// </summary>
		/// <param name="usersIDs">IDs of users.</param>
		/// <param name="roleIDs">Role string IDs.</param>
		public abstract void RemoveUsersFromRoles( int[] usersIDs, string[] roleIDs );

		/// <summary>
		/// Does given role exist.
		/// </summary>
		/// <param name="roleID">Role string ID.</param>
		/// <returns>True if role exists; false, otherwise.</returns>
		public abstract bool RoleExists( string roleID );
	}
}
