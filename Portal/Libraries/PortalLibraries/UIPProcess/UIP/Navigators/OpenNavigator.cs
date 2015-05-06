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
using System.Collections.Generic;
using System.Web;

using UIProcess;
using UIPProcess.UIP.Factories;
using UIPProcess.UIP.ViewManagers;
using UIPProcess.UIP.Views;
using UIPProcess.Controllers;

namespace UIPProcess.UIP.Navigators
{
    /// <summary>
    /// Navigator used when transitions between views are made. To use this navigator, specify the view names 
    /// to transition to.
    /// </summary>
    public class OpenNavigator : Navigator
    {
        private ViewSettings _startView;
        private CacheConfiguration  _cacheConfiguration;

        /// <summary>
        /// Overrides. Initializes a new OpenNavigator.
        /// </summary>
        /// <param name="name">The name of the navigation graph to which open navigation applies.</param>
        public OpenNavigator(string name) 
        {
            Name = name;
            ViewManager = ViewManagerFactory.Create();
            SetState(StateFactory.Create());
            _cacheConfiguration = UIPConfiguration.Config.GetCacheConfiguration();
        }

        /// <summary>
        /// Overloaded. Initializes a new OpenNavigator.
        /// </summary>
        /// <param name="name">The name of the navigation graph to which open navigation applies.</param>
        /// <param name="taskId">The task identifier (a GUID associated with the task).</param>
        public OpenNavigator(string name, Guid taskId)
        {
            Name = name;
            ViewManager = ViewManagerFactory.Create();
            SetState(StateFactory.Load(name, taskId));
            _cacheConfiguration = UIPConfiguration.Config.GetCacheConfiguration();
        }

        /// <summary>
        /// Gets the Timespan object that represents the life span of the OpenNavigator.
        /// </summary>
        public override TimeSpan CacheExpirationInterval
        {
            get { return _cacheConfiguration.Interval; }
        }

        /// <summary>
        /// Gets the <see cref="UIProcess.CacheExpirationMode"/> for the OpenNavigator.
        /// </summary>
        public override CacheExpirationMode CacheExpirationMode
        {
            get { return _cacheConfiguration.Mode; }
        }
        
        /// <summary>
        /// Activates the next view.
        /// </summary>
        /// <param name="nextView">The name of the next view to be activated.</param>
        public override void Navigate(string nextView) 
        {
            if (ViewManager == null)
                return;

            string previousView = CurrentState.CurrentView;
            CurrentState.CurrentView = nextView;
            CurrentState.NavigateValue = "";

            UIPManager.InvokeEventHandlers(CurrentState);
            CurrentState.Save();
            try
            {
                ViewManager.ActivateView(previousView, nextView, this);
            }
            catch(System.Threading.ThreadAbortException) 
            {}
            catch( Exception ex )
            {
                throw new Exception(string.Format("ExceptionCantActivateView: {0} - {1}.", nextView, ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstView">The name of the first view.</param>
        /// <param name="state_params"></param>
        public void StartTaskWithParams(string firstView, params KeyValuePair<String, Object>[] state_params)
        {
            SetState(CreateState());
            foreach (KeyValuePair<string, object> pair in state_params)
            {
                CurrentState[pair.Key] = pair.Value;
            }

            TaskArgumentsHolder args = null;
            StartTask(firstView, args);
        }

        public void StartTask()
        {
            SetState(CreateState());
            //CurrentState.CurrentView = _startView.Name;
            CurrentState.Save();

            SessionMoniker sessionMoniker = new SessionMoniker(string.Empty, CurrentState.CurrentView, CurrentState.TaskId);
            sessionMoniker.StoreInSession();
            HttpContext.Current.Session[WebFormView.CurrentTaskKey] = CurrentState.TaskId.ToString();
        }

        /// <summary>
        /// Overloaded. Starts open navigation, beginning with specified first view.
        /// </summary>
        /// <param name="firstView">The name of the first view.</param>
        public void StartTask(string firstView) 
        {
            StartTask(firstView, null);
        }

        /// <summary>
        /// Overloaded. Starts open navigation for the specified task identifier.
        /// </summary>
        /// <param name="taskId">The task identifier (a GUID associated with the task).</param>
        public void StartTask(Guid taskId) 
        {
            StartTask(taskId, null);
        }

        /// <summary>
        /// Overloaded. Starts open navigation for the specified task identifier.
        /// </summary>
        /// <param name="taskId">The task identifier (a GUID associated with the task).</param>
        /// <param name="args">Additional navigation arguments.</param>
        public void StartTask(Guid taskId, TaskArgumentsHolder args) 
        {
            SetState(GetState(taskId));
            StartTask(string.Empty, args);
        }

        /// <summary>
        /// Overloaded. Starts open navigation beginning with the first view.
        /// </summary>
        /// <param name="firstView">The name of the first view.</param>
        /// <param name="args">Additional navigation arguments.</param>
        public void StartTask(string firstView, TaskArgumentsHolder args) 
        {
            SetState(CreateState());

            string startViewName = null;
            if (!string.IsNullOrEmpty(CurrentState.CurrentView)) 
            {
                startViewName = CurrentState.CurrentView;
                _startView = UIPConfiguration.Config.GetViewSettingsFromName(CurrentState.CurrentView);
            } 
            else if (!string.IsNullOrEmpty(firstView)) 
            {
                startViewName = firstView;
                _startView = UIPConfiguration.Config.GetViewSettingsFromName(firstView);
            }
            if (_startView == null)
                throw new Exception(string.Format("ExceptionViewConfigNotFound: {0}.", startViewName));

            StartTask(args);
        }

        private void StartTask(TaskArgumentsHolder args) 
        {
            ControllerBase firstController = ControllerFactory.Create(_startView.Name, this);
            firstController.EnterTask(null);
            CurrentState.CurrentView = _startView.Name;
            CurrentState.Save();
            try
            {
                ViewManager.ActivateView(null, _startView.Name, this, args);
            }
            catch(System.Threading.ThreadAbortException) {}
            catch( Exception ex )
            {
                throw new Exception(string.Format("ExceptionCantActivateView: {0} - {1}.", _startView.Name, ex.Message));
            }
			
        }

        /// <summary>
        /// Overrides. Creates a new <see cref="State"/> for OpenNavigation.
        /// </summary>
        /// <returns></returns>
        protected override State CreateState()
        {
            return StateFactory.Create();
        }

        /// <summary>
        /// Overrides. Loads the <see cref="State"/> of the OpenNavigator for the given task ID.
        /// </summary>
        /// <param name="taskId">The task identifier (a GUID associated with the task).</param>
        /// <returns>The loaded state.</returns>
        protected override State LoadState(Guid taskId)
        {
            return StateFactory.Load(taskId);
        }
    }
}