using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

using Core;
using Core.Security;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// ������� �������, ������������ ������������ �������� � ����������� �� ���.
	/// </summary>
	public class TemplateField : System.Web.UI.WebControls.TemplateField, IAccessible
	{
		/// <summary>
		/// �������� �� ������ ������� ������������ (���� MLString). 
		/// ���� ��������, �� ��������� ��� ���������� ����� ������������� 
		/// �� ������ ���������� ��������, � ����� ��������, ���������������� �������� �����.
		/// </summary>
		public bool IsMultilangual
		{
			get
			{
				object o = ViewState["IsMultilangual"];
				return o != null ? (bool)o : false;
			}
			set
			{
				ViewState["IsMultilangual"] = value;
			}
		}

		/// <summary>
		/// ��������� ��� ����������.
		/// ��� ������������ ������� � ��������� ��������� ����������� ������� �����.
		/// </summary>
		public override string SortExpression
		{
			get
			{
				if(base.SortExpression != String.Empty && IsMultilangual)
				{
					return CultureManager.CurrentLanguage == CultureManager.Languages.Russian
						? "r" + base.SortExpression
						: "e" + base.SortExpression;
				}
				return base.SortExpression;
			}
			set
			{
				base.SortExpression = value;
			}
		}

		/// <summary>
		/// ������ �����, ��� ������� �������� ����� ������ ������. 
		/// ������, � ������� ����� ������� ����������� �������� �����. 
		/// ���� �������� ������������� ��� ������, �� ������� ����� �������� ����.
		/// </summary>
		public string AllowedRoles
		{
			get
			{
				object o = ViewState["AllowedRoles"];
				return o != null ? (string)o : String.Empty;
			}
			set
			{
				ViewState["AllowedRoles"] = value;
			}
		}

		/// <summary>
		/// ����������, ����� �� ������� ���������� �� ������
		/// </summary>
		public bool IsPrintable
		{
			get
			{
				object o = ViewState["IsPrintable"];
				return o != null ? (bool)o : true;
			}
			set
			{
				ViewState["IsPrintable"] = value;
			}
		}

		#region IAccessible Members

		public bool CheckAccessibilityToUser( User user )
		{
			if( this.Control != null )
				if( ( (BaseWebPage)this.Control.Page ).IsInPrintMode && !IsPrintable )
					return false;
			return !String.IsNullOrEmpty( AllowedRoles ) ? user.IsInRoles( AllowedRoles ) : true;
		}

		#endregion
	}
}
