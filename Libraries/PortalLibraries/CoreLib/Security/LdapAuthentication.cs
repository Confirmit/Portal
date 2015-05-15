using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.DirectoryServices;
using Core.Exceptions;

namespace Core.Security
{
	public class LdapAuthentication
	{
		private string _path;
		private string _groupPath;

		public LdapAuthentication()
		{
            if (System.Configuration.ConfigurationManager.AppSettings["LdapServer"] == null)
                throw new CoreSettingsPropertyNotFoundException(Resources.ResourceManager.GetString("SettingsPropertyNotFoundException", "LdapServer"));

			string ldap_server = System.Configuration.ConfigurationManager.AppSettings["LdapServer"].ToUpper();
			string tpl_string;
			if( ldap_server.IndexOf( "LDAP://" ) == 0 )
				tpl_string = "{0}/{1}";
			else
				tpl_string = "LDAP://{0}/{1}";

            if( System.Configuration.ConfigurationManager.AppSettings["SubdivisionsRoot"] == null )
                throw new CoreSettingsPropertyNotFoundException(Resources.ResourceManager.GetString("SettingsPropertyNotFoundException", "SubdivisionsRoot"));

			_path = String.Format( tpl_string, ldap_server, System.Configuration.ConfigurationManager.AppSettings["SubdivisionsRoot"] );

            if( System.Configuration.ConfigurationManager.AppSettings["GroupsSubdivisionsRoot"] == null )
                throw new CoreSettingsPropertyNotFoundException(Resources.ResourceManager.GetString("SettingsPropertyNotFoundException", "GroupsSubdivisionsRoot"));

			_groupPath = String.Format( tpl_string, ldap_server, System.Configuration.ConfigurationManager.AppSettings["GroupsSubdivisionsRoot"] );
		}

		#region Свойства

