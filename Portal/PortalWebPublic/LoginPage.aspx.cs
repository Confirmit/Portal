using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;

using ConfirmIt.PortalLib.BAL;
using UlterSystems.PortalLib.BusinessObjects;

public partial class LoginPage : System.Web.UI.Page
{
	#region ����������� �������

	/// <summary>
	/// ���������� �������� ��������.
	/// </summary>
	protected void Page_Load(object sender, EventArgs e)
	{
        // ������� ���������� � ������������.
		if (!IsPostBack)
			Session["UserID"] = null;
	}

	/// <summary>
	/// ���������� �������������� ������������.
	/// </summary>
    protected virtual void OnLogin(object sender, EventArgs e)
	{
        if (String.IsNullOrEmpty(logIn.UserName) || HttpContext.Current == null)
	        return;

		Person currentUser = new Person(HttpContext.Current.User.Identity);
        if (!currentUser.LoadByDomainName(logIn.UserName))
            return;

        if (!currentUser.IsInRole(RolesEnum.PublicUser))
        {
            logIn.FailureText = "User cant work with the public portal.";
            return;
        }

	    IList<PersonAttribute> attributes =
	        PersonAttributes.GetPersonAttributesByKeyword(currentUser.ID.Value
	                                                      , PersonAttributeTypes.PublicPassword.ToString());

        if (logIn.Password == (string)attributes[0].Value)
        {
            Session["UserID"] = currentUser.ID.Value;
            FormsAuthentication.RedirectFromLoginPage(logIn.UserName, logIn.RememberMeSet);
        }
	    logIn.FailureText = "Incorrect information.";
	}

    #endregion
}
