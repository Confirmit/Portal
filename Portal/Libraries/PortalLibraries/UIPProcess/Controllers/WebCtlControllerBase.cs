using System;

using UIPProcess.Controllers;
using UIPProcess.UIP.Navigators;
using UIPProcess.UIP.Views;

namespace UIPProcess.Controllers
{
    public class WebCtlControllerBase<T> : ControllerBase
    {
        public WebCtlControllerBase(Navigator navigator)
            : base(navigator)
        {}

        public override void OnLoadView(Object sender, EventArgs e)
        {
            base.OnLoadView(sender, e);

            IWebControl control = sender as IWebControl;
            if (control == null)
                throw new Exception(
                    string.Format("IncorrectUsage: {0} - {1} - {2}."
                                  , GetType(), sender.GetType(), "IWebControl"));

            _controlProcessed = control;
        }

        /// <summary>
        /// Control, processed during current request.
        /// </summary>
        protected IWebControl ProcessedControl
        {
            get { return _controlProcessed; }
        }
        private IWebControl _controlProcessed = null;

        /// <summary>
        /// State mapped property. Could be used to store visual state of the control during 
        /// transferring through different pages.
        /// </summary>
        public virtual Boolean IsControlVisible
        {
            get { return false; }
            set { ; }
        }

        /// <summary>
        /// This event occurs after controls are bounded to events and used
        /// especially to load objects from database and fill session with objects
        /// </summary>
        /// <param name="control"></param>
        public virtual void OnPageLoadingEnd(T control)
        {
        }

        /// <summary>
        /// This event occurs in pages only at the moment when all controls are loaded
        /// and bound to events.
        /// </summary>
        /// <param name="control"></param>
        public virtual void OnPageLoadCompleted(T control)
        {
        }
    }
}