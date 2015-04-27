using System;

using UIPProcess.Controllers;
using UIPProcess.UIP.Views.Page;

namespace UIPProcess.UIP.Views
{
    /// <summary>
    /// Base View class for Web Controls (*.ascx) which have Controllers for actions processing.
    /// </summary>
    public class ControlViewBase : WebFormControlView, IWebControl
    {
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                Page.LoadComplete += ControlViewBase_LoadComplete;
            }
            catch (Exception)
            {
                RedirectToDefaultPage();
            }
        }

        /// <summary>
        /// Redirect application to default page. Used if session is closed by time out.
        /// </summary>
        protected virtual void RedirectToDefaultPage()
        {
            //Page.Response.Redirect("~/Default.aspx");
        }

        /// <summary>
        /// This method is obsolete and should be deleted!!!
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            WebCtlControllerBase<IWebControl> controller =
                Controller as WebCtlControllerBase<IWebControl>;
            if (controller == null)
                return;

            controller.OnPageLoadingEnd(this);
        }

        /// <summary>
        /// This method is obsolete and should be deleted!!!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ControlViewBase_LoadComplete(Object sender, EventArgs e)
        {
            WebCtlControllerBase<IWebControl> controller = Controller as WebCtlControllerBase<IWebControl>;
            if (controller == null)
                return;

            controller.OnPageLoadCompleted(this);
        }

        #region Control Properties

        /*public ControlTitleType TitleType
        {
            get { return _titleType; }
            set { _titleType = value; }
        }
        private ControlTitleType _titleType = ControlTitleType.titleVisible;

        public ControlMenuType MenuType
        {
            get { return _menuType; }
            set { _menuType = value; }
        }
        private ControlMenuType _menuType = ControlMenuType.menuVisible;
        */

        #endregion

        #region IUserControlUIInterface Members

        public virtual Boolean Validate()
        {
            return true;
        }

        public IPageUIInterface PageInterface
        {
            get
            {
                IPageUIInterface page = Page as IPageUIInterface;
                System.Diagnostics.Debug.Assert(page != null);
                return page;
            }
        }

        public virtual String ActionsMenuClientId
        {
            get { return _actionsMenuId; }
            set { _actionsMenuId = value; }
        }
        private String _actionsMenuId = String.Empty;

        public String GetFormFieldValue(String strFieldName)
        {
            return Request.Form[strFieldName];
        }

        public String GetCommandValue(String strCmdName)
        {
            return Request[strCmdName];
        }

        #endregion

        /// <summary>
        /// State mapped property.
        /// </summary>
        public virtual object CurrentUser
        {
            get { return PageInterface.CurrentWorkingUser; }
        }
    }
}