		private string LdapUserName
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings["LdapUser"] + System.Configuration.ConfigurationManager.AppSettings["LdapUserSuffix"];
			}
		}

		#endregion

		#region Аутентификация пользователей

		/// <summary>
		/// Проверяет существование пользователя с данным логином и паролем в AD
		/// </summary>
		/// <param name="username">Имя пользователя (вместе с доменом)</param>
		/// <param name="pwd">Пароль пользователя</param>
		public bool IsAuthenticated( string username, string pwd )
		{
			string domainAndUsername = username;
			DirectoryEntry entry = new DirectoryEntry( _path, domainAndUsername, pwd );

			try
			{
				object obj = entry.NativeObject;

				DirectorySearcher search = new DirectorySearcher( entry );

				search.SearchScope = SearchScope.Subtree;
				search.Filter = "(SAMAccountName=" + username + ")";
				search.PropertiesToLoad.Add( "cn" );
				SearchResult result = search.FindOne();

				if( null == result )
				{
					return false;
				}
			}
			catch( System.Runtime.InteropServices.COMException ex )
			{
				if( ex.ErrorCode == -2147023570 )
					return false;
                throw new CoreException(Resources.ResourceManager.GetString("ErrorAuthentificatingUserException", ex.Message));
			}
			catch( Exception ex )
			{
                throw new CoreException(Resources.ResourceManager.GetString("ErrorAuthentificatingUserException", ex.Message));
			}

			return true;
		}

		#endregion

		#region Поиск пользователей

		/// <summary>
		/// Ищет в AD запись о пользователе с заданным именем.
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <returns></returns>
		protected SearchResult FindUserEntry( string userName )
		{
			try
			{
				using (DirectoryEntry entry = new DirectoryEntry( _path,
					LdapUserName, ConfigurationManager.AppSettings["LdapPassword"] ))
				using (DirectorySearcher searcher = new DirectorySearcher( entry ))
				{
					searcher.SearchScope = SearchScope.Subtree;
					searcher.Filter = String.Format( "(SAMAccountName={0})", userName );
					try
					{
						return searcher.FindOne();
					}
					catch (ArgumentException) // неверная строка поиска
					{
						return null;
					}
				}
			}
			catch (Exception ex)
			{
                throw new CoreApplicationException(Resources.ResourceManager.GetString("ErrorSearchingUserException"), ex);
			}
		}

		/// <summary>
		/// Ищет пользователей AD по заданной части имени пользователя
		/// </summary>
		/// <param name="namePart">Часть имени/логина пользователя</param>
		/// <param name="groupName">
		/// Имя группы, которой следует ограничить поиск (null - искать во всех группах).
		/// </param>
		/// <returns>Список найденных пользователей</returns>
		public PagingResult FindUsers( string namePart, string groupName, PagingArgs args )
		{
			return FindUsers( namePart, groupName ).GetPage( args );
		}

		/// <summary>
		/// Ищет пользователей AD по заданной части имени пользователя
		/// </summary>
		/// <param name="namePart">Часть имени/логина пользователя</param>
		/// <param name="groupName">
		/// Имя группы, которой следует ограничить поиск (null - искать во всех группах).
		/// </param>
		/// <returns>Список найденных пользователей</returns>
		public ADUserCollection FindUsers( string namePart, string groupName )
		{
			DirectoryEntry entry = new DirectoryEntry( _path,
				LdapUserName,
				System.Configuration.ConfigurationManager.AppSettings["LdapPassword"] );
			
			try
			{
				DirectorySearcher search = new DirectorySearcher( entry );
				search.SearchScope = SearchScope.Subtree;
				if (String.IsNullOrEmpty( groupName ))
				{
					search.Filter = String.Format(
						"(& ({1})(|(sAMAccountName=*{0}*)(cn=*{0}*)(mail=*{0}*)(description=*{0}*)) (userPrincipalName=*))",
						namePart, System.Configuration.ConfigurationManager.AppSettings["LdapUserClass"] );
				}
				else
				{
					search.Filter = String.Format(
						"(& ({1})(memberof={2})(|(sAMAccountName=*{0}*)(cn=*{0}*)(mail=*{0}*)(description=*{0}*)) (userPrincipalName=*))",
						namePart, System.Configuration.ConfigurationManager.AppSettings["LdapUserClass"],
						GetGroupDistinguishedName(groupName) );
				}
				
				try
				{
					SearchResultCollection results = search.FindAll();
					ADUserCollection users = new ADUserCollection();
					foreach(SearchResult result in results)
						users.Add( new ADUser( result ) );
					return users;
				}
				catch(ArgumentException) // случай неправильной строки поиска
				{ }
				return new ADUserCollection();
			}
			catch(Exception ex)
			{
                throw new CoreApplicationException(Resources.ResourceManager.GetString("ErrorSearchingUserException"), ex);
			}
		}

		/// <summary>
		/// Возвращает данные пользователя AD по заданному логину
		/// </summary>
		/// <param name="login">Логин пользователя</param>
		public ADUser GetUserByLogin( string login )
		{
			DirectoryEntry entry = new DirectoryEntry( _path,
				LdapUserName,
				System.Configuration.ConfigurationManager.AppSettings["LdapPassword"] );

			object obj = entry.NativeObject;

			DirectorySearcher search = new DirectorySearcher( entry );

			search.SearchScope = SearchScope.Subtree;
			search.Filter = "(SAMAccountName=" + login + ")";
			try
			{
				SearchResult result = search.FindOne();

				if( null != result )
					return new ADUser( result );
				else
					return null;
			}
			catch( ArgumentException )
			{ }
			return null;
		}

		#endregion

		/// <summary>
		/// Возвращает коллекцию пользователей, входящих в данную группу
		/// </summary>
		/// <param name="group_name">Имя группы</param>
		/// <returns></returns>
		public ADUserCollection GetUsersFromGroup(string group_name)
		{
			DirectoryEntry group = new DirectoryEntry(
				string.Format( "LDAP://{1}/{0}", GetGroupDistinguishedName(group_name),
					System.Configuration.ConfigurationManager.AppSettings["LdapServer"] ),
				LdapUserName, System.Configuration.ConfigurationManager.AppSettings["LdapPassword"] );
			DirectorySearcher search = new DirectorySearcher( group );
			search.SearchScope = SearchScope.Subtree;

			ADUserCollection users = new ADUserCollection();
			try
			{
				foreach( string dn in group.Properties["member"] )
				{
					DirectoryEntry user = new DirectoryEntry(
						string.Format( "LDAP://{1}/{0}", dn, System.Configuration.ConfigurationManager.AppSettings["LdapServer"] ),
						LdapUserName, System.Configuration.ConfigurationManager.AppSettings["LdapPassword"] );
					users.Add( new ADUser(user) );
				}
			}
			catch( DirectoryServicesCOMException )
			{
                throw new CoreApplicationException(Resources.ResourceManager.GetString("ADException", group_name));
			}
			return users;
		}

		/// <summary>
		/// Возвращает коллекцию пользователей, входящих в данную группу и удовлетворяющих
		/// указанному фильтру.
		/// </summary>
		/// <param name="groupName">Имя группы</param>
		/// 
		/// <param name="userNamePart">Часть имени пользователя, по которой производится поиск.</param>
		/// <returns></returns>
		public ADUserCollection GetUsersFromGroup(string groupName, string userNamePart )
		{
			if( string.IsNullOrEmpty( userNamePart ) )
				return GetUsersFromGroup(groupName);

			ADUserCollection rawUsers = FindUsers( userNamePart, groupName );
			ADUserCollection usersWithRoles = new ADUserCollection();
			foreach (ADUser user in rawUsers)
				usersWithRoles.Add( new ADUser(user) );
			
			return usersWithRoles;
		}


		#region Поиск групп

		/// <summary>
		/// Возвращает уникальное имя группы.
		/// </summary>
		/// <param name="groupName">Имя группы (CN)</param>
		/// <returns></returns>
		private string GetGroupDistinguishedName( string groupName )
		{
			return String.Format( "CN={0},{1}", groupName, ConfigurationManager.AppSettings["GroupsSubdivisionsRoot"] );
		}

		/// <summary>
		/// Возвращает все группы из AD, имена которых удовлетворяют заданному фильтру
		/// </summary>
		/// <param name="groupFilter">Фильтр по имени группы.</param>
		/// <param name="args"></param>
		/// <returns></returns>
		public ADGroupCollection FindGroups( string groupFilter )
		{
			try
			{
				using (DirectoryEntry entry = new DirectoryEntry( _groupPath, LdapUserName,
					ConfigurationManager.AppSettings["LdapPassword"] ))
				using (DirectorySearcher searcher = new DirectorySearcher( entry ))
				{
					searcher.SearchScope = SearchScope.Subtree;
					searcher.Filter = String.Format( @"(&(objectCategory=group)(name={0}))", groupFilter );
					ADGroupCollection groups = new ADGroupCollection();
					try
					{
						foreach (SearchResult result in searcher.FindAll())
							groups.Add( new ADGroup( result ) );
					}
					catch (ArgumentException) { } // неверная строка поиска
					return groups;
				}
			}
			catch (Exception ex)
			{
                throw new CoreApplicationException(Resources.ResourceManager.GetString("ErrorSearchingGroupException"), ex);
			}
		}
		
		/// <summary>
		/// Возвращает постранично группы из AD, имена которых удовлетворяют заданному фильтру
		/// </summary>
		/// <param name="groupFilter">Фильтр по имени группы.</param>
		/// <param name="args"></param>
		/// <returns></returns>
		public PagingResult FindGroups( string groupFilter, PagingArgs args )
		{
			try
			{
				using (DirectoryEntry entry = new DirectoryEntry( _groupPath, LdapUserName,
					ConfigurationManager.AppSettings["LdapPassword"] ))
				using (DirectorySearcher searcher = new DirectorySearcher( entry ))
				{
					searcher.SearchScope = SearchScope.Subtree;
					searcher.Sort.PropertyName = ADGroup.GetADPropertyName( args.SortExpression );
					searcher.Sort.Direction = args.SortOrderASC ? SortDirection.Ascending : SortDirection.Descending;
					searcher.Filter = String.Format( @"(&(objectCategory=group)(name={0}))", groupFilter );
					ADGroupCollection groups = new ADGroupCollection();
					try
					{
						foreach (SearchResult result in searcher.FindAll())
							groups.Add( new ADGroup( result ) );
					}
					catch (ArgumentException) { } // неверная строка поиска
					return groups.GetPage( args );
				}
			}
			catch (Exception ex)
			{
                throw new CoreApplicationException(Resources.ResourceManager.GetString("ErrorSearchingGroupException"), ex);
			}
		}

		#endregion

		#region Принадлежность пользователя группам

		/// <summary>
		/// Возвращает список групп, членом которым является указанный пользователь
		/// </summary>
		/// <param name="userName">Имя пользователя (login name без указания домена)</param>
		/// <returns></returns>
		public ADGroupCollection GetUserGroupsMembeship( string userName )
		{
			ADGroupCollection groups = new ADGroupCollection();
			
			SearchResult searchResult = FindUserEntry( userName );
			foreach (string groupDn in searchResult.Properties["memberOf"])
			{
				ADGroup group = new ADGroup();
				group.DN = groupDn;
				groups.Add( group );
			}

			return groups;
		}

		#endregion

	}
}
