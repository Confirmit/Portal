using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;

namespace Core.Security
{
	public class ADGroup
	{
		#region Свойства

		private string m_dn = String.Empty;
		public string DN
		{
			get { return m_dn; }
			set { m_dn = value; }
		}

		private string m_name = String.Empty;
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		private string m_description = String.Empty;
		public string Description
		{
			get { return m_description; }
			set { m_description = value; }
		}

		#endregion

		#region Конструкторы

		public ADGroup()
		{
		}

		public ADGroup( System.DirectoryServices.SearchResult searchResult )
		{
			string[] adFields = ConfigurationManager.AppSettings["LdapFields"].Split( ',' );

			if (searchResult.Properties[adFields[9]].Count != 0) // distinguishedname
				m_dn = (string)searchResult.Properties[adFields[9]][0];
			if (searchResult.Properties[adFields[8]].Count != 0) // name
				m_name = (string)searchResult.Properties[adFields[8]][0];
			if (searchResult.Properties[adFields[2]].Count != 0) // description
				m_description = (string)searchResult.Properties[adFields[2]][0];
		}

		#endregion

		/// <summary>
		/// Возвращает имя свойства AD по заданному имени свойства объекта
		/// </summary>
		public static string GetADPropertyName( string propertyName )
		{
			string[] adFields = ConfigurationManager.AppSettings["LdapFields"].Split( ',' );

			switch (propertyName)
			{
				case "DN": return adFields[9];
				case "Name": return adFields[8];
				case "Description": return adFields[2];
				default: return String.Empty;
			}
		}

	}
}
