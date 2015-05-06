using System;
using System.Xml;

using Core;

using UIPProcess.UIP.Navigators;
using UIPProcess.UIP.Views;

namespace UIPProcess.Controllers.ActionMenu
{
    public interface IActionMenuControllerBase
    {
        void RemoveDisallowedActions(XmlNode parent);
    }

    public abstract class ActionMenuControllerBase<TEntity> : ActionMenuControllerBase
    where TEntity : BasePlainObject, new()
    {
        #region [ Constructors ]

        public ActionMenuControllerBase(Navigator navigator)
            : base(navigator)
        { }

        #endregion

        #region [ Properties ]

        public new virtual TEntity SelectedEntity
        {
            get { return null; }
            set { }
        }

        #endregion
    }

    public abstract class ActionMenuControllerBase : WebCtlControllerBase<IWebControl>, IActionMenuControllerBase
    {
        #region [ Constructors ]

        public ActionMenuControllerBase(Navigator navigator)
            : base(navigator)
        { }

        #endregion

        #region [ Fields ]

        protected static readonly String _aclActionTypeIDAttribute = "acl-action-type";

        #endregion

        #region [ Methods ]

        public abstract void RemoveDisallowedActions(XmlNode parent);

        #endregion

        #region [ Properties ]

        public virtual object SelectedEntity
        {
            get { return null; }
            set { }
        }

        #endregion
    }
}