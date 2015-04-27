//===============================================================================
// IT Co. User Interface Process Application Block for .NET
//
// ControllerFactory.cs
//
// This file contains the implementations of the ControllerFactory class.
//
// For more information see the User Interface Process Application Block Implementation Overview. 
// 
//===============================================================================
// Copyright (C) 2007-2008 IT Co. Limited
// All rights reserved.
//==============================================================================

using System;
using System.Web;
using System.Collections;
using System.Globalization;

using UIPProcess.UIP.Navigators;
using UIPProcess.UIP.Views;
using UIProcess;

namespace UIPProcess.UIP.ViewManagers
{
    /// <summary>
    /// Internal class used to store session information.
    /// </summary>
    internal class SessionMoniker
    {
        #region Variable Declarations

        private const string ColonSeparator = ":";
        private const string SessionTaskMonikerKey = "TaskMoniker";

        private string _navigationGraph;
        private string _currentview;
        private Guid _taskId;
        
        #endregion

        #region Constructors

        public SessionMoniker(string navigationGraphName, string currentViewName, Guid taskId)
        {
            _navigationGraph = navigationGraphName;
            _currentview = currentViewName;
            _taskId = taskId;
        }

        public SessionMoniker(string navigationGraphName, string currentViewName, string taskId)
        {
            _navigationGraph = navigationGraphName;
            _currentview = currentViewName;
            _taskId = new Guid(taskId);
        }

        public SessionMoniker(string taskMoniker)
        {
            //  split string on colons
            string[] armoniker = taskMoniker.Split(ColonSeparator.ToCharArray());

            //  check if it has expected 3 items
            if (2 == armoniker.GetUpperBound(0))
            {
                _navigationGraph = armoniker[0];
                _currentview = armoniker[1];
                _taskId = new Guid(armoniker[2]);
            }
            else { throw new ArgumentException(string.Format("ExceptionIncorrectNumberOfItemsInTaskMonikerString")); }
        }

        #endregion

        #region Static helper methods

