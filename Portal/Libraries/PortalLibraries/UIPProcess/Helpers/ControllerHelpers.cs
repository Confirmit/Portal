using System;
using UIPProcess.UIP.Views;

namespace UIPProcess.Helpers
{
    public class ControllerActionHelper
    {
        public static String GetNavigateCommand(IWebControl control)
        {
            return control.GetFormFieldValue(control.ActionsMenuClientId + "_navigate");
        }

        public static String GetActionFieldValue(IWebControl control)
        {
            return control.GetFormFieldValue(control.ActionsMenuClientId + "_action");
        }

        public static String GetActionParameterValue(IWebControl control)
        {
            return control.GetFormFieldValue(control.ActionsMenuClientId + "_parameter");
        }

        public static Boolean IsSaveAndNewAction(IWebControl control)
        {
            String action = GetActionFieldValue(control);
            return !String.IsNullOrEmpty(action) && action.Equals("save_and_new");
        }

        public static Boolean IsAddAction(IWebControl control)
        {
            String action = GetActionFieldValue(control);
            return !String.IsNullOrEmpty(action) && action.Equals("add");
        }

        public static Boolean IsDeleteAction(IWebControl control)
        {
            String action = GetActionFieldValue(control);
            return !String.IsNullOrEmpty(action) && action.Equals("delete");
        }

        public static Boolean IsCloseAction(IWebControl control)
        {
            String action = GetActionFieldValue(control);
            return !String.IsNullOrEmpty(action) && action.Equals("close");
        }

        public static Boolean IsSaveAction(IWebControl control)
        {
            String action = GetActionFieldValue(control);
            return !String.IsNullOrEmpty(action) && action.Equals("save");
        }

        public static Boolean IsSaveAsNewAction(IWebControl control)
        {
            String action = GetActionFieldValue(control);
            return !String.IsNullOrEmpty(action) && action.Equals("savenew");
        }
    }
}