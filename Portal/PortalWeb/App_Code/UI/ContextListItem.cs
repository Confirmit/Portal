using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Core;
using Core.Security;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// ������� ����� ��� �������� ����������� ������, ������� ���������� � ����������� ����.
	/// </summary>
	public abstract class ContextListItem : Control
	{
		#region �����������

		public ContextListItem()
		{
		}

		#endregion

		#region ��������
		/// <summary>
		/// ����� ����������� �������.
		/// </summary>
		[Browsable(true)]
		[Localizable( true )]
		[DefaultValue( "" )]
		public abstract string Text
		{
			get;
			set;
		}

		/// <summary>
		/// ����, ��� ������� �������� �������. ���� �� �������, �� ������� �������� ����.
		/// </summary>
		[Browsable( true )]
		[DefaultValue( "" )]
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
		/// ����������, �������� �� ������� ��� �������� ������������.
		/// ������� ���������� ���������� �������� AllowedRoles ��� �������� ����������� �������.
		/// </summary>
		/// <returns></returns>
		public virtual bool IsAccessible
		{
			get
			{
				if(String.IsNullOrEmpty(AllowedRoles))
				{
					return true;
				}

				return User.Current.IsInRoles( AllowedRoles );
			}
		}

		#endregion
	}
}