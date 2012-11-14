using System;

using UIPProcess.Controllers;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;
using UIPProcess.UIP.Navigators;

namespace ConfirmIt.PortalLib.BusinessObjects.RequestObjects.Controllers
{
    public class BookThemeInfoCtlController : EntityViewController<BookTheme>
    {
        public BookThemeInfoCtlController(Navigator navigator)
            : base(navigator)
        { }
    }
}