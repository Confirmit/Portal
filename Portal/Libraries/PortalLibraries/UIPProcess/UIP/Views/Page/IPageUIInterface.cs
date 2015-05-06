using System;
using System.Web;
using System.Web.SessionState;

using UIPProcess.Controllers;

namespace UIPProcess.UIP.Views.Page
{
    public interface IPageUIInterface
    {
        Boolean IsPostBack { get; }
        HttpRequest Request { get;}
        HttpSessionState Session { get;}
        void Validate();
        void Validate(String validate);
        bool IsValid { get;}

        string MapPath(String str);

        ControllerBase Controller { get;}

        String ActionsMenuClientId { get; set; }
        String GetFormFieldValue(String strFieldName);

        object CurrentWorkingUser { get; }
    }
}