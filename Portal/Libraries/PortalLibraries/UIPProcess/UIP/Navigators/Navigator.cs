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

using UIProcess;
using UIPProcess.UIP.ViewManagers;
using UIPProcess.UIP.Factories;
using UIPProcess.UIP.Views;
using UIPProcess.Controllers;

namespace UIPProcess.UIP.Navigators
{
    /// <summary>
    /// Navigators provide navigation services to controllers, and start tasks.
    /// </summary>
    public abstract class Navigator
    {
        #region Fields

        private IViewManager _viewManager;
        private string _name;
        private State _state;

        #endregion

        #region Properties

        /// <summary>
        /// The view manager used. 
        /// </summary>
        public IViewManager ViewManager 
        {
            get { return _viewManager; }
            set { _viewManager = value; }
        }

        /// <summary>
        /// The name of the navigator.
        /// </summary>
        public string Name 
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The current state of the navigator.
        /// </summary>
        public State CurrentState 
        {
            get { return _state; }
        }

        /// <summary>
        /// Used to set the state during construction of a navigator.
        /// </summary>
        /// <param name="state">State object for this navigator.</param>
        protected void SetState(State state)
        {
            _state = state;
        }

        /// <summary>
        /// The expiration mode of the state cache for this task.
        /// <see cref="CacheExpirationMode"/>
        /// </summary>
        public abstract CacheExpirationMode CacheExpirationMode 
        {
            get;
        }

        /// <summary>
        /// The interval used to expire entries in the state cache.
        /// </summary>
        public abstract TimeSpan CacheExpirationInterval 
        {
            get;
        }

        #endregion

        /// <summary>
        /// Navigates to the next node.
        /// </summary>
        /// <param name="nextNode">The node or view that will display next.</param>
        public abstract void Navigate(String nextNode); 

        /// <summary>
        /// Creates the controller for this view.
        /// </summary>
        /// <param name="view">The view that needs a controller.</param>
        /// <param name="controllerName">The view controller name.</param>
        /// <returns></returns>
        public ControllerBase GetController(IView view, string controllerName) 
        {						
            ActivateViewInStateIfNecessary(view);
            ControllerBase controller = GetControllerForView(view, controllerName);

            //controller.StartTask += OnStartTask;
            return controller;
        }
		
        private void ActivateViewInStateIfNecessary(IView view)
        {
            if (_viewManager == null)
                return;

            if (! _viewManager.IsRequestCurrentView(view,_state.CurrentView))
            {
                if (! UIPConfiguration.Config.AllowBackButton)
                    _viewManager.ActivateView(null,_state.CurrentView,this);
                else
                {
                    if (RunningInNavGraph)
                    {
                        if (! UIPConfiguration.Config.ViewExistsInNavigationGraph(_state.NavigationGraph,_viewManager.GetViewNameForCurrentRequest(view)))
                            _viewManager.ActivateView(null,_state.CurrentView,this);
                    }
                }
            }
        }

        private ControllerBase GetControllerForView(IView view, string controllerName)
        {
            string viewForController = view.ViewName;

            if (_viewManager != null
                && !_viewManager.IsRequestCurrentView(view, _state.CurrentView))	
            {	
                if (UIPConfiguration.Config.AllowBackButton)								
                {
                    viewForController = _viewManager.GetViewNameForCurrentRequest(view);
                    if (RunningInNavGraph)
                    {
                        if (UIPConfiguration.Config.ViewExistsInNavigationGraph(_state.NavigationGraph,viewForController))						
                            _state.CurrentView = viewForController;						
                    }	
                    else
                    {
                        _state.CurrentView = viewForController;
                    }
                }					
            }

            if (string.IsNullOrEmpty(viewForController) && string.IsNullOrEmpty(controllerName))
                return null;

            return String.IsNullOrEmpty(controllerName)
                       ? ControllerFactory.Create(viewForController, this)
                       : ControllerFactory.CreateByName(controllerName, this);
        }

        private bool RunningInNavGraph
        {
            get { return (_state.NavigationGraph != null && _state.NavigationGraph.Trim().Length > 0); }
        }

        ///// <summary>
        ///// Handles any StartTask events raised by a controller.
        ///// </summary>
        ///// <param name="sender">Controller that raised the event.</param>
        ///// <param name="e">Arguments used by the event handler.</param>
        //public virtual void OnStartTask(object sender, StartTaskEventArgs e)
        //{
        //    if (UIPConfiguration.Config.ContainsNavigationGraphSettings(e.NextNavigationGraph)) 
        //    {
        //        /*NavigationGraphSettings settings = UIPConfiguration.Config.GetNavigationGraphSettings(e.NextNavigationGraph);
        //        if (settings.RunInWizardMode)
        //        {
        //            WizardNavigator navigator = new WizardNavigator(e.NextNavigationGraph);
        //            navigator.StartTask(e.NextTask, e.TaskArguments);
        //        }
        //        else*/
        //        {
        //            GraphNavigator navigator = new GraphNavigator(e.NextNavigationGraph);
        //            navigator.StartTask(e.NextTask, e.TaskArguments);
        //        }
				
				
        //        return;
        //    } 
        //    else if (UIPConfiguration.Config.ContainsHostedControlsSettings(e.NextNavigationGraph)) 
        //    {
        //        UserControlsNavigator navigator = new UserControlsNavigator(e.NextNavigationGraph);
        //        navigator.StartTask(e.NextTask);
        //        return;
        //    }
        //    OpenNavigator newNavigator = new OpenNavigator("new");
        //    newNavigator.StartTask(e.NextNavigationGraph, e.NextTask);
        //}

        /// <summary>
        /// Creates or loads state for this task.
        /// </summary>
        /// <param name="taskId">The identifier of the task to load or create state for.</param>
        /// <returns></returns>
        protected State GetState(Guid taskId) 
        {
            State _state = null;

            if ( Guid.Empty == taskId ) 
            {
                // CASE (2) 
                // OK, the application has not pre-set a task. Therefore tell application what the new TaskId is, 
                // and internally the client app will use it...for example, by creating an entry 
                // in its DB lookup table to correlate logon with Task.

                // set up the new State object, since we know now that we're on a new Task so we need new State
                _state = CreateState();
                // new State now contains new TaskID, get it here
                taskId = _state.TaskId;
                // tell the taskObject (which is our sink back into client application) about the new TaskID
            }
            else
            {
                // CASE
                // in this case, ITask was not null, AND it returned a valid TaskID...
                // SO this is a known Task...and there is a State for it. Retrieve that State.
                _state = LoadState(taskId);
            }

            return _state;
        }

        /// <summary>
        /// Creates a State object using the type specified in the configuration file for this navigator.
        /// Navigators can override this if they need to create state in a different way.
        /// </summary>
        /// <returns>The state that was created.</returns>
        protected virtual State CreateState() 
        {
            return StateFactory.Create(Name);
        }

        /// <summary>
        /// Loads a State object using the type specified in the configuration file for this navigator and task ID.
        /// Navigators can override this if they need to load state in a different way.
        /// </summary>
        /// <param name="taskId">The identifier of the task to load the state for.</param>
        /// <returns>The state that was created.</returns>		
        protected virtual State LoadState(Guid taskId) 
        {
            return StateFactory.Load( Name, taskId );
        }
    }
}