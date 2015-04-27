using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

using UIPProcess.UIP.Views;
using DataBinder = UIPProcess.DataBinding.DataBinder;

/// <summary>
/// ������� ����� ��� ���������������� ��������� (UserControl), 
/// ������� ����������� ��������������� �� ��������, ����������� �� BaseWebPage.
/// </summary>
public abstract class BaseUserControl : ControlViewBase //UserControl
{
	/// <summary>
    /// ���������� �������� BaseWebPage, �� ������� ����� ������ �������.
    /// </summary>
    public new BaseWebPage Page
    {
        get { return (BaseWebPage)base.Page; }
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
		get { return GetUniqueSessionKey( this ); }
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

    #region State Mapped Property

    public virtual object SelectedEntity
    {
        set
        {
            _selectedEntity = value;

            if (_viewStorage == null)
                DataBinder.DataBindToControlAttributedProps(value, this);
        }
    }
    protected object _selectedEntity = null;

    public virtual IDictionary<String, Object> ViewStorage
    {
        set
        {
            _viewStorage = value;

            if (_viewStorage != null && _viewStorage.Count > 0)
                DataBinder.DataBindToControlAttributedProps(value, this);
        }
    }
    protected IDictionary<string, object> _viewStorage = null;

    #endregion
}