using System;
using System.Diagnostics;
using System.Web;
using System.Web.UI;

using UIPProcess.UIP.Views;

namespace UIPProcess.UIP.Views.Page
{
    public abstract class PageViewBase : WebFormView, IPageUIInterface
    {
        public PageViewBase()
        {
            LoadComplete += PageViewBase_LoadComplete;
            PreRenderComplete += PageViewBase_PreRenderComplete;
        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
            }
            catch (NullReferenceException)
            {
                RedirectToDefaultPage();
            }
        }

        /// <summary>
        /// Redirect application to default page. Used if session is closed by time out.
        /// </summary>
        protected virtual void RedirectToDefaultPage()
        {
            //Response.Redirect("~/Default.aspx");
        }

        #region IPageUIInterface Members

        public MasterPage MasterPage
        {
            get { return Master; }
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

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                HttpResponse response = Response;
                response.Cache.SetCacheability(HttpCacheability.NoCache);

                //if (!IsMainMenuNavigate())
                {
                    /*  PageControllerBase controller = (PageControllerBase) Controller;
                    controller.OnPreReConnectStartSession(this);
                    controller.OnAfterReConnectStartSession(this);

                    controller.OnPageLoadingEnd(this);*/
                }

                processErrorIfAny();
            }
            catch (Exception exc)
            {
                Debug.Assert(false, exc.Message);
            }
        }

        protected virtual void PageViewBase_LoadComplete(Object sender, EventArgs e)
        {
            try
            {
                /*PageControllerBase controller = (PageControllerBase) Controller;
                controller.OnPageLoadCompleted(this);
                ControlsViewBase_LoadComplete(this, e);*/
            }
            catch (Exception exc)
            {
                Debug.Assert(false, exc.Message);
            }
        }

        protected virtual void PageViewBase_PreRenderComplete(Object sender, EventArgs e)
        {
            try
            {
                //PageControllerBase controller = (PageControllerBase) Controller;
                //controller.OnPagePreRenderComplete(this);
            }
            catch (Exception exc)
            {
                Debug.Assert(false, exc.Message);
            }
        }

        protected static void ControlsViewBase_LoadComplete(Control sender, EventArgs e)
        {
            foreach (Control control in sender.Controls)
            {
                //if (control is ControlViewBase)
                //    ((ControlViewBase) control).ControlViewBase_LoadComplete(control, e);
                ControlsViewBase_LoadComplete(control, e);
            }
        }

        #region Error processing

        protected static String ErrorText = "";

        private void processErrorIfAny()
        {
            if (!String.IsNullOrEmpty(ErrorText))
            {
                ShowError(ErrorText);
                ErrorText = "";
            }
            else
                HideError();
        }

        protected virtual void ShowError(String message)
        { }

        protected virtual void HideError()
        { }

        #endregion

        #region IPageUIInterface Members

        public virtual object CurrentWorkingUser
        {
            get { return null; }
        }

        #endregion
    }
}