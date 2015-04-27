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
	public class BoundField : BaseBoundField
	{
		#region ������������

		public BoundField()
		{
			HtmlEncode = false;
		}

		#endregion

		#region ��������
		/// <summary>
		/// �������� �� ����� ������ �� �������� ����.
		/// </summary>
        public bool HasEmailReference
        {
            get
            {
				object o = ViewState["HasEmailReference"];
				return o != null ? (bool)o : false;
            }
            set
            {
				ViewState["HasEmailReference"] = value;
            }
        }

		/// <summary>
		/// ������������� ������� � ����� Messages.resx, ������� ������������ ����� ��������
		/// </summary>
		public string PostfixMessageResourceName
		{
			get 
			{
				return (string)ViewState["PostfixMessageResourceName"]; 
			}
			set 
			{
				ViewState["PostfixMessageResourceName"] = value; 
			}
		}
		

		/// <summary>
		/// �������� �����, ������� ��������� �������� ����������� �� �������.
		/// �� ��������� - �� ���������.
		/// </summary>
		public int ValueMaxLength
		{
			get
			{
				object o = ViewState["ValueMaxLength"];
				return o != null ? (int)o : 0;
			}
			set
			{
				ViewState["ValueMaxLength"] = value;
			}
		}

		#endregion

		protected override object GetValue( Control controlContainer )
		{
			object data_object = base.GetValue( controlContainer );

			if(!base.DesignMode)
			{
				// ������������ ��������� ���� ��������, ���������� ����� ������ �������
				if(data_object is DateTime)
				{
					DateTime date = (DateTime)data_object;
					return date.ToShortDateString();
				}
				if(data_object is DateTime?)
				{
					DateTime? date = (DateTime?)data_object;
					return date.HasValue ? date.Value.ToShortDateString() : String.Empty;
				}

				string result = data_object.ToString();
				// ������������ �������� ������
				if(ValueMaxLength != 0 && result.Length > ValueMaxLength)
				{
					result = SplitString( result, ValueMaxLength );
				}

				if(!String.IsNullOrEmpty( PostfixMessageResourceName )
					&& !String.IsNullOrEmpty( result ))
				{
					result += " " + (string)HttpContext.GetGlobalResourceObject( "Messages", PostfixMessageResourceName );
				}

				if(HasEmailReference)//���� ���� �������� ������ �� mail, �� ������ �� �������
				{
					result = Regex.Replace( result, @"([\w-\.]+)@(([\w-]+\.){1,20})([a-zA-Z]{2,4})", "<a href = mailto:${0}>${0}</a>" );
				}
				return result;
			}
			else
				return this.GetDesignTimeValue();
		}

		/// <summary>
		/// ��������� ������ �� ������, ���, ����� ����� ��������� ������� ���� �������������� ����� ��������� �����
		/// </summary>
		/// <param name="str"></param>
		/// <param name="lenght"></param>
		/// <returns></returns>
		private string SplitString( string str, int lenght )
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			string[] words = str.Split( new char[] { ' ' }, StringSplitOptions.None );
			int char_count = 0;
			foreach( string word in words )
			{
				result.Append( word + " " );
				char_count += word.Length + 1;
				if( char_count > lenght )
				{
					result.Append( "<br>" );
					char_count = 0;
				}
			}
			return result.ToString();
		}
	}
}