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
/// Интерфейс, определяющий методы диалога, доступные извне.
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
	/// Возвращает роли, для которых доступно созранение объекта (кнопки).
	/// Если список пуст, то сохранение доступно всем.
	/// </summary>
	Role[] AllowedRolesToSave
	{
		get;
	}
}
