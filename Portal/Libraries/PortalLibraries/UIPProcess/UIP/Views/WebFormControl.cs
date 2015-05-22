//===============================================================================
// IT Co. User Interface Process Application Block for .NET
//
// WebFormControl.cs
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
using System.Diagnostics;
using System.Web.UI;

using UIProcess;
using UIPProcess.UIP.Navigators;
using UIPProcess.UIP.ViewManagers;

namespace UIPProcess.UIP.Views
{
    /// <summary>
    /// Represents a composite user control without Controller used in Web applications.
    /// </summary>
    public class WebFormControl : UserControl
    {
        /// <summary>
        /// The name for JavaScript client side controller. Helper property for common use
        /// to be sure that all controls use the same name.
        /// </summary>
        public String ControllerJSObjectName
        {
            get { return String.Format("controller_{0}", ClientID); }
        }

        /// <summary>
        /// The script to initialize the client side controller.
        /// </summary>
        protected virtual String InitControllerScript
        {
            get { return String.Empty; }
        }

        /// <summary>
        /// The name for JavaScript client side control. Helper property for common use
        /// to be sure that all controls use the same name.
        /// </summary>
        public virtual String ClientJSObjectName
        {
            get { return String.Format("obj_{0}", ClientID); }
        }

        /// <summary>
        /// The script to initialize the client side control.
        /// </summary>
        protected virtual String InitClientObjectScript
        {
            get { return String.Empty; }
        }

        private SessionMoniker _sessionMoniker;
        protected Navigator _navigator = null;

        public const string CurrentTaskKey = "CurrentTask";

        /// <summary>
        /// The server-side State of the application, used to store the Domain and UI support 
        /// objects during User's session.
        /// </summary>
        public State State
        {
            get 
            {
                Debug.Assert(_navigator != null);
                return _navigator.CurrentState; 
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _sessionMoniker = GetSessionMoniker();
            _navigator = GetNavigator(_sessionMoniker.NavGraphName, _sessionMoniker.TaskId);        
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (viewSettings == null)
                return;

            ViewKeysMapper.MapKeysToView(viewSettings, this, State);
        }

        private SessionMoniker GetSessionMoniker()
        {
            try 
            {
                if (Session[CurrentTaskKey] == null)
                    UIPManager.StartOpenNavigationTask("new");

                SessionMoniker sessionMoniker = SessionMoniker.GetFromSession(new Guid(Session[CurrentTaskKey].ToString()));
                return sessionMoniker;
            }
            catch (NullReferenceException ex)
            {
                throw new Exception(string.Format("ExceptionNullSessionMoniker: {0}.", ex.Message));
            }        
        }

        protected Navigator GetNavigator(string navigationGraphName, Guid taskId)
        {
            return new OpenNavigator(navigationGraphName, taskId);
        }

        /// <summary>
        /// Gets the task identifier related to this view.
        /// </summary>
        public Guid TaskId
        {
            get { return _sessionMoniker.TaskId; }
        }

        /// <summary>
        /// Map the property of the view to particular key on the state.
        /// </summary>
        /// <param name="key">The name of the key.</param>
        /// <param name="property">The name of the property.</param>
        public void MapPropertyToKey(String key, String property)
        {
            if (viewSettings == null)
                throw new Exception(string.Format("ExceptionInvalidConfigForView: {0}.", GetType()));

            viewSettings.MapPropertyToKey(key, property);
        }

        /// <summary>
        /// Return the name of the key to which this property is mapped to.
        /// </summary>
        /// <param name="property">The name of the key.</param>
        /// <returns></returns>
        public String GetKeyByProperty(String property)
        {
            if (viewSettings == null)
                throw new Exception(string.Format("ExceptionInvalidConfigForView: {0}.", GetType()));

            return viewSettings.GetKeyByProperty(property); 
        }

        /// <summary>
        /// Set the view of the name to find it in configuration
        /// </summary>
        public virtual String ViewName
        {
            get 
            {
                if (viewSettings == null)
                    return string.Empty;
                
                return viewSettings.Name; 
            }
            set { _viewName = value; }
        }
        private String _viewName = String.Empty;

        private ViewSettings viewSettings
        {
            get
            {
                ViewSettings settings = null;
                if (!String.IsNullOrEmpty(_viewName))
                    settings = UIPConfiguration.Config.GetViewSettingsFromName(_viewName);

                if (settings == null) 
                {
                    Type type = GetType();
                    String type_name = type.BaseType.Name;
                    //Reminder. Is the extension of the file name is really needed?
                    settings = UIPConfiguration.Config.GetViewSettingsFromType(type_name + ".ascx");
                }

                return settings;
            }
        }
    }
}