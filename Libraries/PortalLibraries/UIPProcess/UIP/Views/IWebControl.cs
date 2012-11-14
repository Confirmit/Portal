using System;

using UIPProcess.UIP.Views.Page;

namespace UIPProcess.UIP.Views
{
    public interface IWebControl
    {
        Boolean IsPostBack { get; }
        Boolean Visible { get; set; }

        String ClientID { get; }
        String UniqueID { get; }

        String ActionsMenuClientId { get; set; }
        String GetFormFieldValue(String strFieldName);
        String GetCommandValue(String strCmdName);

        IPageUIInterface PageInterface { get; }    
        Boolean Validate();
    }
}