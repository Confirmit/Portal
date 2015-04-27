using System.Collections.Generic;

using UIPProcess.Controllers;

using Core;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;
using UlterSystems.PortalLib.BusinessObjects;

namespace ConfirmIt.PortalLib.BusinessObjects.RequestObjects.Controllers
{
    public class RequestObjectInfoController<TEntity> : EntityViewController<TEntity>
        where TEntity : BasePlainObject, new ()
    {
        #region [ Constructors ]

        public RequestObjectInfoController(UIPProcess.UIP.Navigators.Navigator navigator)
            : base(navigator)
        { }

        #endregion

        protected override void LoadSysParameters()
        {
            if (Owners == null)
                Owners = UserList.GetYaroslavlOfficeUsersList();

            base.LoadSysParameters();
        }

        /// <summary>
        /// State mapped property
        /// </summary>
        public virtual IList<Person> Owners
        {
            get { return null; }
            set { }
        }
    }

    public class BooksInfoController : RequestObjectInfoController<Book>
    {
        #region [ Constructors ]

        public BooksInfoController(UIPProcess.UIP.Navigators.Navigator navigator)
            : base(navigator)
        { }

        #endregion
    }

    public class DiskInfoController : RequestObjectInfoController<Disk>
    {
        #region [ Constructors ]

        public DiskInfoController(UIPProcess.UIP.Navigators.Navigator navigator)
            : base(navigator)
        { }

        #endregion
    }

    public class CardInfoController : RequestObjectInfoController<Card>
    {
        #region [ Constructors ]

        public CardInfoController(UIPProcess.UIP.Navigators.Navigator navigator)
            : base(navigator)
        { }

        #endregion
    }
}