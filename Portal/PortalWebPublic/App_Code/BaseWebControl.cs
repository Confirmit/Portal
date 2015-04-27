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
/// Ѕазовый класс дл€ контролов.
/// </summary>
public abstract class BaseWebControl : System.Web.UI.WebControls.WebControl
{
	/// <summary>
	/// ¬озвращает страницу BaseWebPage, на которой лежит данный контрол.
	/// </summary>
	public new BaseWebPage Page
	{
		get
		{
			return (BaseWebPage)base.Page;
		}
	}

	/// <summary>
	/// ѕровер€ет данные на коректность.
	/// </summary>
	public virtual bool IsValid
	{
		get { return true; }
	}

	/// <summary>
	/// ”никальный в пределах системы ключ дл€ данного контрола дл€ сохранени€ данных в сессии
	/// </summary>
	protected string UniqueSessionKey
	{
		get
		{
			return GetUniqueSessionKey( this );
		}
	}

	/// <summary>
	/// ”никальный в пределах системы ключ дл€ данного контрола дл€ сохранени€ данных в сессии
	/// </summary>
	public static string GetUniqueSessionKey( Control control )
	{
		string page_path = HttpContext.Current.Request.Url.LocalPath;
		page_path = page_path.Replace( "ServerList", "ComputerList" );
		page_path = page_path.Replace( "WorkstationList", "ComputerList" );
		return "controlstate_" + page_path + "_" + control.UniqueID;
	}
}
