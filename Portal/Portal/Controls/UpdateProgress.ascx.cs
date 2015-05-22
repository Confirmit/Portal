using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

public partial class Controls_UpdateProgress : System.Web.UI.UserControl
{
	/// <summary>
	/// Имя update-панели, прогресс обновления которой отображается.
	/// </summary>
	public string AssociatedUpdatePanelID
	{
		get 
		{ 
			return progress.AssociatedUpdatePanelID; 
		}
		set 
		{ 
			progress.AssociatedUpdatePanelID = value; 
		}
	}
}
