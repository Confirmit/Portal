using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web;

using UlterSystems.PortalLib.DB;

namespace UlterSystems.PortalLib.BusinessObjects
{
	/// <summary>
	/// ����� ��������� �����.
	/// </summary>
	public class LangTitles
	{
		#region ����
		private Dictionary<string, string> _LangTitles = new Dictionary<string, string>(); //������ ������������ ����� �� ������ ������
		#endregion

		#region ��������
		/// <summary>
		/// ����������.
		/// </summary>
		public string this[string index]
		{
			get { return _LangTitles[index]; }
			set { _LangTitles[index] = value; }
		}
		#endregion
	}


	/// <summary>
	/// ����� ��������� ������.
	/// </summary>
	public class Languages
	{
		#region ����
		private Dictionary<string, LangTitles> _Languages = new Dictionary<string, LangTitles>();
		#endregion

		#region ��������
		/// <summary>
		/// ��������� ������.
		/// </summary>
		public LangTitles this[string index]
		{
			get { return _Languages[index]; }
			set { _Languages[index] = value; }
		}
		#endregion

		#region ������������
		/// <summary>
		/// �����������.
		/// </summary>
		public Languages()
		{
			XmlTextReader reader = null;
			try
			{
				// Load the reader with the data file and ignore all white space nodes.         
				reader = new XmlTextReader(HttpContext.Current.Server.MapPath(@"~\App_Data\portal.xml"));
				reader.WhitespaceHandling = WhitespaceHandling.None;

				while (reader.Read())
				{
					if (reader.Name == "AvailableInterfaceLanguages")
					{
						reader.Read();
						while (reader.Name == "language" && reader.NodeType == XmlNodeType.Element)
						{
							string s = reader.GetAttribute("code");
							LangTitles titles = new LangTitles();

							reader.Read();
							while (reader.Name == "title" && reader.NodeType == XmlNodeType.Element)
							{
								string s1 = reader.GetAttribute("langcode");
								reader.Read();
								titles[s1] = reader.Value;
								reader.Read();
								reader.Read();
							}
							this._Languages[s] = titles;
							reader.Read();
						}
						break;
					}
				}
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}
		}
		#endregion
	}
}
