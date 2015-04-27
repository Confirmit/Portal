using System;
using System.Xml;

using UIPProcess.Controllers.ActionMenu;
using UlterSystems.PortalLib.BusinessObjects;
using Navigator=UIPProcess.UIP.Navigators.Navigator;
using ConfirmIt.PortalLib.BAL;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.RequestObjects.Controllers
{
    public class AdminActionMenuController : ActionMenuControllerBase
    {
        #region [ Constructors ]

        public AdminActionMenuController(Navigator navigator)
            : base(navigator)
        { }

        #endregion

        public override void RemoveDisallowedActions(XmlNode parent)
        {
            Person currentUser = (Person)ProcessedControl.PageInterface.CurrentWorkingUser;
            bool action_allowed = currentUser.IsInRole(RolesEnum.Administrator);

            int count = parent.ChildNodes.Count;
            int deleted_count = 0;

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
                        if (!action_allowed) // && actionType.Equals("delete"))
                        {
                            parent.RemoveChild(item);
                            deleted_count++;
                            continue;
                        }
                    }
                    catch (Exception)
                    { }
                }
            }

            if (deleted_count == count)
                ProcessedControl.Visible = false;
        }
    }
}