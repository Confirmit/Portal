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
using System.ComponentModel;

using Core;
using Core.Security;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// ������.
	/// ���� ����������� ��������� �������������� ��� ������������ �����.
	/// </summary>
	public class CheckBox : System.Web.UI.WebControls.CheckBox, IAccessible
	{
		#region ��������
		/// <summary>
		/// ������ �����, ��� ������� ��������� ��������������. 
		/// ������, � ������� ����� ������� ����������� �������� �����. 
		/// ���� �������� ������������� ��� ������, �� ������� ����� �������� ����.
		/// </summary>
		[Browsable( true )]
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

		#endregion

		#region IAccessible Members

		public bool CheckAccessibilityToUser( User user )
		{
			bool isAccessible = !String.IsNullOrEmpty( AllowedRoles ) ? user.IsInRoles( AllowedRoles ) : true;
			if(!isAccessible)
			{
				Enabled = false;
			}

			return isAccessible;
		}

		#endregion
	}
}