using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// ���������, �������������� ��������� � �������� ����������.
	/// </summary>
	public interface IGridSelection
	{
		/// <summary>
		/// ����������� ��������� ���������.
		/// </summary>
		int SelectionCount { get;}
	}

}