        /// <summary>
        /// Tests a moniker retrieved from context to see if it matches the NavGraphName:CurrentViewName:TaskGuid pattern.
        /// </summary>
        private static bool IsMonikerValid(string moniker)
        {
            //  check if it's null or zero length first
            if ((null == moniker) || (0 == moniker.Length))
                return false;

            //  potential security risk, this input comes from user
            string[] armon = moniker.Split(ColonSeparator.ToCharArray());

            //  check for correct # elements
            if (!(2 == armon.GetUpperBound(0)))
                return false;

            //  put into string args
            string navgraph = armon[0];
            string view = armon[1];
            string task = armon[2];

            //  check lengths
            if ((navgraph.Length < 255) && (view.Length < 255) && (task.Length == 36))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the session moniker related to the specified task.
        /// </summary>
        /// <param name="taskId">The task identifier (a GUID associated with the task). The session moniker for this task will be retrieved.</param>
        public static SessionMoniker GetFromSession(Guid taskId)
        {
            string stringMoniker = (string)HttpContext.Current.Session[SessionTaskMonikerKey + taskId];
            if (IsMonikerValid(stringMoniker))
                return new SessionMoniker(stringMoniker);
            else
                throw new Exception(string.Format("ExceptionCantGetSessionMoniker: {0}.", taskId));
        }

        /// <summary>
        /// Gets all session monikers actually stored in the user session.
        /// </summary>
        /// <returns>An array of session monikers.</returns>
        public static SessionMoniker[] GetAllFromSession()
        {
            ArrayList monikers = new ArrayList();
            foreach (string key in HttpContext.Current.Session.Keys)
            {
                if (key.StartsWith(SessionTaskMonikerKey))
                {
                    SessionMoniker sm = (SessionMoniker)HttpContext.Current.Session[key];
                    monikers.Add(sm);
                }
            }

            return (SessionMoniker[])monikers.ToArray(typeof(SessionMoniker));
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Stores the session moniker in the user session.
        /// </summary>
        public void StoreInSession()
        {
            HttpContext.Current.Session[SessionTaskMonikerKey + TaskId] = ToString();
        }
        
        #endregion

        #region Public Properties Get/Set

        public string NavGraphName
        {
            get { return _navigationGraph; }
            set { _navigationGraph = value; }
        }

        public string CurrentViewName
        {
            get { return _currentview; }
            set { _currentview = value; }
        }

        public Guid TaskId
        {
            get { return _taskId; }
            set { _taskId = value; }
        }

        #endregion

        #region ToString Override

        public override string ToString()
        {
            return _navigationGraph + ":" + _currentview + ":" + _taskId;
        }
        
        #endregion
    }

    /// <summary>
    /// Provides methods to manipulate WebForm views. 
    /// </summary>
    public class WebFormViewManager : IViewManager
    {
        #region IViewManager Members

        /// <summary>
        /// Activates a specific view.
        /// </summary>
        /// <param name="previousView">The view actually displayed.</param>		
        /// <param name="view">The name of the view to be displayed.</param>
        /// <param name="navigator">The navigator requesting the transition.</param>
        /// <param name="args">Not used in this implementation of ActivateView.</param>
        public void ActivateView(string previousView, string view, Navigator navigator, TaskArgumentsHolder args)
        {
            ActivateView(previousView, view, navigator);
        }

        /// <summary>
        /// Activates a specific view.
        /// </summary>
        /// <param name="previousView">The view actually displayed.</param>		
        /// <param name="view">The name of the view to be displayed.</param>
        /// <param name="navigator">The navigator requesting the transition.</param>		
        public virtual void ActivateView(string previousView, string view, Navigator navigator)
        {
            ActivateView(previousView, navigator.CurrentState.TaskId, navigator.Name, view);
        }

        /// <summary>
        /// Activates a specific view.
        /// </summary>
        /// <param name="previousView">The view actually displayed.</param>
        /// <param name="taskId">An existing task identifier (a GUID associated with the task).</param>
        /// <param name="navGraph">A configured navigation graph name.</param>
        /// <param name="view">The name of the view to be displayed.</param>
        public virtual void ActivateView(string previousView, Guid taskId, string navGraph, string view)
        {
            //  create a session moniker
            var sessionMoniker = new SessionMoniker(navGraph, view, taskId);

            // store the moniker into the session, so the next view can get the task information
            sessionMoniker.StoreInSession();

            Int32 nQuerySign = view.LastIndexOf('?');
            String queryParams = (nQuerySign == -1) ? "" : view.Substring(nQuerySign, view.Length - nQuerySign);
            ViewSettings viewSettings = UIPConfiguration.Config.GetViewSettingsFromName(view);
            if (viewSettings == null)
                throw new Exception(string.Format("ExceptionViewConfigNotFound: {0}.", view));

            HttpContext.Current.Session[WebFormView.CurrentTaskKey] = taskId.ToString();
            redirectToNextView(previousView, viewSettings, queryParams);
        }

        protected virtual void redirectToNextView(string previousView, ViewSettings viewSettings, String queryParams)
        {
            try
            {
                // когда последний параметр = true, то система не всегда ведет себя корректно 
                // 
                //if( previousView == null )
                //    HttpContext.Current.Response.Redirect( HttpContext.Current.Request.ApplicationPath
                //        + "/" + viewSettings.Type + queryParams, true);
                //else

                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.ApplicationPath
                    + "/" + viewSettings.Type + queryParams, false);
                //HttpContext.Current.Response.Redirect(
                //  ApplicationPath.GetFullVirtualPath(viewSettings.Type + queryParams), false);
            }
            catch (System.Threading.ThreadAbortException) { }
        }

        /// <summary>
        /// Stores a property in the view manager. 
        /// Each task has its own properties.
        /// </summary>
        /// <remarks>Property storage is a view manager responsibility.</remarks>
        /// <param name="taskId">The task identifier (a GUID associated with the task).</param>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        public void StoreProperty(Guid taskId, string name, object value)
        { }

        /// <summary>
        /// A utility method that checks Web requests to ensure that the requested page and current view match.
        ///	If a user bookmarks page D, then proceeds to page F, and then returns to the bookmark, the
        ///	state, when loaded, will have F as the current view.  
        /// Any submissions on page D will fail, because the navigation graph may not have appropriate view-navigateResult pairs.
        /// Therefore, you should code defensively against this. Check the current page, check the referring page, andc heck the state object's current view.
        /// </summary>
        /// <param name="view">The next view.</param>
        /// <param name="stateViewName">The view saved in the state.</param>
        /// <returns></returns>
        public bool IsRequestCurrentView(IView view, string stateViewName)
        {
            Int32 nQuerySign = stateViewName.LastIndexOf('?');
            String viewName = (nQuerySign == -1) ? stateViewName : stateViewName.Substring(0, nQuerySign);

            //  get state currentview; must all match
            ViewSettings viewSettings = UIPConfiguration.Config.GetViewSettingsFromName(viewName);
            if (viewSettings == null)
                throw new Exception(string.Format("ExceptionViewConfigNotFound: {0}.", stateViewName));

            String stateViewType = viewSettings.Type;

            ViewSettings viewSettingsView = UIPConfiguration.Config.GetViewSettingsFromType(view.ViewType);
            if (viewSettingsView.IsAjaxWebService || viewSettingsView.IsOpenModal)
                return true;

            return string.Compare(view.ViewType, stateViewType, true, CultureInfo.InvariantCulture) == 0;
        }

        /// <summary>
        /// Returns the name of the view that is being requested by the browser. This method has been implemented
        /// to aid in Back button support functionality.
        /// </summary>
        /// <param name="currentView">View currently executing.</param>
        /// <returns>Name of the view requested by browser.</returns>
        public String GetViewNameForCurrentRequest(IView currentView)
        {
            String viewName = UIPConfiguration.Config.GetViewSettingsFromType(currentView.ViewType).Name;
            return viewName;
        }

        /// <summary>
        /// Gets the running tasks in the manager.
        /// </summary>
        /// <returns>An array with the task identifiers.</returns>
        public Guid[] GetCurrentTasks()
        {
            SessionMoniker[] monikers = SessionMoniker.GetAllFromSession();
            Guid[] tasks = new Guid[monikers.Length];
            for (int index = 0; index < monikers.Length; index++)
            {
                tasks[index] = monikers[index].TaskId;
            }

            return tasks;
        }

        /// <summary>
        /// Not implemented for WebFormViewManager.
        /// </summary>
        /// <returns>Null.</returns>
        public IView GetActiveView()
        {
            return null;
        }

        /// <summary>
        /// Not implemented for WebFormViewManager.
        /// </summary>
        /// <returns>0</returns>
        public int GetActiveViewCount()
        {
            return 0;
        }

        #endregion
    }
}