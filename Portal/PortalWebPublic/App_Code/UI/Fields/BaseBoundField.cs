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
using System.Text.RegularExpressions;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// ������� �������, ����������� ������ ������� ��� �������� � �������������.
	/// </summary>
	public abstract class BaseBoundField : System.Web.UI.WebControls.BoundField, IAccessible
	{
		#region ��������

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

		#endregion

		protected override object GetValue( Control controlContainer )
		{
			object data_object = null;
			string data_field_name = this.DataField;

			if(controlContainer == null)
			{
				throw new ApplicationException( "DataControlField_NoContainer" );
			}

			data_object =  DataBinder.GetDataItem( controlContainer );
			if((data_object == null) && !base.DesignMode)
			{
				throw new ApplicationException( "DataItem_Not_Found" );
			}

			if( !data_field_name.Equals( BoundField.ThisExpression ) )
			{
				data_object = GetPropertyExpressionValue( data_object, data_field_name );
			}

			if(!base.DesignMode)
			{
				return data_object;
			}
			else
			{
				return this.GetDesignTimeValue();
			}
		}

		/// <summary>
		/// ���������� �������� ��������� �� ������� 
		/// (��������, �������� ��������, �������� User.ADUser.OfficeName).
		/// </summary>
		/// <param name="obj">������, � �������� ���� ��������� ��������.</param>
		/// <param name="expression">���������.</param>
		/// <returns></returns>
		protected object GetPropertyExpressionValue( object obj, string expression )
		{
			object dataObject = obj;

			string[] props = expression.Split( '.' );
			for(int i = 0; i < props.Length; i++)
			{
				dataObject = GetPropertyValue( dataObject, props[i] );
				if(dataObject == null)
				{
					return String.Empty; // ���������� ������ ������ ��� �������, ����� ��������� �� ����������
				}
			}

			return dataObject;
		}

		/// <summary>
		/// ���������� �������� ���������� �������� �������.
		/// </summary>
		/// <param name="obj">������, � �������� ���� ��������� ��������.</param>
		/// <param name="propertyName">��� ��������, �������� �������� ����� ���������.</param>
		/// <returns></returns>
		protected object GetPropertyValue( object obj, string propertyName )
		{
			PropertyDescriptor boundFieldDesc = TypeDescriptor.GetProperties( obj ).Find( propertyName, true );
			if(boundFieldDesc == null && !DesignMode)
			{
				throw new ApplicationException( String.Format( "Can't find property '{0}'", propertyName ) );
			}

			return boundFieldDesc.GetValue( obj );
		}

		#region IAccessible Members

		public bool CheckAccessibilityToUser( User user )
		{
			return !String.IsNullOrEmpty( AllowedRoles ) ? user.IsInRoles( AllowedRoles ) : true;
		}

		#endregion

	}
}