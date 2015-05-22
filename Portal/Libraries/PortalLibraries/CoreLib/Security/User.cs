using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Diagnostics;

using System.Security.Principal;

namespace Core.Security
{
	/// <summary>
	/// Класс пользователя системы.
	/// </summary>
	public class User : IPrincipal
	{
		#region Конструкторы

		public User( WindowsPrincipal principal )
		{
			string[] names = principal.Identity.Name.Split( '\\' );
			m_Login = names[names.Length-1];
			m_Identity = principal.Identity;
			m_Role = UserManager.GetUserRoleByPrincipal( principal );

			UserGroupsCacheKey = String.Format( "User_{0}_Groups", Identity.Name );
		}

		#endregion

		#region Поля

		private	string m_Login = String.Empty;
		private Role m_Role = Role.Anonymous;
		private IIdentity m_Identity = null;

		#endregion

		#region Свойства

		/// <summary>
		/// Логин пользователя в Active Directory
		/// </summary>
		public string Login
		{
			get { return m_Login; }
			set { m_Login = value; }
		}

		private ADUser m_adUser = null;
		public ADUser ADUser
		{
			get
			{
				if( m_adUser == null )
				{
					LdapAuthentication ldap_auth = new LdapAuthentication();
					m_adUser = ldap_auth.GetUserByLogin( Login );
					if( m_adUser == null )
						m_adUser = ADUser.Empty;
				}
				return m_adUser;
			}
		}

		public Role Role
		{
			get
			{
				return m_Role;
			}
		}

		public UserRole UserRole
		{
			get
			{
				UserRole role = new UserRole();
				role.Load( (int)m_Role );
				return role;
			}
		}

		#endregion

		#region Статические свойства

		/// <summary>
		/// Делегат для возвращения текущего пользователя.
		/// </summary>
		/// <returns></returns>
		public delegate User RequestUserCallback();

		/// <summary>
		/// Должен возвращать текущего пользователя. 
		/// Приложения, заинтересованные в получении текущего пользователя, 
		/// должны добавить свой обработчик к этому событию.
		/// </summary>
		public static RequestUserCallback RequestUser;

		/// <summary>
		/// Возвращает текущего пользователя. 
		/// Если текущий пользователь не определен, то возвращается объект для AnonymousUser.
		/// </summary>
		public static User Current
		{
			get
			{
				if(RequestUser != null)
				{
					return RequestUser();
				}
				else
				{
					return null;
				}
			}
		}

		#endregion

		#region IPrincipal Members

		public IIdentity Identity
		{
			get 
			{
				return m_Identity;
			}
		}

		/// <summary>
		/// Проверяет на принадлежность к определённой роли.
		/// </summary>
		/// <param name="role">Группа в AD.</param>
		/// <returns>True, если принадлежит, иначе False.</returns>
		public bool IsInRole( string role )
		{
			return m_Role.ToString() == role.Trim();
		}

		#endregion

		/// <summary>
		/// Функция проверяет, принадлежит ли указанная роль данному пользователю с учётом иерархии.
		/// </summary>
		/// <param name="testRole">Проверяемая роль.</param>
		/// <returns>True, если роль принадлежит, иначе False.</returns>
		public bool HasRolePermissions( Role testRole )
		{
			bool result = true;

			if(testRole != Role.Anonymous)
			{
				result = (m_Role <= testRole);
			}

			return result;
		}

		/// <summary>
		/// Функция проверяет, принадлежит ли данный пользователь хотя бы к одной
		/// из указанных групп. 
		/// </summary>
		/// <param name="roles">Список названий групп.</param>
		/// <returns>True, если принадлежит, иначе False.</returns>
		public bool IsInRoles( string roles )
		{
			bool result = false;

			foreach(string strRole in roles.Split( ',' ))
			{
				if(IsInRole( strRole ))
				{
					result = true;
					break;
				}
			}

			return result;
		}

		#region Принадлежность пользователя группам

		private readonly string UserGroupsCacheKey;

		/// <summary>
		/// Колекция групп, членом которых является пользователь.
		/// </summary>
		public ADGroupCollection Groups
		{
			get
			{
				ADGroupCollection groups = CacheManager.Cache[UserGroupsCacheKey] as ADGroupCollection;
				if (groups == null)
				{
					LdapAuthentication ldap = new LdapAuthentication();
					groups = ldap.GetUserGroupsMembeship( Login );
				}
				return groups;
			}
		}

		/// <summary>
		/// Проверяет принадлежность пользователя заданной группе.
		/// </summary>
		/// <param name="group">Группа, на принадлежность к которой следует проверить.</param>
		/// <returns></returns>
		public bool IsInGroup(ADGroup group)
		{
			ADGroupCollection userGroups = Groups;
			return (userGroups.FindGroupByDN(group.DN) != null);
		}

		#endregion
	}
}
