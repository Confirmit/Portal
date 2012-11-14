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

		#region ��������

		private string LdapUserName
		{
			get
			{
				return System.Configuration.ConfigurationManager.AppSettings["LdapUser"] + System.Configuration.ConfigurationManager.AppSettings["LdapUserSuffix"];
			}
		}

		#endregion

		#region �������������� �������������

		/// <summary>
		/// ��������� ������������� ������������ � ������ ������� � ������� � AD
		/// </summary>
		/// <param name="username">��� ������������ (������ � �������)</param>
		/// <param name="pwd">������ ������������</param>
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

		#region ����� �������������

		/// <summary>
		/// ���� � AD ������ � ������������ � �������� ������.
		/// </summary>
		/// <param name="userName">��� ������������</param>
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
					catch (ArgumentException) // �������� ������ ������
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
		/// ���� ������������� AD �� �������� ����� ����� ������������
		/// </summary>
		/// <param name="namePart">����� �����/������ ������������</param>
		/// <param name="groupName">
		/// ��� ������, ������� ������� ���������� ����� (null - ������ �� ���� �������).
		/// </param>
		/// <returns>������ ��������� �������������</returns>
		public PagingResult FindUsers( string namePart, string groupName, PagingArgs args )
		{
			return FindUsers( namePart, groupName ).GetPage( args );
		}

		/// <summary>
		/// ���� ������������� AD �� �������� ����� ����� ������������
		/// </summary>
		/// <param name="namePart">����� �����/������ ������������</param>
		/// <param name="groupName">
		/// ��� ������, ������� ������� ���������� ����� (null - ������ �� ���� �������).
		/// </param>
		/// <returns>������ ��������� �������������</returns>
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
				catch(ArgumentException) // ������ ������������ ������ ������
				{ }
				return new ADUserCollection();
			}
			catch(Exception ex)
			{
                throw new CoreApplicationException(Resources.ResourceManager.GetString("ErrorSearchingUserException"), ex);
			}
		}

		/// <summary>
		/// ���������� ������ ������������ AD �� ��������� ������
		/// </summary>
		/// <param name="login">����� ������������</param>
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
		/// ���������� ��������� �������������, �������� � ������ ������
		/// </summary>
		/// <param name="group_name">��� ������</param>
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
		/// ���������� ��������� �������������, �������� � ������ ������ � ���������������
		/// ���������� �������.
		/// </summary>
		/// <param name="groupName">��� ������</param>
		/// 
		/// <param name="userNamePart">����� ����� ������������, �� ������� ������������ �����.</param>
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


		#region ����� �����

		/// <summary>
		/// ���������� ���������� ��� ������.
		/// </summary>
		/// <param name="groupName">��� ������ (CN)</param>
		/// <returns></returns>
		private string GetGroupDistinguishedName( string groupName )
		{
			return String.Format( "CN={0},{1}", groupName, ConfigurationManager.AppSettings["GroupsSubdivisionsRoot"] );
		}

		/// <summary>
		/// ���������� ��� ������ �� AD, ����� ������� ������������� ��������� �������
		/// </summary>
		/// <param name="groupFilter">������ �� ����� ������.</param>
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
					catch (ArgumentException) { } // �������� ������ ������
					return groups;
				}
			}
			catch (Exception ex)
			{
                throw new CoreApplicationException(Resources.ResourceManager.GetString("ErrorSearchingGroupException"), ex);
			}
		}
		
		/// <summary>
		/// ���������� ����������� ������ �� AD, ����� ������� ������������� ��������� �������
		/// </summary>
		/// <param name="groupFilter">������ �� ����� ������.</param>
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
					catch (ArgumentException) { } // �������� ������ ������
					return groups.GetPage( args );
				}
			}
			catch (Exception ex)
			{
                throw new CoreApplicationException(Resources.ResourceManager.GetString("ErrorSearchingGroupException"), ex);
			}
		}

		#endregion

		#region �������������� ������������ �������

		/// <summary>
		/// ���������� ������ �����, ������ ������� �������� ��������� ������������
		/// </summary>
		/// <param name="userName">��� ������������ (login name ��� �������� ������)</param>
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
