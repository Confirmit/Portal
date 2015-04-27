using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Core;
using Core.Security;

/// <summary>
/// ���������, ������������ ������ �������, ��������� �����.
/// </summary>
public interface IDialog
{
	void Save();
	void Cancel();
	void Apply();
	void Next();
	object GetCurrentObject();
	string GetUrlWithMarker( string url );
	string GetUrlWithParentMarker( string url );
	void FillObject();
	/// <summary>
	/// ���������� ����, ��� ������� �������� ���������� ������� (������).
	/// ���� ������ ����, �� ���������� �������� ����.
	/// </summary>
	Role[] AllowedRolesToSave
	{
		get;
	}
}
