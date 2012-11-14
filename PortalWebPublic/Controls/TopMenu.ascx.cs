using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Controls_TopMenu : System.Web.UI.UserControl
{
	#region Обработчики событий
	/// <summary>
	/// Обработчик загрузки элемента управления.
	/// </summary>
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["UserID"] == null)
			Visible = false;
	}

	/// <summary>
	/// Обработчик выхода с сайта.
	/// </summary>
	protected virtual void OnLogOut(object sender, EventArgs e)
	{
		FormsAuthentication.SignOut();
		Session["UserID"] = null;
		Response.Redirect( hlHome.NavigateUrl );
	}
	#endregion
}
