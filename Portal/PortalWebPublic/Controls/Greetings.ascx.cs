using System;

using UlterSystems.PortalLib.BusinessObjects;

public partial class Controls_Greetings : System.Web.UI.UserControl
{
	#region Fields

	protected Person m_CurrentUser;

	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{
		// Получить текущего пользователя.
		Person curUser = new Person();
		if (Session["UserID"] != null)
		{ 
			curUser.Load((int)Session["UserID"]);
            //FIXME: very interessant part of code
            if (((string)GetLocalResourceObject("Greetings.Text")).Contains("{0}"))
                locGreetings.Text = String.Format((string)GetLocalResourceObject("Greetings.Text"),
                                              curUser.FullName);
            else
            locGreetings.Text = String.Format((string)GetLocalResourceObject("Greetings.Text")
                                              + " {0}", curUser.FullName);
		}
		else
		    Visible = false;
	}
}
