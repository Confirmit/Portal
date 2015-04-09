using System;
using UlterSystems.PortalLib.BusinessObjects;

public partial class ErrorPages_AccessDenied : BaseWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["aspxerrorpath"]))
        {
            string hrManagerMails = "";
            string hrManagerNames = "";
            var managers = UserList.GetHrManagerList();

            foreach (var manager in managers)
            {
                hrManagerMails += string.Format("{0}, ", manager.PrimaryEMail);
                hrManagerNames += string.Format("{0}, ", manager.FullName);
            }

            lblErrorDescription.Text = string.Format(lblErrorDescription.Text, hrManagerMails, hrManagerNames);
        }
    }
}

 
