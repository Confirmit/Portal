//===============================================================================
// IT Co. User Interface Process Application Block for .NET
//
// WebFormView.cs
//
// This file contains the implementations of the WebFormControl class.
//
// For more information see the User Interface Process Application Block Implementation Overview. 
// 
//===============================================================================
// Copyright (C) 2007-2008 IT Co. Limited
// All rights reserved.
//==============================================================================

using System;

using UIProcess;
using UIPProcess.UIP.Navigators;
using UIPProcess.UIP.ViewManagers;

using UIPProcess.Controllers;

namespace UIPProcess.UIP.Views
{
    /// <summary>
    /// Represents a view used in Web applications.
    /// </summary>
    public class WebFormView : System.Web.UI.Page, IView
    {
        #region Declares variables

        private ControllerBase _controller;
        private SessionMoniker _sessionMoniker;
        
        /// <summary>
        /// The QueryString key used to get the current task identifier.
        /// </summary>
        public const string CurrentTaskKey = "CurrentTask";
        
        #endregion

        #region IView implementation 

        /// <summary>
        /// Gets the view controller.
        /// </summary>
        public ControllerBase Controller
        {
            get { return _controller; }
        }

        /// <summary>
        /// Gets the task identifier related to this view.
        /// </summary>
        public Guid TaskId 
        {
            get{ return _sessionMoniker.TaskId; }
        }

        /// <summary>
        /// Gets the view name.
        /// </summary>
        public string ViewName
        {
            get{ return _sessionMoniker.CurrentViewName; }
        }

        /// <summary>
        /// Gets the navigator used by this WebFormView.
        /// </summary>
        public Navigator Navigator
        {
            get { return null; }
        }

        /// <summary>
        /// No implementation for this in WebFormViews.
        /// </summary>
        /// <param name="enabled"></param>
        public void Enable(bool enabled) 
        {}
        
        /// <summary>
        /// Initializes the WebFormView.
        /// </summary>
        /// <param name="args">The initialization arguments.</param>
        /// <param name="settings">The settings for the view.</param>
        public virtual void Initialize(TaskArgumentsHolder args, ViewSettings settings)
        {}

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _sessionMoniker = GetSessionMoniker();
            Navigator navigator = GetNavigator(_sessionMoniker.NavGraphName, _sessionMoniker.TaskId);

            if (viewSettings != null
                && viewSettings.AppTransactionStartConfigured
                && !IsPostBack)
            {
                navigator.CurrentState.Clear();
            }

            _controller = navigator.GetController(this, string.Empty);
            if (_controller == null)
                return;

            Load += _controller.OnLoadView;
            PreRender += _controller.OnPreRenderView;
            Unload += _controller.OnUnloadView;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (_controller == null || viewSettings == null)
                return;

            ViewKeysMapper.MapKeysToView(viewSettings, this, _controller.State);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            if (_controller != null)
            {
                Load -= _controller.OnLoadView;
                PreRender -= _controller.OnPreRenderView;
                Unload -= _controller.OnUnloadView;
            }
        }

        private ViewSettings viewSettings
        {
            get
            {
                ViewSettings settings = null;
                if (!String.IsNullOrEmpty(ViewName))
                    settings = UIPConfiguration.Config.GetViewSettingsFromName(ViewName);

                if (settings == null)
                {
                    Type type = GetType();
                    String type_name = type.BaseType.Name;
                    //Reminder. Is the extension of the file name is really needed?
                    settings = UIPConfiguration.Config.GetViewSettingsFromType(type_name + ".aspx");
                }

                return settings;
                //return UIPConfiguration.Config.GetViewSettingsFromName(ViewName);
            }
        }

        private SessionMoniker GetSessionMoniker()
        {
            try
            {
                SessionMoniker sessionMoniker = SessionMoniker.GetFromSession(new Guid(Session[CurrentTaskKey].ToString()));
                return sessionMoniker;
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(
                    string.Format("ExceptionNullSessionMoniker: {0}.", ex.Message));
            }
        }

        private Navigator GetNavigator(string navigationGraphName, Guid taskId)
        {
            return new OpenNavigator(navigationGraphName, taskId);
        }

        public string ViewType
        {
            get { return URLManager.CutLeftPart(Request.CurrentExecutionFilePath); }
        }
    }
}