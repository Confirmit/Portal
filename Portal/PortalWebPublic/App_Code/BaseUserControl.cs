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
/// Базовый класс для пользовательских контролов (UserControl), 
/// которые добавляются непосредственно на страницы, порожденные от BaseWebPage.
/// </summary>
public abstract class BaseUserControl : System.Web.UI.UserControl
{
	/// <summary>
    /// Возвращает страницу BaseWebPage, на которой лежит данный контрол.
    /// </summary>
    public new BaseWebPage Page
    {
        get
        {
            return (BaseWebPage)base.Page;
        }
    }

	/// <summary>
	/// Проверяет данные на коректность.
	/// </summary>
	public virtual bool IsValid
	{
		get { return true; }
	}

	/// <summary>
	/// Уникальный в пределах системы ключ для данного контрола для сохранения данных в сессии
	/// </summary>
	protected string UniqueSessionKey
	{
		get
		{
			return GetUniqueSessionKey( this );
		}
	}

	/// <summary>
	/// Уникальный в пределах системы ключ для данного контрола для сохранения данных в сессии
	/// </summary>
	public static string GetUniqueSessionKey( Control control )
	{
		string page_path = HttpContext.Current.Request.Url.LocalPath;
		page_path = page_path.Replace( "ServerList", "ComputerList" );
		page_path = page_path.Replace( "WorkstationList", "ComputerList" );
		return "controlstate_" + page_path + "_" + control.UniqueID;
	}
}
