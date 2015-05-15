using System.Collections.Generic;
using System.Diagnostics;

using ConfirmIt.PortalLib.DAL;
using Core;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BAL
{
	public enum RolesEnum
	{
		Administrator = 1,
		Employee = 2,
		OfficeNewsEditor = 3,
		GeneralNewsEditor = 4,
		ForumAdministrator = 5,
		ForumBannedUser = 6,
		OfficeArrangementsEditor = 8,
		GeneralArrangementsEditor = 9,
		PublicUser
	}

	/// <summary>
	/// Class of user role.
	/// </summary>
	public class Role
	{
		#region Fields

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private int m_ID = -1;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string m_RoleID;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private MLText m_Name = new MLText();

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private MLText m_Description = new MLText();

		#endregion

		#region Properties

		/// <summary>
		/// Get actual events for current role (group).
		/// </summary>
		public IList<UserEvent> ActualEvents
		{
			get { return SiteProvider.Events.GetGroupEvents(ID, true); }
		}

		/// <summary>
		/// ID of role.
		/// </summary>
		public int ID
		{
			[DebuggerStepThrough]
			get { return m_ID; }
			[DebuggerStepThrough]
			set { m_ID = value; }
		}

		/// <summary>
		/// Role string ID.
		/// </summary>
		public string RoleID
		{
			[DebuggerStepThrough]
			get { return m_RoleID; }
			[DebuggerStepThrough]
			set { m_RoleID = value; }
		}

		/// <summary>
		/// Role name.
		/// </summary>
		public MLText Name
		{
			[DebuggerStepThrough]
			get { return m_Name; }
			[DebuggerStepThrough]
			set { m_Name = value; }
		}

		/// <summary>
		/// Role description.
		/// </summary>
		public MLText Description
		{
			[DebuggerStepThrough]
			get { return m_Description; }
			[DebuggerStepThrough]
			set { m_Description = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Saves role to storage.
		/// </summary>
		public void Save()
		{
			if (m_ID == -1)
				m_ID = CreateRole(m_RoleID, m_Name, m_Description);
			else
				UpdateRole(m_ID, m_RoleID, m_Name, m_Description);
		}

		#region Event support

		/// <summary>
		/// Add event to group event collection.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void AddGroupEvent(Event eventData)
		{
			SiteProvider.Events.AddGroupEvent(ID, eventData);
		}

		/// <summary>
		/// Remover event from group event collection.
		/// </summary>
		/// <param name="eventID">Event ID.</param>
		public void DeleteGroupEvent(int eventID)
		{
			SiteProvider.Events.DeleteEventFromGroup(ID, eventID);
		}

		#endregion

		/// <summary>
		/// Adds user to role.
		/// </summary>
		/// <param name="userID">User ID.</param>
		public void AddUser( int userID )
		{
			SiteProvider.Roles.AddUsersToRoles( new int[] { userID }, new string[] { m_RoleID } );
		}

		/// <summary>
		/// Removes user from role.
		/// </summary>
		/// <param name="userID">User ID.</param>
		public void RemoveUser( int userID )
		{
			SiteProvider.Roles.RemoveUsersFromRoles( new int[] { userID }, new string[] { m_RoleID } );
		}

		/// <summary>
		/// Is user in role.
		/// </summary>
		/// <param name="userID">User ID.</param>
		/// <returns>True if user is in role; false, otherwise.</returns>
		public bool IsInRole( int userID )
		{
			return SiteProvider.Roles.IsUserInRole( userID, m_RoleID );
		}

		public override bool Equals(object obj)
		{
			Role role = obj as Role;
			if (role == null)
				return false;

			return this.ID == role.ID;
		}

		public override int GetHashCode()
		{
			return ID;
		}

		#endregion

		#region Static methods

		/// <summary>
		/// Updates information about role.
		/// </summary>
		/// <param name="id">Role ID.</param>
		/// <param name="roleID">Role string ID.</param>
		/// <param name="name">Role name.</param>
		/// <param name="description">Role description.</param>
		/// <returns>If record was updated.</returns>
		public static bool UpdateRole( int id, string roleID, MLText name, MLText description )
		{
			RoleDetails details = new RoleDetails();
			details.ID = id;
			details.RoleID = roleID;
			details.Name = name.ToXMLString();
			details.Description = description.ToXMLString();

			return SiteProvider.Roles.UpdateRole( details );
		}

		/// <summary>
		/// Updates information about role.
		/// </summary>
		/// <param name="roleID">Role string ID.</param>
		/// <param name="name">Role name.</param>
		/// <param name="description">Role description.</param>
		/// <returns>If record was created.</returns>
		public static int CreateRole( string roleID, MLText name, MLText description )
		{
			RoleDetails details = new RoleDetails();
			details.RoleID = roleID;
			details.Name = name.ToXMLString();
			details.Description = description.ToXMLString();

			return SiteProvider.Roles.CreateRole( details );
		}

		/// <summary>
		/// Deletes information about role.
		/// </summary>
		/// <param name="id">Role ID.</param>
		/// <returns>If record was deleted.</returns>
		public static bool DeleteRoleByID( int id )
		{
			return SiteProvider.Roles.DeleteRole( id );
		}

		/// <summary>
		/// Deletes information about role.
		/// </summary>
		/// <param name="roleID">Role string ID.</param>
		/// <returns>If record was deleted.</returns>
		public static bool DeleteRoleByRoleID( string roleID )
		{
			Role role = GetRole( roleID );
			if( role == null )
				return false;

			return SiteProvider.Roles.DeleteRole( role.m_ID );
		}

		/// <summary>
		/// Returns all roles.
		/// </summary>
		/// <returns>Array of all roles.</returns>
		public static Role[] GetAllRoles()
		{
			RoleDetails[] rolesDetails = SiteProvider.Roles.GetAllRoles();

			List<Role> roles = new List<Role>( rolesDetails.Length );
			foreach( RoleDetails details in rolesDetails )
			{
				roles.Add( GetRoleFromDetails( details ) );
			}

			return roles.ToArray();
		}

		/// <summary>
		/// Returns user roles.
		/// </summary>
		/// <param name="userID">User ID.</param>
		/// <returns>Array of user roles.</returns>
		public static Role[] GetUserRoles( int userID )
		{
			List<Role> roles = new List<Role>();
			foreach( RoleDetails details in SiteProvider.Roles.GetRolesForUser( userID ) )
			{
				roles.Add( GetRoleFromDetails( details ) );
			}

			return roles.ToArray();
		}

		/// <summary>
		/// Returns role by role string ID.
		/// </summary>
		/// <param name="roleID">Role string ID.</param>
		/// <returns>Role with given string ID.</returns>
		public static Role GetRole(string roleID)
		{
			foreach (RoleDetails role in SiteProvider.Roles.GetAllRoles())
			{
				if (string.Compare(roleID, role.RoleID, true) == 0)
					return GetRoleFromDetails(role);
			}

			return null;
		}

		/// <summary>
		/// Returns role from role details.
		/// </summary>
		/// <param name="details">Role details.</param>
		/// <returns>Role.</returns>
		public static Role GetRoleFromDetails( RoleDetails details )
		{
			Role role = new Role();
			role.m_ID = details.ID;
			role.m_RoleID = details.RoleID;
			role.m_Name = new MLText();
			role.m_Name.LoadFromXML( details.Name );
			role.m_Description = new MLText();
			role.m_Description.LoadFromXML( details.Description );

			return role;
		}

		#endregion
	}
}