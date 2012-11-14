using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// ������� ����� ��� ���������������� ��������� (UserControl), 
/// ������� ����������� ��������������� �� ��������, ����������� �� BaseWebPage.
/// </summary>
public abstract class BaseUserControl : System.Web.UI.UserControl
{
	/// <summary>
    /// ���������� �������� BaseWebPage, �� ������� ����� ������ �������.
    /// </summary>
    public new BaseWebPage Page
    {
        get
        {
            return (BaseWebPage)base.Page;
        }
    }

	/// <summary>
	/// ��������� ������ �� �����������.
	/// </summary>
	public virtual bool IsValid
	{
		get { return true; }
	}

	/// <summary>
	/// ���������� � �������� ������� ���� ��� ������� �������� ��� ���������� ������ � ������
	/// </summary>
	protected string UniqueSessionKey
	{
		get
		{
			return GetUniqueSessionKey( this );
		}
	}

	/// <summary>
	/// ���������� � �������� ������� ���� ��� ������� �������� ��� ���������� ������ � ������
	/// </summary>
	public static string GetUniqueSessionKey( Control control )
	{
		string page_path = HttpContext.Current.Request.Url.LocalPath;
		page_path = page_path.Replace( "ServerList", "ComputerList" );
		page_path = page_path.Replace( "WorkstationList", "ComputerList" );
		return "controlstate_" + page_path + "_" + control.UniqueID;
	}
}
