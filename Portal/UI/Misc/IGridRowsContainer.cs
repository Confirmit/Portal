using System;
using System.Web.UI.WebControls;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// ���������, �������������� �������� ����������, ���������� ������.
	/// </summary>
	public interface IGridRowsContainer
	{
		/// <summary>
		/// ���������� ��������� ����� �������� ����������.
		/// </summary>
		GridViewRowCollection Rows { get; }
		/// <summary>
		/// ���������� ������ ��������� �������� ����������.
		/// </summary>
		GridViewRow HeaderRow { get;}
		/// <summary>
		/// ��������� ������ ��������� �����
		/// </summary>
		DataKeyArray SelectedKeys { get;}
	}

}