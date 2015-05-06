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
	/// PlaceHolder, в который должны быть помещены контролы страницы, предназначенные для печати
	/// </summary>
	public class PrintPlaceHolder : System.Web.UI.WebControls.PlaceHolder
	{
		protected override void OnLoad( EventArgs e )
		{
			( (BaseWebPage)Page ).HasPrintMode = true;
			base.OnLoad( e );
		}
	}
}
