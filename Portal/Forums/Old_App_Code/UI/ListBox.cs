using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// ������, ������� ��������� ���� �������� � ������
	/// </summary>
	public class ListBox : System.Web.UI.WebControls.ListBox
	{
		public override void DataBind()
		{
			base.DataBind();
			if( IsPermanent )
				if( Page.Session[UniqueSessionKey] != null )
					this.Values = (List<string>)Page.Session[UniqueSessionKey];
		}

		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );
			if( IsPermanent )
				Page.Session[UniqueSessionKey] = Values;
		}

		/// <summary>
		/// ���������� ��������
		/// </summary>
		public List<string> Values
		{
			get
			{
				List<string> values = new List<string>();
				foreach( ListItem item in this.Items )
					if( item.Selected )
						values.Add( item.Value );
				return values;
			}
			set
			{
				foreach( ListItem item in this.Items )
					if( value.IndexOf( item.Value ) != -1 )
						item.Selected = true;
					else
						item.Selected = false;
			}
		}

		private bool m_permanent = false;
		/// <summary>
		/// ��������� �� ������� �������� � ������ (�� ��������� - false)
		/// </summary>
		public bool IsPermanent
		{
			get
			{
				return m_permanent;
			}
			set
			{
				m_permanent = value;
			}
		}

		/// <summary>
		/// ���������� � �������� ������� ���� ��� ������� �������� ��� ���������� ������ � ������
		/// </summary>
		protected string UniqueSessionKey
		{
			get
			{
				return BaseUserControl.GetUniqueSessionKey( this );
			}
		}
	}

}