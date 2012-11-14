using System;
using System.Collections.Generic;
using System.Text;

using System.DirectoryServices;

namespace Core.Security
{
	/// <summary>
	/// Пользователь Active Directory
	/// </summary>
	public class ADUser
	{
		private static ADUser m_emptyUser;
		/// <summary>
		/// Пустой пользователь
		/// </summary>
		public static ADUser Empty
		{
			get
			{
				if( m_emptyUser == null )
					m_emptyUser = new ADUser();
				return m_emptyUser;
			}
		}

		private string m_login = "";
		private MLString m_name = MLString.Empty;
		private string m_department = "";
		private string m_phone = "";
		private string m_ipPhone = "";
		private string m_email = "";
		private string m_title = "";

		#region Конструкторы
		public ADUser()
		{
			//m_name[CultureManager.Languages.English] = "-";
			//m_name[CultureManager.Languages.Russian] = "-";
		}

		public ADUser( System.DirectoryServices.SearchResult result )
		{
			PopulateUserProperties( result );
		}

		public ADUser( System.DirectoryServices.SearchResult result, Role role )
		{
			PopulateUserProperties( result );
		}

		public ADUser(DirectoryEntry result)
		{
			string[] fld_names = System.Configuration.ConfigurationManager.AppSettings["LdapFields"].Split( ',' );

			if (result.Properties[fld_names[0]].Count != 0) // SAMAccountName
				m_login = (string)result.Properties[fld_names[0]][0];
			string nameEN = string.Empty;
			string nameRU = string.Empty;
			if (result.Properties[fld_names[1]].Count != 0) // displayname
				nameEN = (string)result.Properties[fld_names[1]][0];
			if (result.Properties[fld_names[2]].Count != 0) // description
				nameRU = (string)result.Properties[fld_names[2]][0];
			m_name = new MLString( nameRU, nameEN );
			if (result.Properties[fld_names[3]].Count != 0) // telephoneNumber
				m_phone = (string)result.Properties[fld_names[3]][0];
			if (result.Properties[fld_names[4]].Count != 0) // department
				m_department = (string)result.Properties[fld_names[4]][0];
			if (result.Properties[fld_names[5]].Count != 0) // ipPnone
				m_ipPhone = (string)result.Properties[fld_names[5]][0];
			if (result.Properties[fld_names[6]].Count != 0) // mail
				m_email = (string)result.Properties[fld_names[6]][0];
			if (result.Properties[fld_names[7]].Count != 0) // title
				m_title = (string)result.Properties[fld_names[7]][0];
		}

		public ADUser(ADUser user)
		{
			m_login = user.m_login;
			m_name = user.m_name;
			m_department = user.m_department;
			m_phone = user.m_phone;
			m_ipPhone = user.m_ipPhone;
			m_email = user.m_email;
			m_title = user.m_title;
		}

		#endregion
		
		/// <summary>
		/// Заполняет свойства пользователя по по результатам поиска в AD.
		/// </summary>
		/// <param name="result"></param>
		protected void PopulateUserProperties( System.DirectoryServices.SearchResult result )
		{
			string[] fld_names = System.Configuration.ConfigurationManager.AppSettings["LdapFields"].Split( ',' );

			if (result.Properties[fld_names[0]].Count != 0) // SAMAccountName
				m_login = (string)result.Properties[fld_names[0]][0];
			string nameEN = string.Empty;
			string nameRU = string.Empty;
			if(result.Properties[fld_names[1]].Count != 0) // displayname
				nameEN = (string)result.Properties[fld_names[1]][0];
			if(result.Properties[fld_names[2]].Count != 0) // description
				nameRU = (string)result.Properties[fld_names[2]][0];
			m_name = new MLString( nameRU, nameEN );
			if (result.Properties[fld_names[3]].Count != 0) // telephoneNumber
				m_phone = (string)result.Properties[fld_names[3]][0];
			if (result.Properties[fld_names[4]].Count != 0) // department
				m_department = (string)result.Properties[fld_names[4]][0];
			if (result.Properties[fld_names[5]].Count != 0) // ipPnone
				m_ipPhone = (string)result.Properties[fld_names[5]][0];
			if (result.Properties[fld_names[6]].Count != 0) // mail
				m_email = (string)result.Properties[fld_names[6]][0];
			if (result.Properties[fld_names[7]].Count != 0) // title
				m_title = (string)result.Properties[fld_names[7]][0];
		}

		/// <summary>
		/// Возвращает имя свойства AD по заданному имени свойства объекта
		/// </summary>
		public static string GetADPropertyName( string obj_pname )
		{
			string[] fld_names = System.Configuration.ConfigurationManager.AppSettings["LdapFields"].Split( ',' );
			if( obj_pname == "Login" )
				return fld_names[0];
			if( obj_pname == "Name" )
				return CultureManager.CurrentLanguage == CultureManager.Languages.Russian ? fld_names[2] : fld_names[1];
			if( obj_pname == "Phone" )
				return fld_names[3];
			if( obj_pname == "Department" )
				return fld_names[4];
			if( obj_pname == "InternalPhone" )
				return fld_names[5];
			if( obj_pname == "EMail" )
				return fld_names[6];
			if( obj_pname == "Title" )
				return fld_names[7];
			return "";
		}

		public string Login
		{
			get { return m_login; }
		}

		public MLString Name
		{
			get { return m_name; }
		}

		public string Department
		{
			get { return m_department; }
		}

		public string Phone
		{
			get { return m_phone; }
		}

		public string InternalPhone
		{
			get { return m_ipPhone; }
		}

		public string EMail
		{
			get { return m_email; }
		}

		public string Title
		{
			get { return m_title; }
		}

		#region Принадлежность группам

		/// <summary>
		/// Проверяет принадлежность пользователя заданной группе.
		/// </summary>
		/// <param name="group">Группа, на принадлежность к которой следует проверить.</param>
		/// <returns></returns>
		public bool IsInGroup( ADGroup group )
		{
			LdapAuthentication ldap = new LdapAuthentication();
			ADGroupCollection userGroups = ldap.GetUserGroupsMembeship( Login );
			return (userGroups.FindGroupByDN( group.DN ) != null);
		}

		#endregion
	}
}
