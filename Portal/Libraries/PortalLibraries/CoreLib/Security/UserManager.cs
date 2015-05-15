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
	/// Класс отвечает за обшие операции с пользователями
	/// </summary>
	public static class UserManager
	{
		/// <summary>
		/// Возвращает список пользователей, удовлетворяющих фильтру по имени и
		/// по идентификатору роли.
		/// </summary>
		/// <param name="userNamePart">Фильтр по имени.</param>
		/// <param name="roleID">Идентификатор роли (-1 - все роли).</param>
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
		/// Возвращает страницу со списком пользователей, удовлетворяющих фильтру по имени и
		/// по идентификатору роли.
		/// </summary>
		/// <param name="userNamePart">Фильтр по имени</param>
		/// <param name="args">Параметры страницы</param>
		/// <param name="roleID">Номер роли (-1 - все роли)</param>
		/// <returns></returns>
		public static PagingResult GetUsersPage( string userNamePart, int roleID, PagingArgs args )
		{
			return GetUsers( userNamePart, roleID ).GetPage( args );
		}

		/// <summary>
		/// Возвращает всех пользователей, начиная с указанной роли до верхушки иерархии.
		/// </summary>
		/// <param name="role">Роль.</param>
		/// <returns>Коллекция пользователей.</returns>
		public static ADUserCollection GetUsersOfRoleHierarchical( Role role )
		{
			ADUserCollection result = new ADUserCollection();

			Role[] roles = (Role[])Enum.GetValues( typeof( Role ) );
			// начинаем с 1, т.к. нулевой у нас аноним
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
					// админы принадлежат к группе админов, с ними всё ясно
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
		/// Функция проверяет, назначена ли пользователю определённая роль.
		/// Т.к. администраторы хранятся не в базе, а в AD, то для них всегда
		/// возвращаем False. Для readonly пользователей, которые тоже не хранятся
		/// в базе возвращаем True, если не найдена ни одна запись с таким пользователем
		/// в базе.
		/// </summary>
		/// <param name="userLogin">Логин пользователя.</param>
		/// <param name="role">Роль пользователя.</param>
		/// <returns>True, если роль назначена, иначе False</returns>
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
		/// Возвращает имя AD-группы из конфигурации для соотв. роли.
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
				// администратор
				result = Role.Admin;
			}
			else if( principal.IsInRole( GetADGroupNameByRole( Role.User ) ) )
			{
				/* 
				 * пользователь. Сейчас все пользователи находятся в одной группе,
				 * а их роли лежат в базе.
				 */
				string[] names = principal.Identity.Name.Split( '\\' );
				string login = names[names.Length - 1];

				result = GetUserRoleByLogin( login );
			}

			return result;
		}

		/// <summary>
		/// Возвращает роль пользователя по его логину. Предполагается, 
		/// что пользователь уже прошёл проверку на принадлежность к AD группе
		/// пользователей.
		/// </summary>
		/// <param name="login">Логин пользователя в домене.</param>
		/// <returns>Роль пользователя.</returns>
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

		#region Свойства

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
