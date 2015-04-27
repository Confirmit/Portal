using System;
using System.Collections.Generic;

using UIPProcess.UIP.Navigators;
using UIPProcess.Controllers;
using UlterSystems.PortalLib.BusinessObjects;

using Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;
using ConfirmIt.PortalLib.BAL;

namespace ConfirmIt.PortalLib.BusinessObjects.RequestObjects.Controllers
{
    public class RequestObjectFilterCtlController<TFilter> : FilterViewController<TFilter>
        where TFilter : IEntitiesListFilter, new()
    {
        #region [ Constructors ]

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navigator"></param>
        public RequestObjectFilterCtlController(UIPProcess.UIP.Navigators.Navigator navigator)
            : base(navigator)
        { }

        #endregion 

        #region [ Methods ]

        protected override void LoadSysParameters()
        {
            if(Offices == null)
                Offices = ConfirmIt.PortalLib.BAL.Office.GetAllOffices();

            base.LoadSysParameters();
        }

        #endregion

        #region [ State Mapped Property ]

        public virtual ConfirmIt.PortalLib.BAL.Office[] Offices
        {
            get { return null; }
            set { }
        }

        #endregion
    }

    public class BookFilterCtlController : RequestObjectFilterCtlController<BookFilter>
    {
        #region [ Constructors ]

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navigator"></param>
        public BookFilterCtlController(UIPProcess.UIP.Navigators.Navigator navigator)
            : base(navigator)
        { }

        #endregion

        #region [ Methods ]

        protected override void LoadSysParameters()
        {
            Languages = Book.GetLanguages();
            BookThemes = BookTheme.GetAllBookThemes();

            base.LoadSysParameters();
        }

        #endregion

        #region [ State Mapped Property ]

        public virtual string[] Languages
        {
            get { return null; }
            set { }
        }

        public virtual BookTheme[] BookThemes
        {
            get { return null; }
            set { }
        }

        #endregion
    }

    public class CardFilterCtlController : RequestObjectFilterCtlController<CardFilter>
    {
        #region [ Constructors ]

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navigator"></param>
        public CardFilterCtlController(UIPProcess.UIP.Navigators.Navigator navigator)
            : base(navigator)
        { }

        #endregion
    }

    public class DiskFilterCtlController : RequestObjectFilterCtlController<DiskFilter>
    {
        #region [ Constructors ]

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navigator"></param>
        public DiskFilterCtlController(UIPProcess.UIP.Navigators.Navigator navigator)
            : base(navigator)
        { }

        #endregion
    }
}