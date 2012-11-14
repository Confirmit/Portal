using System;

using UIPProcess.Controllers.GridView;
using UlterSystems.PortalLib.BusinessObjects;
using Navigator = UIPProcess.UIP.Navigators.Navigator;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.RequestObjects.Controllers
{
    public class BookThemesListController : GridViewDataSourceController<BookTheme>
    {
        public BookThemesListController(Navigator navigator)
            : base(navigator)
        { }
    }
}