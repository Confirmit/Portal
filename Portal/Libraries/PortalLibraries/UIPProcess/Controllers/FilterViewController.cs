using System;
using System.Collections.Generic;

using UIProcess;
using UIPProcess.DataBinding;
using UIPProcess.UIP.Navigators;
using UIPProcess.UIP.Views;

namespace UIPProcess.Controllers
{
    public interface IEntitiesListFilter
    {
        bool IsFiltered { get; }
    }

    /// <summary>
    /// The base class for Filter Views controllers.
    /// </summary>
    /// <typeparam name="TFilter">The Filter class for processing.</typeparam>
    public class FilterViewController<TFilter> : WebCtlControllerBase<IWebControl>
        where TFilter : IEntitiesListFilter, new()
    {
        #region [ Constructors ]

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navigator"></param>
        public FilterViewController(Navigator navigator)
            : base(navigator)
        { }

        #endregion

        /// <summary>
        /// This function should be overwritten to load the System Parameters,
        /// necessary for the filter.
        /// </summary>
        protected virtual void LoadSysParameters() { }
        
        /// <summary>
        /// Create filter object. Could be overwritten to perform additional 
        /// default initialization of the Filter.
        /// </summary>
        /// <returns></returns>
        protected virtual TFilter CreateFilterObject()
        {
            return new TFilter();
        }

        public override void OnLoadView(object control, EventArgs e)
        {
            base.OnLoadView(control, e);

            //if (IsShouldStopProcessing(control))
            //    return;

            if (Filter == null)
                Filter = CreateFilterObject();

            LoadSysParameters();
        }

        public virtual void OnApply(Object control)
        {
            if (Filter == null)
                Filter = CreateFilterObject();

            DataBinder.DataBindFromControlAttributedProps(control, Filter);

            FilterChanged = true;
        }

        public virtual void OnReset(Object control)
        {
            Filter = CreateFilterObject();
            FilterChanged = true;
        }

        public override void OnUnloadView(object control, EventArgs e)
        {
            base.OnUnloadView(control, e);

            FilterChanged = false;
        }

        /// <summary>
        /// <b>State mapped property.</b> Used to store the filter object.
        /// </summary>
        public virtual TFilter Filter
        {
            get { return default(TFilter); }
            set { }
        }

        /// <summary>
        /// <b>State mapped property.</b> Filter Changed flag is used to notify the 
        /// view.
        /// </summary>
        public virtual Boolean FilterChanged
        {
            get { return false; }
            set { }
        }    
    }
}