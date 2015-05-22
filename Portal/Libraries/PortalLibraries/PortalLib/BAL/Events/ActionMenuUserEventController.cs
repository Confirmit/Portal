using System;
using System.Xml;

using UIPProcess.Controllers.ActionMenu;
using UlterSystems.PortalLib.BusinessObjects;
using Navigator=UIPProcess.UIP.Navigators.Navigator;

namespace ConfirmIt.PortalLib.BAL.Events
{
    public class ActionMenuUserEventController : ActionMenuControllerBase<UserEvent>
    {
        public ActionMenuUserEventController(Navigator navigator)
            : base(navigator)
        {}

        public override void RemoveDisallowedActions(XmlNode parent)
        {
            Person currentUser = (Person) ProcessedControl.PageInterface.CurrentWorkingUser;
            bool action_allowed = SelectedEntity != null
                                      ? currentUser.IsCanEditEvent(SelectedEntity)
                                      : currentUser.IsInRole(RolesEnum.Administrator);

            for (Int32 i = parent.ChildNodes.Count - 1; i >= 0; i--)
            {
                XmlNode item = parent.ChildNodes[i];

                if (item == null || !item.Name.Equals("node"))
                    continue;

                if (item.Attributes[_aclActionTypeIDAttribute] != null)
                {
                    try
                    {
                        string actionType = item.Attributes[_aclActionTypeIDAttribute].Value;
                        if (!action_allowed && actionType.Equals("delete"))
                        {
                            parent.RemoveChild(item);
                            continue;
                        }
                    }
                    catch (Exception)
                    {}
                }
            }
        }
    }
}