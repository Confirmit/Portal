using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Security.Principal;

using Core.Exceptions;
using Core.DBInterface;

namespace Core.Security
{
	/// <summary>
	/// ����� �������� �� ����� �������� � ��������������
	/// </summary>
	public static class UserManager
	{
		/// <summary>
		/// ���������� ������ �������������, ��������������� ������� �� ����� �
		/// �� �������������� ����.
		/// </summary>
		/// <param name="userNamePart">������ �� �����.</param>
		/// <param name="roleID">������������� ���� (-1 - ��� ����).</param>
		/// <returns></returns>
		public static ADUserCollection GetUsers(string userNamePart, int roleID)
		{
			ADUserCollection users = new ADUserCollection();
			if (roleID == -1)
			{
				Dictionary<string, ADUser> loadedUsers = new Dictionary<string, ADUser>();

				foreach (Role role in Enum.GetValues(typeof(Role)))
				{
					if (role == Role.Anonymous) continue;
					foreach (ADUser user in GetUsersOfRole(role, userNamePart))
					{
						if (!loadedUsers.ContainsKey(user.Login))
						{
							users.Add(user);
							loadedUsers.Add(user.Login, user);
						}
					}
				}
			}
			else
				users = GetUsersOfRole((Role)roleID, userNamePart);

			return users;
		}

		/// <summary>
		/// ���������� �������� �� ������� �������������, ��������������� ������� �� ����� �
		/// �� �������������� ����.
		/// </summary>
		/// <param name="userNamePart">������ �� �����</param>
		/// <param name="args">��������� ��������</param>
		/// <param name="roleID">����� ���� (-1 - ��� ����)</param>
		/// <returns></returns>
		public static PagingResult GetUsersPage( string userNamePart, int roleID, PagingArgs args )
		{
			return GetUsers( userNamePart, roleID ).GetPage( args );
		}

		/// <summary>
		/// ���������� ���� �������������, ������� � ��������� ���� �� �������� ��������.
		/// </summary>
		/// <param name="role">����.</param>
		/// <returns>��������� �������������.</returns>
		public static ADUserCollection GetUsersOfRoleHierarchical( Role role )
		{
			ADUserCollection result = new ADUserCollection();

			Role[] roles = (Role[])Enum.GetValues( typeof( Role ) );
			// �������� � 1, �.�. ������� � ��� ������
			for(int i = 1; i < roles.Length && roles[i] <= role; i++)
			{
				ADUserCollection tmpRoles = GetUsersOfRole( roles[i] );
				if(tmpRoles.Count > 0)
				{
					result.AddRange( tmpRoles );
				}
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public static ADUserCollection GetUsersOfRole( Role role )
		{
			return GetUsersOfRole( role, string.Empty );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="role"></param>
		/// <param name="userNamePart"></param>
		/// <returns></returns>
		private static ADUserCollection GetUsersOfRole( Role role, string userNamePart )
		{
			ADUserCollection result = new ADUserCollection();

			if(role != Role.Anonymous)
			{
				ADUserCollection users = new ADUserCollection();
				string groupName = GetADGroupNameByRole( role );
				LdapAuthentication ldap = new LdapAuthentication();
				users = ldap.GetUsersFromGroup( groupName, userNamePart );

				if(role == Role.Admin)
				{
					// ������ ����������� � ������ �������, � ���� �� ����
					result = users;
				}
				else
				{
					foreach(ADUser user in users)
					{
						if(CheckUserRole( user.Login, role ))
						{
							result.Add( user );
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// ������� ���������, ��������� �� ������������ ����������� ����.
		/// �.�. �������������� �������� �� � ����, � � AD, �� ��� ��� ������
		/// ���������� False. ��� readonly �������������, ������� ���� �� ��������
		/// � ���� ���������� True, ���� �� ������� �� ���� ������ � ����� �������������
		/// � ����.
		/// </summary>
		/// <param name="userLogin">����� ������������.</param>
		/// <param name="role">���� ������������.</param>
		/// <returns>True, ���� ���� ���������, ����� False</returns>
		private static bool CheckUserRole( string userLogin, Role role )
		{
			bool result = false;

			if(role != Role.Admin && role != Role.Anonymous)
			{
				Role tmpRole = GetUserRoleByLogin( userLogin );
				result = (role == tmpRole);
			}

			return result;
		}

		/// <summary>
		/// ���������� ��� AD-������ �� ������������ ��� �����. ����.
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public static string GetADGroupNameByRole( Role role )
		{
			string groupKey;
			switch(role)
			{
				case Role.Admin: groupKey = "AdminGroupName"; break;
				case Role.ProjectEditor: groupKey = "UserGroupName"; break;
				case Role.StageEditor: groupKey = "UserGroupName"; break;
				case Role.ValuationEditor: groupKey = "UserGroupName"; break;
				case Role.User: groupKey = "UserGroupName"; break;
                default: throw new CoreArgumentException(Resources.ResourceManager.GetString("RoleException", role));
			}
			return System.Configuration.ConfigurationManager.AppSettings[groupKey];
		}

		public static Role GetUserRoleByPrincipal( IPrincipal principal )
		{
			Role result = Role.Anonymous;

			if( principal.IsInRole( GetADGroupNameByRole( Role.Admin ) ) )
			{
				// �������������
				result = Role.Admin;
			}
			else if( principal.IsInRole( GetADGroupNameByRole( Role.User ) ) )
			{
				/* 
				 * ������������. ������ ��� ������������ ��������� � ����� ������,
				 * � �� ���� ����� � ����.
				 */
				string[] names = principal.Identity.Name.Split( '\\' );
				string login = names[names.Length - 1];

				result = GetUserRoleByLogin( login );
			}

			return result;
		}

		/// <summary>
		/// ���������� ���� ������������ �� ��� ������. ��������������, 
		/// ��� ������������ ��� ������ �������� �� �������������� � AD ������
		/// �������������.
		/// </summary>
		/// <param name="login">����� ������������ � ������.</param>
		/// <returns>���� ������������.</returns>
		public static Role GetUserRoleByLogin( string login )
		{
			Role result = Role.User;

			DataRow row = UserDB.GetRoleIDByLogin( login );
			if(row != null)
			{
				try
				{
					result = (Role)Enum.ToObject( typeof( Role ), row[0] );
				}
				catch(Exception /*ex*/ )
				{
				}
			}

			return result;
		}

		public static void UpdateUserRole(string login, Role role)
		{
			UserDB.DeleteRoleIDByUserID(login);

			if (role != Role.Admin && role != Role.Anonymous && role != Role.User)
			{
				UserDB.AddRoleIDByUserID(login, (int)role);
			}
		}

		public static bool CheckUserInUserList( User user, string[] userList )
		{
			bool result = false;

			if(userList != null)
			{
				foreach(string us in userList)
				{
					if(user.ADUser.Login == us)
					{
						result = true;

						break;
					}
				}
			}

			return result;
		}

		#region ��������

		private static UserRoleCollection m_Roles = null;

        /// <summary>
        /// 
        /// </summary>
		public static UserRoleCollection UserRoles
		{
			get
			{
				if( m_Roles == null )
				{
					m_Roles = new UserRoleCollection();

					foreach(Role role in Enum.GetValues( typeof( Role ) ))
					{
						if(role == Role.Anonymous) 
							continue;

						UserRole tmp = new UserRole();
						if(tmp.Load( (int)role ))
						{
							m_Roles.Add( tmp );
						}
					}
				}

				return m_Roles;
			}
		}

		#endregion
	}
}
