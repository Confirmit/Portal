using System;
using System.Linq;
using System.Collections.Generic;

using UIPProcess.DataBinding;
using UIPProcess.Controllers;
using UIPProcess.UIP.Navigators;

using Confirmit.PortalLib.BusinessObjects.RequestObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.RequestObjects.Controllers
{
    public class ReqObjectEditCtlController : WebCtlControllerBase<RequestObject>
    {
        #region [ Constructors ]

        public ReqObjectEditCtlController(Navigator navigator)
            : base (navigator)
        { }

        #endregion

        public override void OnLoadView(object sender, EventArgs e)
        {
            base.OnLoadView(sender, e);

            var dict = new Dictionary<string, object>();
            DataBinder.DataBindFromControlAttributedProps(ProcessedControl, dict);
            RequestObjectType = (Type)dict.First().Value;
        }

        public void OnActiveTabChanged(object sender, int headerIndex)
        {
            SelectedEntity = null;
            ViewStorage = null;
        }

        public virtual Type RequestObjectType
        {
            get { return null; }
            set { }
        }

        public virtual RequestObject SelectedEntity
        {
            get { return null; }
            set { }
        }

        public virtual object ViewStorage
        {
            get { return null; }
            set { }
        }
    }
}