using System;
using System.Configuration;
using System.Globalization;
using System.Web.Configuration;
using UlterSystems.PortalLib.BusinessObjects;

public partial class ErrorPages_AccessDenied : BaseWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string adminMail = ""; //=WebConfigurationManager.AppSettings["AdminsMails"];
        string adminName = "";//= WebConfigurationManager.AppSettings["AdminsNames" + CultureInfo.CurrentCulture.TwoLetterISOLanguageName];
        var managers = UserList.GetHrManagerList();
       
        foreach (var manager in managers)
        {
            adminMail += manager.PrimaryEMail;
            adminName += manager.FullName;
        }
       

        string message = string.Format(lblErrorDescription.Text, adminMail, adminName);
     
        lblErrorDescription.Text = message;


                //string.Format(lblErrorDescription.Text, userName);
    }
}

 
